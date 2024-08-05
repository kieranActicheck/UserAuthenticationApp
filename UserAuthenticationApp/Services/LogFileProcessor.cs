using System.Text.RegularExpressions;
using UserAuthenticationApp.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace UserAuthenticationApp.Services
{
    /// <summary>
    /// Background service to process log files and store log entries in the database.
    /// </summary>
    public class LogFileProcessor : BackgroundService
    {
        private readonly ILogger<LogFileProcessor> _logger;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly string _logFilePath;

        /// <summary>
        /// Initialises a new instance of the <see cref="LogFileProcessor"/> class.
        /// </summary>
        /// <param name="logger">The logger instance.</param>
        /// <param name="scopeFactory">The service scope factory.</param>
        /// <param name="configuration">The configuration instance.</param>
        public LogFileProcessor(ILogger<LogFileProcessor> logger, IServiceScopeFactory scopeFactory, IConfiguration configuration)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
            _logFilePath = configuration.GetValue<string>("LogFilePath", "Logs/log.txt");
        }

        /// <summary>
        /// Executes the background service to process log files.
        /// </summary>
        /// <param name="stoppingToken">A token that indicates when the background service should stop.</param>
        /// <returns>A task that represents the background service execution.</returns>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("LogFileProcessor started.");
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    if (File.Exists(_logFilePath))
                    {
                        _logger.LogInformation("Processing log file: {LogFilePath}", _logFilePath);

                        var lines = await File.ReadAllLinesAsync(_logFilePath, stoppingToken);
                        using (var scope = _scopeFactory.CreateScope())
                        {
                            var context = scope.ServiceProvider.GetRequiredService<LogContext>();
                            int entriesAdded = 0;
                            foreach (var line in lines)
                            {
                                _logger.LogInformation("Processing line: {Line}", line);
                                var match = Regex.Match(line, @"\bMessage (\d+) sent on ""([^""]+)"" response time ([\d.]+) min ([\d.]+) max ([\d.]+)\b");
                                if (match.Success)
                                {
                                    try
                                    {
                                        var logEntry = new LogEntry
                                        {
                                            MessageNumber = int.Parse(match.Groups[1].Value),
                                            Timestamp = DateTime.Parse(match.Groups[2].Value),
                                            ResponseTime = double.Parse(match.Groups[3].Value),
                                            MinResponseTime = double.Parse(match.Groups[4].Value),
                                            MaxResponseTime = double.Parse(match.Groups[5].Value)
                                        };

                                        // Validate parsed values
                                        if (logEntry.ResponseTime < 0 || logEntry.MinResponseTime < 0 || logEntry.MaxResponseTime < 0)
                                        {
                                            _logger.LogWarning("Invalid response times in line: {Line}", line);
                                            continue;
                                        }

                                        _logger.LogInformation("Adding log entry: {LogEntry}", logEntry);
                                        context.LogEntries.Add(logEntry);
                                        entriesAdded++;
                                    }
                                    catch (FormatException ex)
                                    {
                                        _logger.LogError(ex, "Error parsing line: {Line}", line);
                                    }
                                    catch (Exception ex)
                                    {
                                        _logger.LogError(ex, "Unexpected error processing line: {Line}", line);
                                    }
                                }
                                else
                                {
                                    _logger.LogDebug("Line did not match expected format: {Line}", line);
                                }
                            }
                            _logger.LogInformation("Saving {EntriesAdded} entries to the database.", entriesAdded);
                            await context.SaveChangesAsync(stoppingToken);
                        }
                        _logger.LogInformation("Deleting log file: {LogFilePath}", _logFilePath);
                        File.Delete(_logFilePath); // Optionally delete the log file after processing
                    }
                }
                catch (IOException ex)
                {
                    _logger.LogError(ex, "An IO error occurred while processing the log file.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while processing the log file.");
                }
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken); // Adjust the delay as needed
            }
            _logger.LogInformation("LogFileProcessor stopped.");
        }
    }
}
