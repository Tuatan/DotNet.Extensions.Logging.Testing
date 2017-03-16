using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace DotNet.Extensions.Logging.Testing
{
    /// <summary>
    /// Logger provider that creates test loggers.
    /// </summary>
    public sealed class ObservableLoggerProvider : ILoggerProvider
    {
        private readonly IObserver<Tuple<string, LogEvent>> _observer;
        private ConcurrentDictionary<string, ILogger> loggers = new ConcurrentDictionary<string, ILogger>();

        public ObservableLoggerProvider(IObserver<Tuple<string, LogEvent>> observer)
        {
            if (observer == null) throw new ArgumentNullException(nameof(observer));
            _observer = observer;
        }

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
                var ls = this.loggers.ToArray();

                this.loggers.Clear();
                this.loggers = null;

                foreach (var disposable in ls.Select(i => i.Value).OfType<IDisposable>())
                {
                    disposable.Dispose();
                }
            }
        }

        private ILogger Create(string name)
        {
            return new ObservableLogger(
                name,
                (a, b) => true,
                new LoggerObserver(name, this._observer));
        }
    }
}
