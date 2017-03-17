using System;
using Microsoft.Extensions.Logging;

namespace DotNet.Extensions.Logging.Testing
{
    /// <summary>
    /// Logger implementation that publishes all log entries to the specified IObserver.
    /// </summary>
    public class ObservableLogger : ILogger, IDisposable
    {
        private IObserver<LogEvent> _logObserver;
        private readonly Func<string, LogLevel, bool> filter;

        /// <summary>
        /// Creates an instance of <see cref="ObservableLogger"/> class.
        /// </summary>
        /// <param name="name">Name of the logger instance.</param>
        /// <param name="filter">Filter function.</param>
        /// <param name="logObserver">Observer that receives log entries.</param>
        public ObservableLogger(string name, Func<string, LogLevel, bool> filter, IObserver<LogEvent> logObserver)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }
            if (filter == null)
            {
                throw new ArgumentNullException(nameof(filter));
            }
            if (logObserver == null)
            {
                throw new ArgumentNullException(nameof(logObserver));
            }

            this.Name = name;
            this.filter = filter;
            _logObserver = logObserver;
        }

        /// <summary>
        /// Checks if the given <paramref name="logLevel" /> is enabled.
        /// </summary>
        /// <param name="logLevel">level to be checked.</param>
        /// <returns><c>true</c> if enabled.</returns>
        public bool IsEnabled(LogLevel logLevel)
        {
            return this.filter(this.Name, logLevel);
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
                throw new ArgumentNullException(nameof(formatter));

            string message = formatter(state, exception);

            if (string.IsNullOrEmpty(message) && exception == null)
                return;

            var @event = new LogEvent
            {
                Name = this.Name,
                Message = message,
                Error = exception,
                Level = logLevel,
                Id = eventId,
                ScopeData = LogScope.GetState(),
                State = state
            };

            this._logObserver.OnNext(@event);
        }

        /// <summary>Begins a logical operation scope.</summary>
        /// <param name="state">The identifier for the scope.</param>
        /// <returns>An <see cref="IDisposable"/> that ends the logical operation scope on dispose.</returns>
        public IDisposable BeginScope<TState>(TState state)
        {
            return LogScope.Push(this.Name, state);
        }

        internal string Name { get; }

        public void Dispose()
        {
            if (this._logObserver != null)
            {
                try
                {
                    this._logObserver.OnCompleted();
                }
                catch
                {
                    // ignored
                }
                this._logObserver = null;
            }
        }
    }

    /// <summary>
    /// Logger implementation that publishes all log entries to the specified IObserver.
    /// </summary>
    public sealed class ObservableLogger<T> : ObservableLogger, ILogger<T>
    {
        /// <summary>
        /// Creates an instance of <see cref="ObservableLogger"/> class.
        /// </summary>
        /// <param name="filter">Filter function.</param>
        /// <param name="logObserver">Observer that receives log entries.</param>
        public ObservableLogger(Func<string, LogLevel, bool> filter, IObserver<LogEvent> logObserver) 
            : base(typeof(T).FullName, filter, logObserver)
        {
        }
    } 
}