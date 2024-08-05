using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using UserAuthenticationApp.Data;

namespace UserAuthenticationApp.Controllers
{
    /// <summary>
    /// Controller to handle data logging operations.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class DataController : ControllerBase
    {
        private readonly ILogger<DataController> _logger;
        private readonly LogContext _context;
        private static int _messageCount = 0;
        private static double _minResponseTime = double.MaxValue;
        private static double _maxResponseTime = double.MinValue;

        /// <summary>
        /// Initialises a new instance of the <see cref="DataController"/> class.
        /// </summary>
        /// <param name="logger">The logger instance.</param>
        /// <param name="context">The database context instance.</param>
        public DataController(ILogger<DataController> logger, LogContext context)
        {
            _logger = logger;
            _context = context;
        }

        /// <summary>
        /// Receives log data and updates the response time statistics.
        /// </summary>
        /// <param name="logData">The log data containing timestamp, response time, and payload.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.</returns>
        [HttpPost("receive")]
        public IActionResult Receive([FromBody] LogData logData)
        {
            if (logData == null || logData.Timestamp == DateTime.MinValue || logData.ResponseTime == 0)
            {
                return BadRequest("Invalid log data");
            }

            _messageCount++;
            double responseTime = logData.ResponseTime;
            _minResponseTime = Math.Min(_minResponseTime, responseTime);
            _maxResponseTime = Math.Max(_maxResponseTime, responseTime);

            _logger.LogInformation("{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [INF] Message {MessageNumber} sent on \"{Timestamp:yyyy-MM-ddTHH:mm:ss.fffffff}\" response time {ResponseTime} min {MinResponseTime} max {MaxResponseTime} payload {Payload}",
                DateTime.Now, _messageCount, logData.Timestamp, responseTime, _minResponseTime, _maxResponseTime, logData.Payload);

            // Save log data to the database
            var logEntry = new LogEntry
            {
                MessageNumber = _messageCount,
                Timestamp = logData.Timestamp,
                ResponseTime = responseTime,
                MinResponseTime = _minResponseTime,
                MaxResponseTime = _maxResponseTime,
                Payload = logData.Payload
            };

            try
            {
                _context.LogEntries.Add(logEntry);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while saving the log entry.");
                return StatusCode(500, "Internal server error");
            }

            return Ok(new
            {
                logData.Timestamp,
                MinResponseTime = _minResponseTime,
                MaxResponseTime = _maxResponseTime
            });
        }
    }

    /// <summary>
    /// Represents the log data containing timestamp, response time, and payload.
    /// </summary>
    public class LogData
    {
        /// <summary>
        /// Gets or sets the timestamp of the log data.
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Gets or sets the response time of the log data.
        /// </summary>
        public double ResponseTime { get; set; }

        /// <summary>
        /// Gets or sets the payload of the log data.
        /// </summary>
        public string Payload { get; set; }
    }
}
