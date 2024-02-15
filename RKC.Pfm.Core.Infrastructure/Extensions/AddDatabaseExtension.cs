using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RKC.Pfm.Core.Infrastructure.Database;
using RKC.Pfm.Core.Infrastructure.Database.Services;

namespace RKC.Pfm.Core.Infrastructure.Extensions;

public static class AddDatabaseExtension
{
    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddDbContext<RkcPfmCoreDbContext>(op =>
            {
                op.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
            })
            .AddScoped<ISchemaNameProvider, SchemaNameProvider>()
            .AddScoped<IResolveFilterEntities, ResolveFilterEntities>()
            .AddScoped<IResolveSavingEntities, ResolveSavingEntities>();
        
        return services;
    }
}