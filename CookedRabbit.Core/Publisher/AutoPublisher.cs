using CookedRabbit.Core.Pools;
using CookedRabbit.Core.Utils;
using System;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace CookedRabbit.Core
{
    public interface IAutoPublisher
    {
        bool Compress { get; }
        Config Config { get; }
        bool CreatePublishReceipts { get; }
        bool Encrypt { get; }
        bool Initialized { get; }
        IPublisher Publisher { get; }
        bool Shutdown { get; }

        ChannelReader<PublishReceipt> GetReceiptBufferReader();
        ValueTask QueueLetterAsync(Letter letter, bool priority = false);
        Task SetProcessReceiptsAsync(Func<PublishReceipt, Task> processReceiptAsync);
        Task SetProcessReceiptsAsync<TIn>(Func<PublishReceipt, TIn, Task> processReceiptAsync, TIn inputObject);
        Task StartAsync(byte[] hashKey = null);
        Task StopAsync(bool immediately = false);
    }

    public class AutoPublisher : IAutoPublisher
    {
        public Config Config { get; }
        public IPublisher Publisher { get; }
        private Channel<Letter> LetterQueue { get; set; }
        private Channel<Letter> PriorityLetterQueue { get; set; }
        private Task PublishingTask { get; set; }
        private Task PublishingPriorityTask { get; set; }
        private Task ProcessReceiptsAsync { get; set; }

        private readonly SemaphoreSlim pubLock = new SemaphoreSlim(1, 1);

        public bool Initialized { get; private set; }
        public bool Shutdown { get; private set; }

        public bool Compress { get; private set; }
        public bool Encrypt { get; private set; }
        public bool CreatePublishReceipts { get; private set; }
        private byte[] HashKey { get; set; }
        private bool _withHeaders;

        public AutoPublisher(Config config, bool withHeaders = true)
        {
            Guard.AgainstNull(config, nameof(config));

            Config = config;
            Publisher = new Publisher(Config);
            _withHeaders = withHeaders;
        }

        public AutoPublisher(IChannelPool channelPool, bool withHeaders = true)
        {
            Guard.AgainstNull(channelPool, nameof(channelPool));

            Config = channelPool.Config;
            Publisher = new Publisher(channelPool);
            _withHeaders = withHeaders;
        }

        public async Task StartAsync(byte[] hashKey = null)
        {
            await pubLock.WaitAsync().ConfigureAwait(false);

            try
            {
                CreatePublishReceipts = Config.PublisherSettings.CreatePublishReceipts;
                Compress = Config.PublisherSettings.Compress;
                Encrypt = Config.PublisherSettings.Encrypt;

                if (Encrypt && (hashKey == null || hashKey.Length != 32)) throw new InvalidOperationException(Strings.EncrypConfigErrorMessage);
                HashKey = hashKey;

                await Publisher.ChannelPool.InitializeAsync().ConfigureAwait(false);

                LetterQueue = Channel.CreateBounded<Letter>(
                    new BoundedChannelOptions(Config.PublisherSettings.LetterQueueBufferSize)
                    {
                        FullMode = Config.PublisherSettings.BehaviorWhenFull
                    });
                PriorityLetterQueue = Channel.CreateBounded<Letter>(
                    new BoundedChannelOptions(Config.PublisherSettings.PriorityLetterQueueBufferSize)
                    {
                        FullMode = Config.PublisherSettings.BehaviorWhenFull
                    });

                PublishingTask = Task.Run(() => ProcessDeliveriesAsync(LetterQueue.Reader).ConfigureAwait(false));
                PublishingPriorityTask = Task.Run(() => ProcessDeliveriesAsync(PriorityLetterQueue.Reader).ConfigureAwait(false));

                Initialized = true;
                Shutdown = false;
            }
            finally { pubLock.Release(); }
        }

        public async Task StopAsync(bool immediately = false)
        {
            await pubLock.WaitAsync().ConfigureAwait(false);

            try
            {
                LetterQueue.Writer.Complete();
                PriorityLetterQueue.Writer.Complete();
                Publisher.ReceiptBuffer.Writer.Complete();

                if (!immediately)
                {
                    await LetterQueue
                        .Reader
                        .Completion
                        .ConfigureAwait(false);

                    await PriorityLetterQueue
                        .Reader
                        .Completion
                        .ConfigureAwait(false);

                    while (!PublishingTask.IsCompleted)
                    {
                        await Task.Delay(10).ConfigureAwait(false);
                    }

                    while (!PublishingPriorityTask.IsCompleted)
                    {
                        await Task.Delay(10).ConfigureAwait(false);
                    }
                }

                Shutdown = true;
            }
            finally
            { pubLock.Release(); }
        }

        // TODO: Simplify usage. Add a memorycache failures for optional / automatic republish.
        public ChannelReader<PublishReceipt> GetReceiptBufferReader() => Publisher.ReceiptBuffer.Reader;

        public async ValueTask QueueLetterAsync(Letter letter, bool priority = false)
        {
            if (!Initialized || Shutdown) throw new InvalidOperationException(Strings.InitializeError);

            if (priority)
            {
                if (!await PriorityLetterQueue
                     .Writer
                     .WaitToWriteAsync()
                     .ConfigureAwait(false))
                {
                    throw new InvalidOperationException(Strings.QueueChannelError);
                }

                await PriorityLetterQueue
                    .Writer
                    .WriteAsync(letter)
                    .ConfigureAwait(false);
            }
            else
            {
                if (!await LetterQueue
                     .Writer
                     .WaitToWriteAsync()
                     .ConfigureAwait(false))
                {
                    throw new InvalidOperationException(Strings.QueueChannelError);
                }

                await LetterQueue
                    .Writer
                    .WriteAsync(letter)
                    .ConfigureAwait(false);
            }
        }

        private async Task ProcessDeliveriesAsync(ChannelReader<Letter> channelReader)
        {
            while (await channelReader.WaitToReadAsync().ConfigureAwait(false))
            {
                while (channelReader.TryRead(out var letter))
                {
                    if (letter == null)
                    { continue; }

                    if (Compress)
                    {
                        letter.Body = await Gzip.CompressAsync(letter.Body).ConfigureAwait(false);
                        letter.LetterMetadata.Compressed = Compress;
                    }

                    if (Encrypt && (HashKey != null || HashKey.Length == 0))
                    {
                        letter.Body = AesEncrypt.Encrypt(letter.Body, HashKey);
                        letter.LetterMetadata.Encrypted = Encrypt;
                    }

                    await Publisher
                        .PublishAsync(letter, CreatePublishReceipts, _withHeaders)
                        .ConfigureAwait(false);
                }
            }
        }

        public async Task SetProcessReceiptsAsync(Func<PublishReceipt, Task> processReceiptAsync)
        {
            await pubLock.WaitAsync().ConfigureAwait(false);

            try
            {
                if (ProcessReceiptsAsync == null && processReceiptAsync != null)
                {
                    ProcessReceiptsAsync = Task.Run(async () =>
                    {
                        await foreach (var receipt in GetReceiptBufferReader().ReadAllAsync())
                        {
                            await processReceiptAsync(receipt).ConfigureAwait(false);
                        }
                    });
                }
            }
            finally { pubLock.Release(); }
        }

        public async Task SetProcessReceiptsAsync<TIn>(Func<PublishReceipt, TIn, Task> processReceiptAsync, TIn inputObject)
        {
            await pubLock.WaitAsync().ConfigureAwait(false);

            try
            {
                if (ProcessReceiptsAsync == null && processReceiptAsync != null)
                {
                    ProcessReceiptsAsync = Task.Run(async () =>
                    {
                        await foreach (var receipt in GetReceiptBufferReader().ReadAllAsync())
                        {
                            await processReceiptAsync(receipt, inputObject).ConfigureAwait(false);
                        }
                    });
                }
            }
            finally { pubLock.Release(); }
        }
    }
}
