using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using UserAuthenticationApp.Data;

namespace UserAuthenticationApp.Services
{
    /// <summary>
    /// Background service to process log files and store log entries in the database.
    /// </summary>
    public class LogFileProcessor : BackgroundService
    {
        private readonly ILogger<LogFileProcessor> _logger;
        private readonly LogContext _context;
        private readonly string _logFilePath = "Logs/log.txt";

        /// <summary>
        /// Initialises a new instance of the <see cref="LogFileProcessor"/> class.
        /// </summary>
        /// <param name="logger">The logger instance.</param>
        /// <param name="context">The database context.</param>
        public LogFileProcessor(ILogger<LogFileProcessor> logger, LogContext context)
        {
            _logger = logger;
            _context = context;
        }

        /// <summary>
        /// Executes the background service to process log files.
        /// </summary>
        /// <param name="stoppingToken">A token that indicates when the background service should stop.</param>
        /// <returns>A task that represents the background service execution.</returns>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (File.Exists(_logFilePath))
                {
                    var lines = await File.ReadAllLinesAsync(_logFilePath, stoppingToken);
                    foreach (var line in lines)
                    {
                        var match = Regex.Match(line, @"Message (\d+) sent on ""(.+?)"" response time (\d+\.\d+) min (\d+\.\d+) max (\d+\.\d+)");
                        if (match.Success)
                        {
                            var logEntry = new LogEntry
                            {
                                MessageNumber = int.Parse(match.Groups[1].Value),
                                Timestamp = DateTime.Parse(match.Groups[2].Value),
                                ResponseTime = double.Parse(match.Groups[3].Value),
                                MinResponseTime = double.Parse(match.Groups[4].Value),
                                MaxResponseTime = double.Parse(match.Groups[5].Value)
                            };
                            _context.LogEntries.Add(logEntry);
                        }
                    }
                    await _context.SaveChangesAsync(stoppingToken);
                    File.Delete(_logFilePath); // Optionally delete the log file after processing
                }
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken); // Adjust the delay as needed
            }
        }
    }
}
