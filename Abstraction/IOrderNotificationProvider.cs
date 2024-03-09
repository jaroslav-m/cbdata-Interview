using CbData.Interview.Abstraction.Model;

namespace CbData.Interview.Abstraction
{
    /// <summary>
    /// Represents a provider that is responsible for orders notification.
    /// </summary>
    public interface IOrderNotificationProvider
    {
        /// <summary>
        /// Process an <see cref="Order"/>.
        /// </summary>
        /// <param name="order">The <see cref="Order"/> instance to be processed.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task ProcessOrderAsync(Order order, CancellationToken cancellationToken);
    }
}
