using HSX.Contract.Common;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Net.Http.Headers;

namespace HSX.WebClient.Handlers;

/// <summary>
/// Handler to ensure token is automatically sent over with each request.
/// </summary>
public class TokenHandler : DelegatingHandler
{
    private readonly ILocalStorageService _localStorageService;
    private readonly ILogger<TokenHandler> _logger;

    public TokenHandler(ILocalStorageService localStorageService,ILogger<TokenHandler> logger)
    {
        _localStorageService = localStorageService;
        _logger = logger;
    }


    /// <summary>
    /// Main method to override for the handler.
    /// </summary>
    /// <param name="request">The original request.</param>
    /// <param name="cancellationToken">The token to handle cancellations.</param>
    /// <returns>The <see cref="HttpResponseMessage"/>.</returns>
    protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var token = await _localStorageService.GetItemAsync<string>(Constants.AccessToken);
        if (token != null)
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
        return await base.SendAsync(request, cancellationToken);
    }
}
