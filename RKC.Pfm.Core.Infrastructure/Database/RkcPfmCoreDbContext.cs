using System.Reflection;
using Microsoft.EntityFrameworkCore;
using RKC.Pfm.Core.Domain.Communs;
using RKC.Pfm.Core.Domain.Periods;
using RKC.Pfm.Core.Infrastructure.Database.Services;
using RKC.Pfm.Core.Infrastructure.Supabse;

namespace RKC.Pfm.Core.Infrastructure.Database;

public class RkcPfmCoreDbContext: DbContext
{
    private ISchemaNameProvider? _schemaNameProvider;
    private IResolveSavingEntities? _resolveSavingEntities;
    private IResolveFilterEntities? _resolveFilterEntities;
    
    
    public RkcPfmCoreDbContext()
    {
    }
    
    public RkcPfmCoreDbContext(
        DbContextOptions<RkcPfmCoreDbContext> options,
        ISchemaNameProvider schemaNameProvider,
        IResolveSavingEntities? resolveSavingEntities,
        IResolveFilterEntities? resolveFilterEntities): base(options)
    {
        _schemaNameProvider = schemaNameProvider;
        _resolveSavingEntities = resolveSavingEntities;
        _resolveFilterEntities = resolveFilterEntities;
        Database.Migrate();
    }
    
    public DbSet<Period> Periods { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema(GetSchemaName());

        modelBuilder.Entity<Period>(period =>
        {
            period.HasIndex(e => e.Start).IsUnique();
            period.HasIndex(e => e.End).IsUnique();

            period
                .HasMany(e => e.BasedPeriods)
                .WithOne(e => e.SchemaPeriod)
                .HasForeignKey(e => e.IdSchemaPeriod)
                .HasPrincipalKey(e => e.Id)
                .IsRequired(false);
        });

        _resolveFilterEntities?.Resolve(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        if (!options.IsConfigured)
        {
            options.UseNpgsql();
        }
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        _resolveSavingEntities?.Resolve(ChangeTracker.Entries());
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }
    
    public override Task<int> SaveChangesAsync(
        bool acceptAllChangesOnSuccess,
        CancellationToken cancellationToken = default)
    {
        _resolveSavingEntities?.Resolve(ChangeTracker.Entries());
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
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