using Microsoft.EntityFrameworkCore;
using RKC.Pfm.Core.Infrastructure.Database.Services;

namespace RKC.Pfm.Core.Infrastructure.Database;

public class RkcPfmCoreDbContext: DbContext
{
    private ISchemaNameProvider? _schemaNameProvider;
    
    public RkcPfmCoreDbContext()
    {
    }
    
    public RkcPfmCoreDbContext(DbContextOptions<RkcPfmCoreDbContext> options, ISchemaNameProvider schemaNameProvider): base(options)
    {
        _schemaNameProvider = schemaNameProvider;
        Database.Migrate();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema(GetSchemaName());
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        if (!options.IsConfigured)
        {
            options.UseNpgsql();
        }
    }

    private string GetSchemaName()
    {
        if (_schemaNameProvider is not null)
        {
            _schemaNameProvider.GetSchemaName();
        }
        return "public";
    }
}