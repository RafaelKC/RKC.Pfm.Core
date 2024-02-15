using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace RKC.Pfm.Core.Infrastructure.Extensions;

public static class AddInfrastructureExtension
{
        public static IConfiguration AddInfrastructure(this IServiceCollection services)
        {
                var configuration = services.AddConsul();

                services
                        .AddDatabase(configuration)
                        .AddSupabase(configuration)
                        .AddCache(configuration)
                        .AddAuthentication(configuration);

                return configuration;
        }
}