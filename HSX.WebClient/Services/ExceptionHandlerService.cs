namespace HSX.WebClient.Services;

public class ExceptionHandlerService
{
    private readonly ILogger<ExceptionHandlerService> _logger;

    public ExceptionHandlerService(ILogger<ExceptionHandlerService> logger)
    {
        _logger = logger;
    }

    public async Task HandleExceptionAsync<T>(Exception exception) where T : class
    {
        exception.Source = nameof(T);
        await HandleExceptionAsync(exception);
    }

    public async Task HandleExceptionAsync(Exception exception)
    {
        // Gather exception details
        var exceptionMessage = exception.Message ?? "No exception message";
        var source = exception.Source ?? "Unknown source";
        var stackTrace = exception.StackTrace ?? "No stack trace available";
        var innerExceptionMessage = exception.InnerException?.Message ?? "No inner exception";
        var innerExceptionStackTrace = exception.InnerException?.StackTrace ?? "No inner exception stack trace";

        // Log the full exception details
        _logger.LogError(exception,
            $"An unhandled exception occurred; " +
            $"Message: {exceptionMessage}; " +
            $"Exception: {innerExceptionMessage}; "
        );

        // add custom handling logic, such as displaying a notification to the user
        // or redirecting to an error page.

        await Task.Delay(0); // Simulating async work
    }
}
