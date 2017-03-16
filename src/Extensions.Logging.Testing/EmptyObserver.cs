using System;

namespace Extensions.Logging.Testing
{
    internal sealed class EmptyObserver<T> : IObserver<T>
    {
        public static EmptyObserver<T> Default = new EmptyObserver<T>();

        public void OnCompleted()
        {
        }

        public void OnError(Exception error)
        {
        }

        public void OnNext(T value)
        {
        }
    }
}