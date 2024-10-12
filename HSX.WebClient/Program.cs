using HSX.WebClient.Providers;
using Blazored.LocalStorage;
using HSX.WebClient;
using HSX.WebClient.Extensions;
using HSX.WebClient.Handlers;
using HSX.WebClient.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddBlazoredLocalStorage(); // used for storing tokens 
builder.Services.AddClientServices();

// set up authorization
builder.Services.AddAuthorizationCore();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

//builder.Services.AddOidcAuthentication(options =>
//{
//    // Configure your authentication provider options here.
//    // For more information, see https://aka.ms/blazor-standalone-auth
//    builder.Configuration.Bind("Local", options.ProviderOptions);
//});

// Add Logging
builder.Logging.ClearProviders(); // Clear existing loggers
builder.Logging.AddProvider(new ConsoleLoggerProvider());
builder.Logging.AddFilter("System.Net.Http.HttpClient", LogLevel.Warning);


// Register the token handler 
builder.Services.AddTransient<TokenHandler>();
builder.Services.AddTransient<ExceptionHandler>();
builder.Services.AddTransient<RetryHandler>();

// configure client for auth interactions
builder.Services.AddHttpClient<HttpServiceProvider>(options =>
{
    options.BaseAddress = new Uri(builder.Configuration["BackendUrl"] ?? "https://localhost:5001");

})
.AddHttpMessageHandler<TokenHandler>() // Adds token authentication first
.AddHttpMessageHandler<ExceptionHandler>() // Retries failed requests
.AddHttpMessageHandler<RetryHandler>(); // Catches and handles any final exceptions

var host = builder.Build();

AppDomain.CurrentDomain.UnhandledException += async (sender, e) =>
{
    var logger = host.Services.GetRequiredService<ILogger>();
    logger.LogInformation("UnhandledException");

    var exceptionHandler = host.Services.GetRequiredService<ExceptionHandlerService>();
    await exceptionHandler.HandleExceptionAsync(e.ExceptionObject as Exception);
};

await host.RunAsync();