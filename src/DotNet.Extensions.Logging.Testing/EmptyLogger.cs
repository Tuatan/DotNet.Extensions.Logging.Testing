using System;
using Microsoft.Extensions.Logging;

namespace DotNet.Extensions.Logging.Testing
{
    /// <summary>
    /// Empty logger implementation that only validates the input.
    /// </summary>
    public class EmptyLogger : ILogger
    {
        /// <summary>
        /// Default immutable instance of <see cref="EmptyLogger"/> class.
        /// </summary>
        public static readonly EmptyLogger Default = new EmptyLogger();

        /// <summary>
        /// Checks if the given <paramref name="logLevel" /> is enabled.
        /// </summary>
        /// <param name="logLevel">level to be checked.</param>
        /// <returns><c>true</c> if enabled.</returns>
        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        /// <summary>Writes a log entry.</summary>
        /// <param name="logLevel">Log level.</param>
        /// <param name="eventId">Identifier of the log entry.</param>
        /// <param name="state">The object that represents the state for the log entry.</param>
        /// <param name="exception">The exception object.</param>
        /// <param name="formatter">The message formatter function.</param>
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!this.IsEnabled(logLevel))
                return;

            if (formatter == null)
            {
                throw new ArgumentNullException(nameof(formatter));
            }

            formatter(state, exception);
        }

        /// <summary>Begins a logical operation scope.</summary>
        /// <param name="state">The identifier for the scope.</param>
        /// <returns>An <see cref="IDisposable"/> that ends the logical operation scope on dispose.</returns>
        public IDisposable BeginScope<TState>(TState state)
        {
            return EmptyDisposable.Default;
        }
    }

    /// <summary>
    /// Empty logger implementation that only validates the input.
    /// </summary>
    public sealed class EmptyLogger<T> : EmptyLogger, ILogger<T>
    {
    }
}
