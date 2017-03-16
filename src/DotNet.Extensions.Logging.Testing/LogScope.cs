using System;
using System.Collections.Generic;

namespace DotNet.Extensions.Logging.Testing
{
#if (NET45)

    using System.Collections.Immutable;
    using System.Runtime.Remoting.Messaging;
    using System.Linq;

    internal static class LogScope
    {
        private static readonly string name = Guid.NewGuid().ToString("N");

        private sealed class Wrapper : MarshalByRefObject
        {
            public ImmutableStack<object> Value { get; set; }
        }

        private static ImmutableStack<object> CurrentContext
        {
            get
            {
                var ret = CallContext.LogicalGetData(name) as Wrapper;
                return ret == null ? ImmutableStack.Create<object>() : ret.Value;
            }

            set
            {
                CallContext.LogicalSetData(name, new Wrapper { Value = value });
            }
        }

        public static IDisposable Push(string name, object state)
        {
            CurrentContext = CurrentContext.Push(state);
            return new DisposableScope();
        }

        public static IReadOnlyCollection<object> GetState()
        {
            return CurrentContext.Reverse().ToArray();
        }

        private sealed class DisposableScope : IDisposable
        {
            private bool disposed;

            public void Dispose()
            {
                if (this.disposed)
                    return;

                CurrentContext = CurrentContext.Pop();

                this.disposed = true;
            }
        }
    }

#else

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
#endif
}
