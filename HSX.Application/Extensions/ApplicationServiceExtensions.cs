using HSX.Application.Services;
using HSX.Application.Specifications;
using HSX.Core.Interfaces;
using HSX.Infrastructure.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HSX.Application.Extensions;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config){


        // Repository Layer DI

        // Application Service Layer DI
        // Add services to the container.
        services.AddMemoryCache(); // Register in-memory caching

        /// <summary>
        /// The MemoryCacheService is registered as a singleton for the following reasons:
        /// 
        /// 1. **Efficiency**: A single instance of the cache is shared across all requests, reducing memory overhead and improving performance by avoiding the creation of multiple cache instances.
        /// 
        /// 2. **Thread Safety**: `IMemoryCache` is thread-safe, ensuring that concurrent access from multiple threads does not lead to conflicts.
        /// 
        /// 3. **Performance**: Accessing a singleton instance is faster than creating new instances, particularly beneficial in high-traffic scenarios.
        /// 
        /// 4. **Simplicity**: Centralizes cache configuration and management, making it easier to maintain cache settings across the application.
        /// 
        /// 5. **Lifecycle Management**: Managed by the dependency injection container, ensuring proper initialization and disposal during the application lifecycle.
        /// </summary>
        services.AddSingleton<ICacheService, MemoryCacheService>();
        services.AddScoped<IIdentityService, IdentityService>();

        services.AddSignalR();

        // Register AutoMapper and scan for profiles
        services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly); // Registers all profiles in the same assembly
                                                                     // services.AddAutoMapper(typeof(AutoMapperProfiles));

        services.AddScoped<ITokenService, TokenService>();

        return services;
    }
    
}