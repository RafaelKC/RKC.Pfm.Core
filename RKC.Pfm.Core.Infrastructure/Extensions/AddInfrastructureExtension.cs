using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RKC.Pfm.Core.Infrastructure.Consts;
using RKC.Pfm.Core.Infrastructure.Database;
using Supabase;

namespace RKC.Pfm.Core.Infrastructure.Extensions;

public static class AddInfrastructureExtension
{
        public static IConfiguration AddInfrastructure(this IServiceCollection services)
        {
                var configuration = services.AddConsul();

                services
                        .AddDbContext<RkcPfmCoreDbContext>(op =>
                        {
                                op.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
                        })
                        .AddSupabase(configuration)
                        .AddAuthentication(configuration);

                return configuration;
        }
}