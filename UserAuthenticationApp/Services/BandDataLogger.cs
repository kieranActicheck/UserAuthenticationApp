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
            _logger.LogInformation($"DeviceId: {_bandData.DeviceId}");
            _logger.LogInformation($"DateOfPacket: {_bandData.DateOfPacket}");
            _logger.LogInformation($"Key1: {_bandData.Key1}");
            _logger.LogInformation($"Key2: {_bandData.Key2}");
            _logger.LogInformation($"BlueToothLink: {_bandData.BlueToothLink}");
            _logger.LogInformation($"Temperature: {_bandData.Temperature}");
            _logger.LogInformation($"Presence: {_bandData.Presence}");
            _logger.LogInformation($"AccelX: {_bandData.AccelX}");
            _logger.LogInformation($"AccelY: {_bandData.AccelY}");
            _logger.LogInformation($"AccelZ: {_bandData.AccelZ}");
            _logger.LogInformation($"MovementData: {_bandData.MovementData}");
            _logger.LogInformation($"BatteryVoltage: {_bandData.BatteryVoltage}");
            _logger.LogInformation($"Link: {_bandData.Link}");
            _logger.LogInformation($"Link2: {_bandData.Link2}");
            _logger.LogInformation($"Count: {_bandData.Count}");
            _logger.LogInformation($"ISMRadioLink: {_bandData.ISMRadioLink}");
            _logger.LogInformation($"OnWrist: {_bandData.OnWrist}");
            _logger.LogInformation($"MovementTrigger: {_bandData.MovementTrigger}");
        }
    }
}
