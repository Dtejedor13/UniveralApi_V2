namespace UniversalAPI.Logger
{
    public class MyLogger : ILogger
    {
        public IDisposable BeginScope<TState>(TState state) => default!;

        public bool IsEnabled(LogLevel logLevel) => false;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            // Do stuff here
        }
    }
}
