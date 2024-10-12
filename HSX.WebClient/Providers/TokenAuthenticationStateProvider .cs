using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using HSX.Contract.Common;
using HSX.Contract.DTOs;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using HSX.WebClient.Providers;
namespace HSX.WebClient.Providers;

public class TokenAuthenticationStateProvider(IHttpClientFactory httpClientFactory, ILocalStorageService sessionStorageService) : AuthenticationStateProvider, IAccountManager
{
    private readonly ILocalStorageService _localStorageService = sessionStorageService;
    private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

    /// <summary>
    /// Special auth client.
    /// </summary>
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient(Constants.Auth);

    /// <summary>
    /// Authentication state.
    /// </summary>
    private bool _authenticated = false;

    /// <summary>
    /// Default principal for anonymous (not authenticated) users.
    /// </summary>
    private readonly ClaimsPrincipal Unauthenticated = new(new ClaimsIdentity());

    public async Task<bool> CheckAuthenticatedAsync()
    {
        await GetAuthenticationStateAsync();
        return _authenticated;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        // default to unauthenticated
        var user = Unauthenticated;

        var saveToken = await _localStorageService.GetItemAsync<string>(Constants.AccessToken);
        if (saveToken == null)
        {
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }

        var tokenContent = _jwtSecurityTokenHandler.ReadJwtToken(saveToken);
        if (tokenContent.ValidTo < DateTime.Now)
        {
            return new AuthenticationState(user);
        }

        var claims = tokenContent.Claims;
        user = new ClaimsPrincipal(new ClaimsIdentity(claims, "jwt"));
        return new AuthenticationState(user);
    }

    public async Task<ServiceResult> LoginAsync(LoginDTO model)
    {
        //var savedToken = await _localStorageService.GetItemAsync<string>(Constants.AccessToken);
        //var tokenContent = _jwtSecurityTokenHandler.ReadJwtToken(savedToken);
        //var claims = tokenContent.Claims.ToList();
        ////claims.Add(new Claim(ClaimTypes.Name, tokenContent.Subject));
        //var user = new ClaimsPrincipal(new ClaimsIdentity(claims, "jwt"));
        //var authState = Task.FromResult(new AuthenticationState(user));
        //NotifyAuthenticationStateChanged(authState);

        var result = await _httpClient.PostAsJsonAsync($"account/login", model);

        if (result.IsSuccessStatusCode)
        {
            _authenticated = true;
            var obj = await result.Content.ReadFromJsonAsync<UserDTO>();

            await _localStorageService.SetItemAsync(Constants.AccessToken, obj!.Token);

            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());

            return ServiceResult.Ok();
        }
        return ServiceResult.Fail(errors: "Login failure");
    }

    public async Task LogoutAsync()
    {
        //await _localStorageService.RemoveItemAsync(Constants.AccessToken);
        //var nobody = new ClaimsPrincipal(new ClaimsIdentity());
        //var authState = Task.FromResult(new AuthenticationState(nobody));
        //NotifyAuthenticationStateChanged(authState);

        const string Empty = "{}";
        var emptyContent = new StringContent(Empty, Encoding.UTF8, "application/json");
        await _httpClient.PostAsync("account/logout", emptyContent);
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    public Task<ServiceResult> RegisterAsync(RegisterDTO model)
    {
        throw new NotImplementedException();
    }
}