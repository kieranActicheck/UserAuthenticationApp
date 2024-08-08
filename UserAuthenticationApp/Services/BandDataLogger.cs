using UserAuthenticationApp.Data;
using Microsoft.Extensions.Logging;

namespace UserAuthenticationApp.Services
{
    /// <summary>
    /// Provides logging functionality for BandData properties.
    /// </summary>
    public class BandDataLogger
    {
        private readonly BandData _bandData;
        private readonly ILogger _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="BandDataLogger"/> class.
        /// </summary>
        /// <param name="bandData">The BandData instance to log.</param>
        /// <param name="logger">The logger instance to use for logging.</param>
        public BandDataLogger(BandData bandData, ILogger logger)
        {
            _bandData = bandData;
            _logger = logger;
        }

        /// <summary>
        /// Logs the properties of the BandData instance.
        /// </summary>
        public void LogBandData()
        {
            if (_bandData == null)
            {
                _logger.LogInformation("BandData is null.");
                return;
            }
            _logger.LogInformation("Logging BandData properties:");
            _logger.LogInformation($"ID: {_bandData.ID}");
            _logger.LogInformation($"DeviceId: {_bandData.DeviceId}");
            _logger.LogInformation($"Status: {_bandData.Status}");
            _logger.LogInformation($"Temperature: {_bandData.Temperature}");
            _logger.LogInformation($"Presence: {_bandData.Presence}");
            _logger.LogInformation($"Keys: {_bandData.Keys}");
            _logger.LogInformation($"MovementData: {_bandData.MovementData}");
            _logger.LogInformation($"AccelX: {_bandData.AccelX}");
            _logger.LogInformation($"AccelY: {_bandData.AccelY}");
            _logger.LogInformation($"AccelZ: {_bandData.AccelZ}");
            _logger.LogInformation($"Voltage: {_bandData.Voltage}");
            _logger.LogInformation($"Link: {_bandData.Link}");
            _logger.LogInformation($"Link2: {_bandData.Link2}");
            _logger.LogInformation($"Fallmode: {_bandData.Fallmode}");
            _logger.LogInformation($"Count: {_bandData.Count}");
            _logger.LogInformation($"Passcode: {_bandData.Passcode}");
            _logger.LogInformation($"OnWrist: {_bandData.OnWrist}");
            _logger.LogInformation($"DateOfPacket: {_bandData.DateOfPacket}");
            _logger.LogInformation($"MovementTrigger: {_bandData.MovementTrigger}");
        }
    }
}
