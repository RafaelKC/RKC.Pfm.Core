using RKC.Pfm.Core.Domain.Communs;

namespace RKC.Pfm.Core.Domain.Periods;

public class PeriodDto: EntityDto
{
    public string Name { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public bool IsSchema { get; set; }
    public Guid? IdSchemaPeriod { get; set; }
    
    public PeriodDto(Period dto)
    {
        Id = dto.Id;
        Name = dto.Name;
        Start = dto.Start;
        End = dto.End;
        IsSchema = dto.IsSchema;
        IdSchemaPeriod = dto.IdSchemaPeriod;
    }
    
    public PeriodDto()
    {
    }
}