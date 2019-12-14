using System;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using CookedRabbit.Core.Configs;

namespace CookedRabbit.Core.Pools
{
    public class ChannelPool
    {
        public Config Config { get; }
        public ConnectionPool ConnectionPool { get; }

        private Channel<ChannelHost> Channels { get; set; }
        private Channel<ChannelHost> AckChannels { get; set; }
        private ConcurrentDictionary<ulong, bool> FlaggedChannels { get; set; }

        public ulong CurrentChannelId { get; private set; }

        public bool Shutdown { get; private set; }

        private bool Initialized { get; set; }
        private readonly SemaphoreSlim poolLock = new SemaphoreSlim(1, 1);

        private const string ValidationMessage = "ChannelPool is not initialized or is shutdown.";
        private const string ShutdownValidationMessage = "ChannelPool is not initialized. Can't be Shutdown.";
        private const string GetChannelError = "Threading.Channel used for reading RabbitMQ channels has been closed.";

        public ChannelPool(Config config)
        {
            ConnectionPool = new ConnectionPool(config);
            Config = config;
        }

        public ChannelPool(ConnectionPool connPool)
        {
            ConnectionPool = connPool;
            Config = connPool.Config;
        }

        public async Task InitializeAsync()
        {
            await poolLock
                .WaitAsync()
                .ConfigureAwait(false);

            if (!Initialized)
            {
                ConfigurePool();

                await ConnectionPool
                    .InitializeAsync()
                    .ConfigureAwait(false);

                await CreateChannelsAsync()
                    .ConfigureAwait(false);

                Initialized = true;
                Shutdown = false;
            }

            poolLock.Release();
        }

        private void ConfigurePool()
        {
            FlaggedChannels = new ConcurrentDictionary<ulong, bool>();
            Channels = Channel.CreateBounded<ChannelHost>(Config.PoolSettings.MaxChannels);
            AckChannels = Channel.CreateBounded<ChannelHost>(Config.PoolSettings.MaxChannels);
        }

        private async Task CreateChannelsAsync()
        {
            for (int i = 0; i < Config.PoolSettings.MaxChannels; i++)
            {
                var connHost = await ConnectionPool
                    .GetConnectionAsync()
                    .ConfigureAwait(false);

                await Channels
                    .Writer
                    .WriteAsync(new ChannelHost(CurrentChannelId++, connHost, false));
            }

            for (int i = 0; i < Config.PoolSettings.MaxChannels; i++)
            {
                var connHost = await ConnectionPool
                    .GetConnectionAsync()
                    .ConfigureAwait(false);

                await AckChannels
                    .Writer
                    .WriteAsync(new ChannelHost(CurrentChannelId++, connHost, true));
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public async ValueTask<ChannelHost> GetChannelAsync()
        {
            if (!Initialized || Shutdown) throw new InvalidOperationException(ValidationMessage);
            if (!await Channels
                .Reader
                .WaitToReadAsync()
                .ConfigureAwait(false))
            {
                throw new InvalidOperationException(GetChannelError);
            }

            var chanHost = await Channels
                .Reader
                .ReadAsync()
                .ConfigureAwait(false);

            var healthy = await chanHost.HealthyAsync().ConfigureAwait(false);
            var flagged = FlaggedChannels.ContainsKey(chanHost.ChannelId) && FlaggedChannels[chanHost.ChannelId];
            if (flagged || !healthy)
            {
                chanHost = await CreateChannelAsync(chanHost.ChannelId, chanHost.Ackable)
                    .ConfigureAwait(false);
            }

            return chanHost;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public async ValueTask<ChannelHost> GetAckChannelAsync()
        {
            if (!Initialized || Shutdown) throw new InvalidOperationException(ValidationMessage);
            if (!await AckChannels
                .Reader
                .WaitToReadAsync()
                .ConfigureAwait(false))
            {
                throw new InvalidOperationException(GetChannelError);
            }

            var chanHost = await AckChannels
                .Reader
                .ReadAsync()
                .ConfigureAwait(false);

            var healthy = await chanHost.HealthyAsync().ConfigureAwait(false);
            var flagged = FlaggedChannels.ContainsKey(chanHost.ChannelId) && FlaggedChannels[chanHost.ChannelId];
            if (flagged || !healthy)
            {
                chanHost = await CreateChannelAsync(chanHost.ChannelId, chanHost.Ackable)
                    .ConfigureAwait(false);
            }

            return chanHost;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private async Task<ChannelHost> CreateChannelAsync(ulong channelId, bool ackable = false)
        {
            ChannelHost chanHost = null;
            ConnectionHost connHost = null;

            while (true)
            {
                var sleep = false;

                try
                {
                    connHost = await ConnectionPool
                        .GetConnectionAsync()
                        .ConfigureAwait(false);

                    if (!await connHost.HealthyAsync().ConfigureAwait(false))
                    { sleep = true; } // TODO: Consider Log?
                }
                catch
                { sleep = true; }

                if (!sleep)
                {
                    try
                    { chanHost = new ChannelHost(channelId, connHost, ackable); }
                    catch
                    { sleep = true; } // TODO: Consider Log?
                }

                if (sleep)
                {
                    await Task
                        .Delay(Config.PoolSettings.SleepOnErrorInterval)
                        .ConfigureAwait(false);

                    continue; // Continue here forever (till reconnection is established).
                }

                break;
            }

            FlaggedChannels[chanHost.ChannelId] = false;
            return chanHost;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public async ValueTask ReturnChannelAsync(ChannelHost chanHost, bool flagChannel = false)
        {
            if (!Initialized || Shutdown) throw new InvalidOperationException(ValidationMessage);

            FlaggedChannels[chanHost.ChannelId] = flagChannel;
            if (chanHost.Ackable)
            {
                await AckChannels
                    .Writer
                    .WriteAsync(chanHost)
                    .ConfigureAwait(false);
            }
            else
            {
                await Channels
                    .Writer
                    .WriteAsync(chanHost)
                    .ConfigureAwait(false);
            }
        }

        public async Task ShutdownAsync()
        {
            if (!Initialized) throw new InvalidOperationException(ShutdownValidationMessage);

            await poolLock
                .WaitAsync()
                .ConfigureAwait(false);

            if (!Shutdown)
            {
                await CloseChannelsAsync()
                    .ConfigureAwait(false);

                Shutdown = true;
                Initialized = false;

                await ConnectionPool
                    .ShutdownAsync()
                    .ConfigureAwait(false);
            }

            poolLock.Release();
        }

        private async Task CloseChannelsAsync()
        {
            Channels.Writer.Complete(); // Signal to Channel no more data is coming.
            AckChannels.Writer.Complete();
#if CORE3
            await foreach (var chanHost in Channels.Reader.ReadAllAsync())
#elif CORE2
            await Channels.Reader.WaitToReadAsync().ConfigureAwait(false);
            while (Channels.Reader.TryRead(out ChannelHost chanHost))
#endif
            {
                try
                { chanHost.Close(); }
                catch { }
            }

#if CORE3
            await foreach (var chanHost in AckChannels.Reader.ReadAllAsync())
#elif CORE2
            await AckChannels.Reader.WaitToReadAsync().ConfigureAwait(false);
            while (AckChannels.Reader.TryRead(out ChannelHost chanHost))
#endif
            {
                try
                { chanHost.Close(); }
                catch { }
            }
        }
    }
}