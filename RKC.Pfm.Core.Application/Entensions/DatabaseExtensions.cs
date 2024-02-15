using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using RKC.Pfm.Core.Application.Communs;

namespace RKC.Pfm.Core.Application.Entensions;

public static class DatabaseExtensions
{
    public static async Task<PagedResult<T>> ToPagedResult<T>(this IQueryable<T> query, PagedFilteredInput input)
    {
        var totalCount = await query.CountAsync();
        var items = await query.PageBy(input.SkipCount, input.MaxResultCount).ToListAsync();

        return new PagedResult<T>
        {
            Items = items,
            TotalCount = totalCount
        };
    }

    public static IQueryable<T> PageBy<T>(this IQueryable<T> query, int skipCount, int maxResultCount)
    {
        return query.Skip(skipCount).Take(maxResultCount);
    }

    public static IQueryable<T> WhereIf<T>(this IQueryable<T> query, bool condition, Expression<Func<T, bool>> predicate)
    {
        if (condition)
        {
            return query.Where(predicate);
        }

        return query;
    }
}