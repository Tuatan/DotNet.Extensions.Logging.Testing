using System;

namespace DotNet.Extensions.Logging.Testing
{
    internal sealed class Session : IDisposable
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