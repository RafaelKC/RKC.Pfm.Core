using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RKC.Pfm.Core.Infrastructure.Database;
using RKC.Pfm.Core.Infrastructure.Transients;

namespace RKC.Pfm.Core.Infrastructure.Extensions;

public static class AddInfrastructureExtension
{
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
                services
                        .AddDbContext<RkcPfmCoreDbContext>(op =>
                        {
                                op.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
                        })
                        .AddAutoTransients();

                return services;
        }
}