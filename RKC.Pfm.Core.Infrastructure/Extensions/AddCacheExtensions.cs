using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RKC.Pfm.Core.Infrastructure.Consts;
using StackExchange.Redis;

namespace RKC.Pfm.Core.Infrastructure.Extensions;

public static class AddCacheExtensions
{
    public static IServiceCollection AddCache(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddStackExchangeRedisCache(options =>
        {
            options.InstanceName = AppConfig.AppName;
            options.Configuration = configuration[AppConfig.CacheRedisUrl];

            var redisPassword = configuration[AppConfig.CacheRedisPassword];
            if (!string.IsNullOrWhiteSpace(redisPassword))
            {
                options.ConfigurationOptions = new ConfigurationOptions
                {
                    Password = redisPassword,
                };
            }
        });
        
        return services;
    }
}