using Microsoft.EntityFrameworkCore;

namespace RKC.Pfm.Core.Infrastructure.Services;

public interface IMigrationsService
{
    public void ApplyMigrations();
}

public class MigrationService: IMigrationsService
{
    private readonly RkcPfmCoreDbContext _dbContext;

    public MigrationService(RkcPfmCoreDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void ApplyMigrations()
    {
        _dbContext.Database.Migrate();
    }
}