using System;
using Microsoft.EntityFrameworkCore;
using UserAuthenticationApp.Data;

namespace UserAuthenticationApp.Tests
{
    public class TestLogContext : LogContext
    {
        public TestLogContext() : base(new DbContextOptionsBuilder<LogContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options)
        {
            // Initialize any required properties or fields
        }
    }
}
