using CbData.Interview.Abstraction;
using CbData.Interview.Abstraction.Model;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CbData.Interview.Common
{
    /// <summary>
    /// Represents the <see cref="IOrderNotificationProvider"/> which just log an order to console.
    /// </summary>
    internal class OrderNotificationProvider : IOrderNotificationProvider
    {
        private readonly ILogger _logger;
        private readonly Random _random = new();

        /// <summary/>
        public OrderNotificationProvider(ILogger<OrderNotificationProvider> logger)
        {
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task ProcessOrderAsync(Order order, CancellationToken cancellationToken)
        {
            //SimulateError();

            var jsonString = JsonConvert.SerializeObject(order);
            _logger.LogDebug("Sending order, {0}", jsonString);

            await Task.CompletedTask;
        }

        private void SimulateError()
        {
            if (_random.Next(0, 2) == 1)
                throw new Exception("Service is unavailable");
        }
    }
}
