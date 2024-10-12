using HSX.Contract.Common;
using HSX.Core.Models;
using HSX.Infrastructure.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace HSX.Infrastructure.Extensions;

public static class IdentityServiceExtensions
{
    public static IServiceCollection AddIdentityServices(this IServiceCollection services,IConfiguration config)
    {
        services.AddIdentityCore<AppUser>(opt =>
        {
            opt.Password.RequireNonAlphanumeric = false;
        })
            .AddRoles<AppRole>()
            .AddRoleManager<RoleManager<AppRole>>()
            .AddUserManager<UserManager<AppUser>>()
            .AddRoleManager<RoleManager<AppRole>>()
            .AddUserStore<UserStore>() // registers both IUserStore and IUserLoginStore
            .AddRoleStore<RoleStore>()
            .AddSignInManager<SignInManager<AppUser>>()
            .AddApiEndpoints()
            .AddDefaultTokenProviders();


        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(opt =>
            {
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"])),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
                opt.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query[Constants.AccessToken];
                        var path = context.HttpContext.Request.Path;

                        // Check if the token is present and if the request path starts with "/hubs".
                        // This is useful for handling token authentication in real-time applications like SignalR hubs,
                        // where the access token is sometimes passed in the query string (WebSocket connections can't send headers like normal HTTP requests).
                        if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hubs"))
                        {
                            // If the access token exists and the request is for a SignalR hub, assign the token from the query string to the context.
                            // This allows the authentication system to use this token for validating the request.
                            context.Token = accessToken;
                        }
                        return Task.CompletedTask;
                    }
                };
            });
        services.AddAuthorization(opt =>
        {
            opt.AddPolicy("RequireAdminRole", policy => policy.RequireRole(Constants.Roles.Admin));
            opt.AddPolicy("ModeratePhotoRole", policy => policy.RequireRole(Constants.Roles.Admin, "Moderator"));
        });


        return services;
    }

    public static IServiceCollection AddApplicationIdentityCookie(this IServiceCollection services, IConfiguration config)
    {
        // Configure app cookie
        //
        // The default values, which are appropriate for hosting the Backend and
        // BlazorWasmAuth apps on the same domain, are Lax and SameAsRequest. 
        // For more information on these settings, see:
        // https://learn.microsoft.com/aspnet/core/blazor/security/webassembly/standalone-with-identity#cross-domain-hosting-same-site-configuration

        services.ConfigureApplicationCookie(options =>
        {
            options.Cookie.SameSite = SameSiteMode.Lax;
            options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
        });
        ;
        return services;
    }
}