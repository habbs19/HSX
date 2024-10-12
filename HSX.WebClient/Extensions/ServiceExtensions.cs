using HSX.WebClient.Providers;
using Microsoft.AspNetCore.Components.Authorization;

namespace HSX.WebClient.Extensions;
public static class ServiceExtensions
{
    public static void AddClientServices(this IServiceCollection services)
    {
        // Register the token provider
        //services.AddScoped<ITokenProvider, TokenProvider>();

        // Register the TokenAuthenticationStateProvider only once
        services.AddScoped<TokenAuthenticationStateProvider>();

        // Reuse the TokenAuthenticationStateProvider for both AuthenticationStateProvider and IAccountManagement
        services.AddScoped<IAccountManager>(sp => sp.GetRequiredService<TokenAuthenticationStateProvider>());
        services.AddScoped<AuthenticationStateProvider, TokenAuthenticationStateProvider>();

        services.AddScoped<HttpServiceProvider>(sp =>
        {
            var httpClient = sp.GetRequiredService<HttpClient>();
            var loggerFactory = sp.GetRequiredService<ILoggerFactory>();

            return new HttpServiceProvider(httpClient, loggerFactory);
        });
    }
}
