using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RKC.Pfm.Core.Infrastructure.Authentication;
using RKC.Pfm.Core.Infrastructure.Consts;

namespace RKC.Pfm.Core.Infrastructure.Extensions;

public static class AddAuthenticationExtension
{
    public static IServiceCollection AddAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddScoped<IAuthenticationService, AuthenticationService>()
            .AddHttpClient<IJwtProvider, JwtProvider>();

        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.Authority = configuration[AppConfig.AuthenticationIssuer];
                options.Audience = configuration[AppConfig.AuthenticationAudience];
                options.TokenValidationParameters.ValidIssuer = configuration[AppConfig.AuthenticationIssuer];
                options.TokenValidationParameters.ValidateIssuer = true;
                options.TokenValidationParameters.ValidateAudience = true;
                options.TokenValidationParameters.ValidateLifetime = true;
            });

        return services;
    }
}