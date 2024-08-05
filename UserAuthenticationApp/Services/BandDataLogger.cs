using UserAuthenticationApp.Data;

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
        /// Initialises a new instance of the <see cref="BandDataLogger"/> class.
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
            _logger.LogInformation("Logging BandData properties:");
            _logger.LogInformation($"NeedConfig: {_bandData.NeedConfig}");
            _logger.LogInformation($"PassCode: {_bandData.PassCode}");
            _logger.LogInformation($"BandEncKey: {_bandData.BandEncKey}");
            _logger.LogInformation($"NeedPassCode: {_bandData.NeedPassCode}");
            _logger.LogInformation($"NeedKey: {_bandData.NeedKey}");
            _logger.LogInformation($"FallMode: {_bandData.FallModeProperty}");
            _logger.LogInformation($"ShakeAfterBuzz: {_bandData.shakeAfterBuzz}");
            _logger.LogInformation($"PanicButton: {_bandData.panicButton}");
            _logger.LogInformation($"SmokeAlarm: {_bandData.smokeAlarm}");
            _logger.LogInformation($"HeatAlarm: {_bandData.heatAlarm}");
            _logger.LogInformation($"AppBandStatusRequest: {_bandData.appBandStatusRequest}");
            _logger.LogInformation($"IsPanicButton: {_bandData.isPanicButton}");
            _logger.LogInformation($"BandWatchdog: {_bandData.bandWatchdog}");
            _logger.LogInformation($"CosmosData: {_bandData.CosmosData}");
            foreach (var tag in _bandData.Tags)
            {
                _logger.LogInformation($"Tag: {tag.Key} - {tag.Value}");
            }
        }
    }
}
