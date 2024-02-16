using Microsoft.EntityFrameworkCore;
using RKC.Pfm.Core.Application.Communs;
using RKC.Pfm.Core.Application.Entensions;
using RKC.Pfm.Core.Application.Periods.Dtos;
using RKC.Pfm.Core.Application.Transients;
using RKC.Pfm.Core.Domain.Periods;
using RKC.Pfm.Core.Infrastructure.Database;

namespace RKC.Pfm.Core.Application.Periods;

public interface IPeriodService
{
    public Task<PagedResult<PeriodDto>> GetList(PeriodGetListInput input);
    public Task<PeriodDto?> Get(Guid id);
    public Task<bool> Create(PeriodDto input);
    public Task<bool> Update(Guid id, PeriodUpdateDto input);
    public Task<bool> Delete(Guid id);
}

public class PeriodService: IPeriodService, IAutoTransient
{
    private readonly RkcPfmCoreDbContext _context;

    public PeriodService(RkcPfmCoreDbContext context)
    {
        _context = context;
    }

    public async Task<PagedResult<PeriodDto>> GetList(PeriodGetListInput input)
    {
        var query = _context.Periods
            .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), p => p.Name.ToLower().Contains(input.Filter.ToLower()))
            .WhereIf(input.EndOnOrBeforeFilter.HasValue, p => p.End >= input.EndOnOrBeforeFilter)
            .Where(p => p.IsSchema == input.SchemasFilter)
            .OrderBy(p => p.Start);

        if (!input.OrderAscending)
        {
            query = query.OrderByDescending(p => p.Start);
        }

        var result = await query.Select(p => new PeriodDto(p)).ToPagedResult(input);
        return result;
    }

    public async Task<PeriodDto?> Get(Guid id)
    {
        return await _context.Periods.Select(p => new PeriodDto(p)).FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<bool> Create(PeriodDto input)
    {
        var alreadyExist = await _context.Periods.AnyAsync(p => p.Id == input.Id);
        var endBeforeStart = input.Start >= input.End;
        var hasConflictPeriod = await _context.Periods.AnyAsync(p =>
            (p.Start >= input.Start && p.Start <= input.End)
            || (p.End >= input.Start && p.End <= input.End));

        if (alreadyExist || endBeforeStart || hasConflictPeriod)
        {
            return false;
        }

        var period = new Period(input);
        await _context.Periods.AddAsync(period);
        await _context.SaveChangesAsync();
        
        return true;
    }

    public async Task<bool> Update(Guid id, PeriodUpdateDto input)
    {
        var period = await _context.Periods.FirstOrDefaultAsync(p => p.Id == id);
        if (period is null)
        {
            return false;
        }
        period.Name = input.Name;
        _context.Periods.Update(period);
        await _context.SaveChangesAsync();
        
        return true;
    }

    public async Task<bool> Delete(Guid id)
    {
        var period = await _context.Periods.FirstOrDefaultAsync(p => p.Id == id);
        if (period is null)
        {
            return false;
        }

        _context.Periods.Remove(period);
        await _context.SaveChangesAsync();
        
        return true;
    }
}