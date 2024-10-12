using HSX.WebClient.Classes;

namespace HSX.WebClient.Providers;

public class ConsoleLoggerProvider : ILoggerProvider
{
    public ILogger CreateLogger(string categoryName)
    {
        return new ConsoleLogger(categoryName);
    }

    public void Dispose() { }
}