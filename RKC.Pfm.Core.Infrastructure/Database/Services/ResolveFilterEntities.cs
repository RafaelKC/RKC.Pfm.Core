﻿using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Query;
using RKC.Pfm.Core.Domain.Communs;
using RKC.Pfm.Core.Infrastructure.Supabse;

namespace RKC.Pfm.Core.Infrastructure.Database.Services;

public interface IResolveFilterEntities
{
    public void Resolve(ModelBuilder builder);
}

public class ResolveFilterEntities : IResolveFilterEntities
{
    private readonly ISupabseClient _supabseClient;

    public ResolveFilterEntities(ISupabseClient supabseClient)
    {
        _supabseClient = supabseClient;
    }

    public void Resolve(ModelBuilder builder)
    {
        var currentUser = _supabseClient.Auth.CurrentUser?.Id;
        
        builder.ApplyGlobalFilters<IHasUserId>(e => currentUser != null && e.UserId == Guid.Parse(currentUser));
    }
}

public static class ModelBuilderExtension
{
    public static void ApplyGlobalFilters<TInterface>(this ModelBuilder modelBuilder, Expression<Func<TInterface, bool>> expression)
    {
        var entities = modelBuilder.Model
            .GetEntityTypes()
            .Where(e => e.ClrType.GetInterface(typeof(TInterface).Name) != null)
            .Select(e => e.ClrType);
        foreach (var entity in entities)
        {
            var newParam = Expression.Parameter(entity);
            var newbody = ReplacingExpressionVisitor.Replace(expression.Parameters.Single(), newParam, expression.Body);    
            modelBuilder.Entity(entity).HasQueryFilter(Expression.Lambda(newbody, newParam));
        }
    }
}