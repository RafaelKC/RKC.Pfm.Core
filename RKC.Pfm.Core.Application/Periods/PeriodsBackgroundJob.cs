using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RKC.Pfm.Core.Domain.Periods.Enums;
using RKC.Pfm.Core.Infrastructure.Database;

namespace RKC.Pfm.Core.Application.Periods;

public class PeriodsBackgroundJob: BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public PeriodsBackgroundJob(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
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
        using var scope = _serviceScopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<RkcPfmCoreDbContext>();
        
        var now = DateTime.Now;
        
        var finalizedPeriods = await context.Periods
            .Where(p => p.End <= now)
            .Where(p => p.State != PeriodState.Finalized)
            .ToListAsync();
        
        var currentPeriods = await context.Periods
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
        
        context.Periods.UpdateRange(finalizedPeriods);
        context.Periods.UpdateRange(currentPeriods);
        await context.SaveChangesAsync();
    }
}