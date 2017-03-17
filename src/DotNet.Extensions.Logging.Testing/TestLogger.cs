using System;
using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;

namespace DotNet.Extensions.Logging.Testing
{
    /// <summary>
    /// Test logger implementation that accumulates all log entries.
    /// </summary>
    public class TestLogger : ILogger, IObserver<LogEvent>
    {
        private readonly Func<string, LogLevel, bool> filter;
        private readonly ObservableLogger logger;
        private readonly ConcurrentQueue<LogEvent> logEntries = new ConcurrentQueue<LogEvent>();

        /// <summary>
        /// Creates an instance of <see cref="TestLogger"/> class.
        /// </summary>
        /// <param name="name">Name of the logger instance.</param>
        /// <param name="filter">Filter function.</param>
        public TestLogger(string name, Func<string, LogLevel, bool> filter)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));
            if (filter == null) throw new ArgumentNullException(nameof(filter));

            this.Name = name;
            this.filter = filter;

            this.logger = new ObservableLogger(name, filter, this);
        }

        /// <summary>
        /// Checks if the given <paramref name="logLevel" /> is enabled.
        /// </summary>
        /// <param name="logLevel">level to be checked.</param>
        /// <returns><c>true</c> if enabled.</returns>
        public bool IsEnabled(LogLevel logLevel)
        {
            return this.logger.IsEnabled(logLevel);
        }

        /// <summary>Writes a log entry.</summary>
        /// <param name="logLevel">Log level.</param>
        /// <param name="eventId">Identifier of the log entry.</param>
        /// <param name="state">The object that represents the state for the log entry.</param>
        /// <param name="exception">The exception object.</param>
        /// <param name="formatter">The message formatter function.</param>
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            this.logger.Log(logLevel, eventId, state, exception, formatter);
        }

        /// <summary>Begins a logical operation scope.</summary>
        /// <param name="state">The identifier for the scope.</param>
        /// <returns>An <see cref="IDisposable"/> that ends the logical operation scope on dispose.</returns>
        public IDisposable BeginScope<TState>(TState state)
        {
            return this.logger.BeginScope(state);
        }

        internal string Name { get; }

        /// <summary>
        /// Get the collected log entries.
        /// </summary>
        /// <returns>Collected log entries.</returns>
        public LogEvent[] GetLogs()
        {
            return this.logEntries.ToArray();
        }

        /// <summary>
        /// Checks if there are no logs collected.
        /// </summary>
        /// <returns><c>true</c> if there are no logs collected.</returns>
        public bool IsEmpty => this.logEntries.Count == 0;

        /// <summary>
        /// Checks if there are any error (and above) logs collected.
        /// </summary>
        /// <returns><c>true</c> if there are any error (and above) logs collected.</returns>
        public bool HasErrors { get; internal set; }

        /// <summary>
        /// Checks if there are any warning (and above) logs collected.
        /// </summary>
        /// <returns><c>true</c> if there are any warning (and above) logs collected.</returns>
        public bool HasWarnings { get; internal set; }

        void IObserver<LogEvent>.OnCompleted()
        {
        }

        void IObserver<LogEvent>.OnError(Exception error)
        {
        }

        void IObserver<LogEvent>.OnNext(LogEvent value)
        {
            if (!this.HasErrors)
            {
                if (value.Level == LogLevel.Critical || value.Level == LogLevel.Error)
                {
                    this.HasErrors = true;
                    this.HasWarnings = true;
                }
            }
            if (!this.HasWarnings)
            {
                if (value.Level == LogLevel.Warning)
                {
                    this.HasWarnings = true;
                }
            }

            this.logEntries.Enqueue(value);
        }
    }

    /// <summary>
    /// Test logger implementation that accumulates all log entries.
    /// </summary>
    public sealed class TestLogger<T> : TestLogger, ILogger<T>
    {
        /// <summary>
        /// Creates an instance of <see cref="TestLogger"/> class.
        /// </summary>
        /// <param name="filter">Filter function.</param>
        public TestLogger(Func<string, LogLevel, bool> filter) 
            : base(typeof(T).FullName, filter)
        {
        }
    }
}
