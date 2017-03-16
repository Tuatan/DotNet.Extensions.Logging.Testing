using System;

namespace DotNet.Extensions.Logging.Testing
{
    internal sealed class LoggerObserver : IObserver<LogEvent>
    {
        private readonly string _category;
        private readonly IObserver<Tuple<string, LogEvent>> _parent;

        public LoggerObserver(string category, IObserver<Tuple<string, LogEvent>> parent)
        {
            if (parent == null) throw new ArgumentNullException(nameof(parent));
            _category = category;
            _parent = parent;
        }

        public void OnCompleted()
        {
        }

        public void OnError(Exception error)
        {
        }

        public void OnNext(LogEvent value)
        {
            _parent.OnNext(Tuple.Create(this._category, value));
        }
    }
}