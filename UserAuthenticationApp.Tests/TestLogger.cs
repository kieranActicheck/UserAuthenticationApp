using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace UserAuthenticationApp.Tests
{
    public class TestLogger<T> : ILogger<T>
    {
        public List<string> LoggedMessages { get; } = new List<string>();
        public bool ThrowExceptionOnLog { get; set; } = false;

        public IDisposable BeginScope<TState>(TState state) => null;

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            LoggedMessages.Add(formatter(state, exception));
        }
    }
}
