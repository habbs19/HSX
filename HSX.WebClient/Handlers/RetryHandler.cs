using Polly;
using Polly.Retry;

namespace HSX.WebClient.Handlers;

public class RetryHandler : DelegatingHandler
{
    private readonly AsyncRetryPolicy<HttpResponseMessage> _retryPolicy =
        Policy<HttpResponseMessage>
            .Handle<HttpRequestException>()
            .WaitAndRetryAsync(1, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

    private readonly ILogger<RetryHandler> _logger;

    public RetryHandler(ILogger<RetryHandler> logger)
    {
        _logger = logger;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        int retryCount = -1; // start from -1 because the first count is the original request. the following counts are the retry attempts

        var policyResult = await _retryPolicy.ExecuteAndCaptureAsync(async () =>
        {
            retryCount++; // Increment retry count on each execution
            return await base.SendAsync(request, cancellationToken);
        });

        if (policyResult.Outcome == OutcomeType.Failure)
        {
            // Capture relevant details for logging
            var httpMethod = request.Method;
            var requestUri = request.RequestUri?.ToString() ?? "Unknown URI";
            var finalExceptionMessage = policyResult.FinalException?.Message ?? "No exception message";

            // Log a detailed error message for admins
            

            throw new HttpRequestException(
              $"HTTP request {httpMethod} {requestUri} failed after {retryCount} retry",
                policyResult.FinalException);
        }
        return policyResult.Result;
    }
}