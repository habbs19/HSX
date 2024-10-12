using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace HSX.API.Middlewares;

public class ExceptionHandler : IExceptionHandler
{
    public ILogger<ExceptionHandler> _logger { get; }
    public ExceptionHandler(ILogger<ExceptionHandler> logger)
    {
        _logger = logger;
    }


    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var details = new ProblemDetails
        {
            Status = (int)HttpStatusCode.InternalServerError,
            Type = exception.GetType().Name,
            Title = "An unexpected error occurred",
            Detail = exception.Message,
            Instance = $"{httpContext.Request.Method} {httpContext.Request.Path}"
        };

        httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        httpContext.Response.ContentType = "application/json";

        (int statusCode, string errorMsg) = exception switch
        {
            //=> (403, null),
            BadHttpRequestException badrequestException => (400, badrequestException.Message),
            _ => (500, "Internal server error.")
        };

        _logger.LogError(exception, exception.Message);
        await httpContext.Response.WriteAsJsonAsync(errorMsg,cancellationToken);
        return true;

    }
}