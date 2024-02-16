using RKC.Pfm.Core.Domain.Communs;
using RKC.Pfm.Core.Domain.Periods.Enums;

namespace RKC.Pfm.Core.Domain.Periods;

public class Period: Entity, IHasUserId
{
    public string Name { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public Guid UserId { get; set; }
    public bool IsSchema { get; set; }
    public PeriodState State { get; set; }
    
    public Guid? IdSchemaPeriod { get; set; }
    public Period? SchemaPeriod { get; set; }
    public ICollection<Period> BasedPeriods { get; set; }

    public Period()
    {
    }
    
    public Period(PeriodDto dto)
    {
        Id = dto.Id;
        Name = dto.Name;
        Start = dto.Start;
        End = dto.End;
        IsSchema = dto.IsSchema;
        IdSchemaPeriod = dto.IdSchemaPeriod;
        State = dto.State;
    }
}