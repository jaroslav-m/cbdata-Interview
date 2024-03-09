using CbData.Interview.Abstraction;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Polly.Retry;
using Polly;
using Microsoft.Extensions.Logging;
using CbData.Interview.Abstraction.Model;

namespace CbData.Interview.Common
{
    /// <summary>
    /// Represents a processor that is responsible for an <see cref="Order"/> notification.
    /// </summary>
    public class OrderProcessor : IHostedService, IDisposable
    {
        private CancellationTokenSource _cancellationTokenSource;

        private readonly IModelDataConnector _connector;
        private readonly IOrderNotificationProvider _orderNotificationProvider;
        private readonly IServiceScope _scope;
        private readonly ILogger _logger;
        private readonly OrderProcessorConfiguration _configuration;
        private readonly AsyncRetryPolicy _retryPolicy;
        private bool _inProcessing;

        /// <summary/>
        public OrderProcessor(ILogger<OrderProcessor> logger, IServiceProvider serviceProvider, IOptions<OrderProcessorConfiguration> options)
        {
            _configuration = options.Value;
            _scope = serviceProvider.CreateScope();
            _logger = logger;
            _connector = _scope.ServiceProvider.GetService<IModelDataConnector>()
                ?? throw new Exception("No model data connector is defined.");
            _orderNotificationProvider = _scope.ServiceProvider.GetService<IOrderNotificationProvider>()
                ?? throw new Exception("No order notification provider is defined.");
            _retryPolicy = Policy.Handle<Exception>()
                .WaitAndRetryAsync(_configuration.ReTryCount, c => _configuration.ReTrySleepDuration, OnError);
        }

        /// <inheritdoc/>
        public async Task StartAsync(CancellationToken cancellationToken)
        {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            StartInternalAsync(cancellationToken);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

            await Task.CompletedTask;
        }

        private async void StartInternalAsync(CancellationToken cancellationToken)
        {
            _cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            
            while (!_cancellationTokenSource.IsCancellationRequested)
            {
                if (!_inProcessing)
                {
                    _inProcessing = true;
                    await ProcessOrdersAsync(cancellationToken);
                    _inProcessing = false;
                }
                await Task.Delay(_configuration.Period, cancellationToken);
            }
        }

        /// <inheritdoc/>
        public Task StopAsync(CancellationToken cancellationToken)
        {
            _cancellationTokenSource.Cancel();
            return Task.CompletedTask;
        }

        private async Task ProcessOrdersAsync(CancellationToken cancellationToken)
        {
            // TODO: implement circuit breaker pattern
            var orders = _connector.Query<Order>()
                .Where(o => !o.IsAnnounced)
                .ToArray()
                .GroupBy(o => o.ProductId);

            foreach (var orderGroup in orders)
            {
                try
                {
                    var orderToReport = new Order
                    {
                        ProductId = orderGroup.Key,
                        Quantity = orderGroup.Sum(o => o.Quantity)
                    };
                    await _retryPolicy.ExecuteAsync(token => _orderNotificationProvider.ProcessOrderAsync(orderToReport, token), cancellationToken);
                    foreach (var order in orderGroup)
                    {
                        order.IsAnnounced = true;
                        _connector.Update(order);
                    }                   
                    _connector.Commit();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to process order, Product Id: {0}.", orderGroup.Key);
                }
            }
        }

        private void OnError(Exception exception, TimeSpan span)
        {
            _logger.LogWarning(exception, "An error occurred while processing the order.");
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            _scope.Dispose();
        }
    }
}
