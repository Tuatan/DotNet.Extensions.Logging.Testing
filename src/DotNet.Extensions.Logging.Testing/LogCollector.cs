using System;

namespace DotNet.Extensions.Logging.Testing
{
    /// <summary>
    /// Collector of log entries.
    /// </summary>
    public sealed class LogCollector
    {
        /// <summary>
        /// Creates an instance of <see cref="LogCollector"/> class.
        /// </summary>
        public LogCollector()
        {
            this.Logger = new TestLogger(string.Empty, (a, b) => true);
        }

        /// <summary>
        /// Gets an current test logger.
        /// </summary>
        public TestLogger Logger { get; }

        /// <summary>
        /// Gets all collected log entries.
        /// </summary>
        public LogEvent[] Logs { get; private set; }

        /// <summary>
        /// Starts collection of log entries.
        /// </summary>
        /// <returns>An <see cref="IDisposable"/> that ends the collection of the logs on dispose.</returns>
        public IDisposable CollectLogs()
        {
            return new Session(() => { this.Logs = this.Logger.GetLogs(); });
        }

        private sealed class Session : IDisposable
        {
            private Action action;

            public Session(Action action)
            {
                this.action = action;
            }

            public void Dispose()
            {
                if (this.action != null)
                {
                    this.action();
                    this.action = null;
                }
            }
        }
    }
}
