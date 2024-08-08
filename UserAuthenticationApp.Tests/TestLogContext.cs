using Microsoft.EntityFrameworkCore;
using UserAuthenticationApp.Data;

namespace UserAuthenticationApp.Tests
{
    /// <summary>
    /// A test-specific implementation of the <see cref="LogContext"/> class that uses an in-memory database.
    /// </summary>
    public class TestLogContext : LogContext
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="TestLogContext"/> class.
        /// </summary>
        public TestLogContext() : base(new DbContextOptionsBuilder<LogContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options)
        {
            // Initialise any required properties or fields
        }
    }
}
