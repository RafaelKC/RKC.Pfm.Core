using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace RKC.Pfm.Core.Infrastructure;

public class RkcPfmCoreDbContext: DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseNpgsql("Host=xxx.xxx.xxx.xx; Database=RKC.Pfm.Core; Username=postgres; Password=1234");
    }
}