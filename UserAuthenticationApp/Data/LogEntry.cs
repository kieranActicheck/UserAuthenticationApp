namespace UserAuthenticationApp.Data
{
    /// <summary>
    /// Represents a log entry containing message details and response times.
    /// </summary>
    public class LogEntry
    {
        /// <summary>
        /// Gets or sets the unique identifier for the log entry.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the message number.
        /// </summary>
        public int MessageNumber { get; set; }

        /// <summary>
        /// Gets or sets the timestamp of the log entry.
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Gets or sets the response time of the log entry.
        /// </summary>
        public double ResponseTime { get; set; }

        /// <summary>
        /// Gets or sets the minimum response time recorded.
        /// </summary>
        public double MinResponseTime { get; set; }

        /// <summary>
        /// Gets or sets the maximum response time recorded.
        /// </summary>
        public double MaxResponseTime { get; set; }

        /// <summary>
        /// Gets or sets the payload of the log entry.
        /// </summary>
        public string Payload { get; set; }
    }
}
