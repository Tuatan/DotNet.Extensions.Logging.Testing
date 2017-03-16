using System;

namespace DotNet.Extensions.Logging.Testing
{
    internal sealed class EmptyDisposable : IDisposable
    {
        public static EmptyDisposable Default = new EmptyDisposable();

        public void Dispose()
        {
        }
    }
}