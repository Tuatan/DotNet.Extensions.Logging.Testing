namespace Microsoft.Extensions.Logging.Test
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;

    internal sealed class LogScope
    {
        private static readonly AsyncLocal<LogScope> value = new AsyncLocal<LogScope>();
        private readonly string name;
        private readonly object state;

        public LogScope Parent { get; private set; }

        public static LogScope Current
        {
            get
            {
                return value.Value;
            }
            set
            {
                LogScope.value.Value = value;
            }
        }

        public static IReadOnlyCollection<object> GetState()
        {
            var states = new List<object>();

            if (Current.state == null)
            {
                return new object[0];
            }
            else
            {
                var next = Current;
                do
                {
                    states.Add(next.state);
                    next = next.Parent;
                }
                while(next != null);
            }

            return states;
        }

        internal LogScope(string name, object state)
        {
            this.name = name;
            this.state = state;
        }

        public static IDisposable Push(string name, object state)
        {
            var current = Current;
            Current = new LogScope(name, state) { Parent = current };
            return new DisposableScope();
        }

        public override string ToString()
        {
            object obj = this.state;
            return obj?.ToString();
        }

        private sealed class DisposableScope : IDisposable
        {
            private bool disposed;

            public void Dispose()
            {
                if (this.disposed)
                    return;
    
                Current = Current.Parent;

                this.disposed = true;
            }
        }
    }
}
