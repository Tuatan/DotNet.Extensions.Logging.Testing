using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace DotNet.Extensions.Logging.Testing
{
    /// <summary>
    /// Logger provider that creates test loggers.
    /// </summary>
    public sealed class TestLoggerProvider : ILoggerProvider
    {
        private ConcurrentDictionary<string, ILogger> loggers = new ConcurrentDictionary<string, ILogger>();

        /// <summary>
        /// Creates a new <see cref="T:Microsoft.Extensions.Logging.ILogger" /> instance.
        /// </summary>
        /// <param name="categoryName">The category name for messages produced by the logger.</param>
        /// <returns>An <see cref="T:Microsoft.Extensions.Logging.ILogger" /> instance.</returns>
        public ILogger CreateLogger(string categoryName)
        {
            if (categoryName == null)
            {
                throw new ArgumentNullException(nameof(categoryName));
            }

            return this.loggers.GetOrAdd(categoryName, Create);
        }

        /// <summary>
        /// Gets a collection of loggers created by this provider
        /// </summary>
        /// <returns>Collection of loggers created by this provider.</returns>
        public KeyValuePair<string, ILogger>[] GetLoggers()
        {
            return this.loggers.ToArray();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources
        /// </summary>
        public void Dispose()
        {
            if (this.loggers != null)
            {
                this.loggers.Clear();
                this.loggers = null;
            }
        }

        private static ILogger Create(string name)
        {
            return new TestLogger(
                name,
                (a, b) => true);
        }
    }
}
