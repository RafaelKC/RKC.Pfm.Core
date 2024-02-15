using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using RKC.Pfm.Core.Domain.Communs;
using RKC.Pfm.Core.Infrastructure.Supabse;

namespace RKC.Pfm.Core.Infrastructure.Database.Services;

public interface IResolveSavingEntities
{
    public void Resolve(IEnumerable<EntityEntry> entriesList);
}

public class ResolveSavingEntities : IResolveSavingEntities
{
    private readonly ISupabseClient _supabseClient;

    public ResolveSavingEntities(ISupabseClient supabseClient)
    {
        _supabseClient = supabseClient;
    }

    public void Resolve(IEnumerable<EntityEntry> entriesList)
    {
        var currentUser = _supabseClient.Auth.CurrentUser;
        if (currentUser is null || string.IsNullOrWhiteSpace(currentUser.Id)) return;
        
        var entries = entriesList
            .Where(e => e.Entity is IHasUserId && e.State is EntityState.Added or EntityState.Modified);

        foreach (var entityEntry in entries)
        {
            ((IHasUserId)entityEntry.Entity).UserId = Guid.Parse(currentUser.Id);
        }
    }
}