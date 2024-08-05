using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Abstractions;

namespace UserAuthenticationApp.Data
{
    /// <summary>
    /// Represents the database context for log entries.
    /// </summary>
    public class LogContext : DbContext
    {
        /// <summary>
        /// Gets or sets the DbSet of log entries.
        /// </summary>
        public DbSet<LogEntry> LogEntries { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogContext"/> class.
        /// </summary>
        /// <param name="options">The options to be used by the DbContext.</param>
        public LogContext(DbContextOptions<LogContext> options) : base(options)
        {
        }
    }
}
