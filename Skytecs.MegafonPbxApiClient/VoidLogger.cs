using Microsoft.Extensions.Logging;
using System;

namespace Skytecs.MegafonPbxApiClient
{
    class VoidLogger<T> : ILogger<T>
    {
        public IDisposable BeginScope<TState>(TState state)
        {
            throw new NotSupportedException();
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return false;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {

        }
    }
}

