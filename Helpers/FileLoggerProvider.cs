using Microsoft.Extensions.Logging;

namespace PersonalFinanceApp.Helpers;

public sealed class FileLoggerProvider : ILoggerProvider
{
    private readonly string _path;
    private readonly LogLevel _minLevel;
    private readonly object _lock = new();

    public FileLoggerProvider(string path, LogLevel minLevel = LogLevel.Information)
    {
        _path = path;
        _minLevel = minLevel;
        Directory.CreateDirectory(Path.GetDirectoryName(path)!);
    }

    public ILogger CreateLogger(string categoryName) =>
        new FileLogger(categoryName, _path, _minLevel, _lock);

    public void Dispose() { }

    private sealed class FileLogger : ILogger
    {
        private readonly string _category;
        private readonly string _path;
        private readonly LogLevel _minLevel;
        private readonly object _lock;

        public FileLogger(string category, string path, LogLevel minLevel, object @lock)
        {
            _category = category;
            _path = path;
            _minLevel = minLevel;
            _lock = @lock;
        }

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;

        public bool IsEnabled(LogLevel logLevel) =>
            logLevel != LogLevel.None && logLevel >= _minLevel;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state,
                                Exception? exception, Func<TState, Exception?, string> formatter)
        {
            if (!IsEnabled(logLevel))
                return;

            var line = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} [{logLevel}] {_category}: {formatter(state, exception)}";
            if (exception is not null)
                line += Environment.NewLine + exception;

            try
            {
                lock (_lock)
                {
                    File.AppendAllText(_path, line + Environment.NewLine);
                }
            }
            catch
            {
                // Єдиний виправданий порожній catch: логер не може залогувати власний збій
                // і не повинен через це валити застосунок.
            }
        }
    }
}