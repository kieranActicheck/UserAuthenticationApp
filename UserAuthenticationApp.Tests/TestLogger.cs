using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace UserAuthenticationApp.Tests
{
    /// <summary>
    /// A test implementation of the <see cref="ILogger{T}"/> interface that logs messages to a list.
    /// </summary>
    /// <typeparam name="T">The type for which this logger is being created.</typeparam>
    public class TestLogger<T> : ILogger<T>
    {
        /// <summary>
        /// Gets the list of logged messages.
        /// </summary>
        public List<string> LoggedMessages { get; } = new List<string>();

        /// <summary>
        /// Gets or sets a value indicating whether to throw an exception on log.
        /// </summary>
        public bool ThrowExceptionOnLog { get; set; } = false;

        /// <summary>
        /// Begins a logical operation scope.
        /// </summary>
        /// <typeparam name="TState">The type of the state to begin scope for.</typeparam>
        /// <param name="state">The state to begin scope for.</param>
        /// <returns>An <see cref="IDisposable"/> that ends the logical operation scope on dispose.</returns>
        IDisposable ILogger.BeginScope<TState>(TState state) => null!;

        /// <summary>
        /// Checks if the given log level is enabled.
        /// </summary>
        /// <param name="logLevel">The log level to check.</param>
        /// <returns><c>true</c> if the log level is enabled; otherwise, <c>false</c>.</returns>
        public bool IsEnabled(LogLevel logLevel) => true;

        /// <summary>
        /// Logs a message with the given log level and event ID.
        /// </summary>
        /// <typeparam name="TState">The type of the state to log.</typeparam>
        /// <param name="logLevel">The log level.</param>
        /// <param name="eventId">The event ID.</param>
        /// <param name="state">The state to log.</param>
        /// <param name="exception">The exception to log, if any.</param>
        /// <param name="formatter">The function to create a log message from the state and exception.</param>
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            LoggedMessages.Add(formatter(state, exception));
        }
    }
}
