using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace DotNet.Extensions.Logging.Testing
{
    /// <summary>
    /// Data model object that represents a log entry.
    /// </summary>
    public class LogEvent
    {
        internal LogEvent()
        {            
        }

        /// <summary>
        /// Creates an instance of <see cref="LogEvent"/> class.
        /// </summary>
        /// <param name="name">Category name.</param>
        /// <param name="level">Severity level.</param>
        /// <param name="id">Log's kind identifier.</param>
        /// <param name="message">Formatted log message.</param>
        /// <param name="error">Exception object.</param>
        /// <param name="scopeData">Scope data.</param>
        public LogEvent(
            string name,
            LogLevel level,
            EventId id,
            string message,
            Exception error,
            IReadOnlyCollection<object> scopeData)
        {
            this.Name = name;
            this.Level = level;
            this.Id = id;
            this.Message = message;
            this.Error = error;
            this.ScopeData = scopeData;
        }

        /// <summary>
        /// Category name.
        /// </summary>
        public string Name { get; internal set; }

        /// <summary>
        /// Severity level.
        /// </summary>
        public LogLevel Level { get; internal set; }

        /// <summary>
        /// Log's kind identifier.
        /// </summary>
        public EventId Id { get; internal set; }

        /// <summary>
        /// Formatted log message.
        /// </summary>
        public string Message { get; internal set; }

        /// <summary>
        /// Exception object.
        /// </summary>
        public Exception Error { get; internal set; }

        /// <summary>
        /// Severity level.
        /// </summary>
        public object State { get; internal set; }

        /// <summary>
        /// Scope data.
        /// </summary>
        public IReadOnlyCollection<object> ScopeData { get; internal set; }
    }
}
