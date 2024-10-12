
using HSX.WebClient.Services;

namespace HSX.WebClient.Handlers;

public class ExceptionHandler : DelegatingHandler
{
    private readonly ExceptionHandlerService _exceptionHandler;
    private readonly ILogger<ExceptionHandler> _logger;

    public ExceptionHandler(ExceptionHandlerService exceptionHandler, ILogger<ExceptionHandler> logger)
    {
        _exceptionHandler = exceptionHandler;
        _logger = logger;
    }


    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        try
        {
            return await base.SendAsync(request, cancellationToken);
        }
        catch (Exception ex)
        {
            await _exceptionHandler.HandleExceptionAsync(ex);

            // Create an error response indicating failure
            var response = new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError)
            {
                Content = new StringContent("An error occurred while processing request.")
            };

            return response;
        }
    }
}
