﻿using System;
using System.Threading;
using System.Threading.Tasks;

namespace CookedRabbit.Core.WorkEngines
{
    public interface IConsumerPipeline<TOut> where TOut : IWorkState
    {
        string ConsumerPipelineName { get; }
        ConsumerOptions Options { get; }

        Task AwaitCompletionAsync();
        Task StartAsync(bool useStream);
        Task StopAsync();
    }

    public class ConsumerPipeline<TOut> : IConsumerPipeline<TOut>, IDisposable where TOut : IWorkState
    {
        public string ConsumerPipelineName { get; }
        public ConsumerOptions Options { get; }

        private IConsumer<ReceivedData> Consumer { get; }
        private IPipeline<ReceivedData, TOut> Pipeline { get; }
        private Task FeedPipelineWithDataTasks { get; set; }
        private TaskCompletionSource<bool> _completionSource;
        private CancellationTokenSource _cancellationTokenSource;
        private bool _started;
        private bool _disposedValue;
        private readonly SemaphoreSlim _cpLock = new SemaphoreSlim(1, 1);

        public ConsumerPipeline(
            IConsumer<ReceivedData> consumer,
            IPipeline<ReceivedData, TOut> pipeline)
        {
            Pipeline = pipeline ?? throw new ArgumentNullException(nameof(pipeline));
            Consumer = consumer ?? throw new ArgumentNullException(nameof(consumer));
            Options = consumer.Options ?? throw new ArgumentNullException(nameof(consumer.Options));
            if (consumer.Options.ConsumerPipelineSettings == null) throw new ArgumentNullException(nameof(consumer.Options.ConsumerPipelineSettings));

            ConsumerPipelineName = !string.IsNullOrWhiteSpace(consumer.Options.ConsumerPipelineSettings.ConsumerPipelineName)
                ? consumer.Options.ConsumerPipelineSettings.ConsumerPipelineName
                : "Unknown";
        }

        public async Task StartAsync(bool useStream)
        {
            await _cpLock.WaitAsync().ConfigureAwait(false);

            try
            {
                if (!_started)
                {
                    _cancellationTokenSource = new CancellationTokenSource();
                    _completionSource = new TaskCompletionSource<bool>();

                    await Consumer
                        .StartConsumerAsync(
                            Options.AutoAck.Value,
                            Options.UseTransientChannels.Value)
                        .ConfigureAwait(false);

                    if (Consumer.Started)
                    {
                        if (useStream)
                        {
                            FeedPipelineWithDataTasks = Task.Run(
                                () =>
                                Consumer.PipelineStreamEngineAsync(
                                    Pipeline,
                                    Options.ConsumerPipelineSettings.WaitForCompletion.Value,
                                    _cancellationTokenSource.Token));
                        }
                        else
                        {
                            FeedPipelineWithDataTasks = Task.Run(
                                () =>
                                Consumer.PipelineExecutionEngineAsync(
                                    Pipeline,
                                    Options.ConsumerPipelineSettings.WaitForCompletion.Value,
                                    _cancellationTokenSource.Token));
                        }

                        _started = true;
                    }
                }
            }
            catch { }
            finally
            { _cpLock.Release(); }
        }

        public async Task StopAsync()
        {
            await _cpLock.WaitAsync().ConfigureAwait(false);

            try
            {
                if (_started)
                {
                    _cancellationTokenSource.Cancel();

                    await Consumer
                        .StopConsumerAsync(false)
                        .ConfigureAwait(false);

                    if (FeedPipelineWithDataTasks != null)
                    {
                        await FeedPipelineWithDataTasks.ConfigureAwait(false);
                        FeedPipelineWithDataTasks = null;
                    }
                    _started = false;
                    _completionSource.SetResult(true);
                }
            }
            catch { }
            finally { _cpLock.Release(); }
        }

        public async Task AwaitCompletionAsync()
        {
            await _completionSource.Task.ConfigureAwait(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _cpLock.Dispose();
                    _cancellationTokenSource.Dispose();
                }

                _disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
