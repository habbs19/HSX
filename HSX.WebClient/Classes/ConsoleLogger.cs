
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace HSX.WebClient.Classes
{
    public class ConsoleLogger(string categoryName) : ILogger
    {
        private readonly string categoryName = categoryName;
        private static string LogLevelShort(LogLevel logLevel)
        {
            return logLevel switch
            {
                LogLevel.Debug => "DEBUG",
                LogLevel.Information => "INFO",
                LogLevel.Warning => "WARN",
                LogLevel.Error => "ERROR",
                LogLevel.Critical => "CRIT",
                _ => "UNKNOWN" // Default case for other LogLevels
            };
        }

#pragma warning disable CS8633 // Nullability in constraints for type parameter doesn't match the constraints for type parameter in implicitly implemented interface method'.
        public IDisposable BeginScope<TState>(TState state) => null!;
#pragma warning restore CS8633 // Nullability in constraints for type parameter doesn't match the constraints for type parameter in implicitly implemented interface method'.

        public bool IsEnabled(LogLevel logLevel) => logLevel >= LogLevel.Information;

#pragma warning disable CS8767 // Nullability of reference types in type of parameter doesn't match implicitly implemented member (possibly because of nullability attributes).
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
#pragma warning restore CS8767 // Nullability of reference types in type of parameter doesn't match implicitly implemented member (possibly because of nullability attributes).
        {
            if (!IsEnabled(logLevel)) return;

            var logMessage = $"{LogLevelShort(logLevel)}: {categoryName} - {formatter(state, exception)}";

            switch(logLevel)
            {
                case LogLevel.Information:
                case LogLevel.Warning:
                    Console.WriteLine(logMessage);
                    break;
                case LogLevel.Error:
                case LogLevel.Critical:
                    Console.Error.WriteLineAsync(logMessage);
                    break;
                case LogLevel.Debug:
                    Debug.WriteLine(logMessage);
                    break;
            }
                       
        }
    }
}
