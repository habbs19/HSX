using HSX.Infrastructure.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HSX.Infrastructure.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection service,IConfiguration configuration)
    {

        service.AddScoped<UpdateUserLastActiveFilter>();


        return service;
    }
}
