using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RKC.Pfm.Core.Infrastructure.Consts;
using Winton.Extensions.Configuration.Consul;

namespace RKC.Pfm.Core.Infrastructure.Extensions;

public static class AddConsulExtension
{
    public static IConfiguration AddConsul(this IServiceCollection services)
    {
        var host = Environment.GetEnvironmentVariable(AppConfig.ConsulHostKey);
        if (string.IsNullOrWhiteSpace(host))
        {
            throw new Exception("Consul host not set");
        }
        
        var builder = new ConfigurationBuilder()
            .AddConsul(
                AppConfig.AppName,
                options =>
                {
                    options.ConsulConfigurationOptions = config =>
                    {
                        config.Address = new Uri(host);
                    };
                    options.Optional = false;
                    options.ReloadOnChange = true;
                }
            );
        var configuration = builder.Build();
        services.AddSingleton<IConfiguration>(configuration);
        return configuration;
    }
}