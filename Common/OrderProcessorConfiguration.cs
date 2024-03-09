namespace CbData.Interview.Common
{
    /// <summary>
    /// Represents a configuration for <see cref="OrderProcessor"/> service.
    /// </summary>
    public class OrderProcessorConfiguration
    {
        /// <summary>
        /// Gets or sets a processor working period.
        /// </summary>
        public TimeSpan Period { get; set; } = TimeSpan.FromSeconds(20);

        /// <summary>
        /// Gets or sets a number of re-try count.
        /// </summary>
        public int ReTryCount { get; set; } = 1;

        /// <summary>
        /// Gets or sets a sleep duration period.
        /// </summary>
        public TimeSpan ReTrySleepDuration { get; set; } = TimeSpan.FromSeconds(1);
    }
}
