using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

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
        private static int _messageCount = 0;
        private static double _minResponseTime = double.MaxValue;
        private static double _maxResponseTime = double.MinValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataController"/> class.
        /// </summary>
        /// <param name="logger">The logger instance.</param>
        public DataController(ILogger<DataController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Receives log data and updates the response time statistics.
        /// </summary>
        /// <param name="logData">The log data containing timestamp and response time.</param>
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

            _logger.LogInformation("Message {MessageNumber} sent on {Timestamp} response time {ResponseTime} min {MinResponseTime} max {MaxResponseTime}",
                _messageCount, logData.Timestamp, responseTime, _minResponseTime, _maxResponseTime);

            return Ok(new
            {
                logData.Timestamp,
                MinResponseTime = _minResponseTime,
                MaxResponseTime = _maxResponseTime
            });
        }
    }

    /// <summary>
    /// Represents the log data containing timestamp and response time.
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
    }
}
