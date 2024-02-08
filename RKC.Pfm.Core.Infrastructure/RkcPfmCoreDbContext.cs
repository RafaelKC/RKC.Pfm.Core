using Microsoft.EntityFrameworkCore;

namespace RKC.Pfm.Core.Infrastructure;

public class RkcPfmCoreDbContext: DbContext
{
    public RkcPfmCoreDbContext()
    {
    }
    
    public RkcPfmCoreDbContext(DbContextOptions<RkcPfmCoreDbContext> options): base(options)
    {
        Database.Migrate();
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        if (!options.IsConfigured)
        {
            options.UseNpgsql();
        }
    }
}