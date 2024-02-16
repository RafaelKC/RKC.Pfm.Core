using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using RKC.Pfm.Core.Domain.Periods.Enums;
using RKC.Pfm.Core.Infrastructure.Database;

namespace RKC.Pfm.Core.Application.Periods;

public class PeriodsBackgroundJob: BackgroundService
{
    private readonly RkcPfmCoreDbContext _context;

    public PeriodsBackgroundJob(RkcPfmCoreDbContext context)
    {
        _context = context;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        do
        {
            int hourSpan = 24 - DateTime.Now.Hour;
            int numberOfHours = hourSpan;

            if (hourSpan == 24)
            {
                numberOfHours = 24;
            }

            await Execute();
            
            await Task.Delay(TimeSpan.FromHours(numberOfHours), stoppingToken);
        }
        while (!stoppingToken.IsCancellationRequested);
    }

    private async Task Execute()
    {
        var now = DateTime.Now;
        
        var finalizedPeriods = await _context.Periods
            .Where(p => p.End <= now)
            .Where(p => p.State != PeriodState.Finalized)
            .ToListAsync();
        
        var currentPeriods = await _context.Periods
            .Where(p => p.Start <= now && p.End < now)
            .Where(p => p.State != PeriodState.Current)
            .ToListAsync();
        
        foreach (var finalizedPeriod in finalizedPeriods)
        {
            finalizedPeriod.State = PeriodState.Finalized;
        }
        
        foreach (var currentPeriod in currentPeriods)
        {
            currentPeriod.State = PeriodState.Current;
        }
        
        _context.Periods.UpdateRange(finalizedPeriods);
        _context.Periods.UpdateRange(currentPeriods);
        await _context.SaveChangesAsync();
    }
}