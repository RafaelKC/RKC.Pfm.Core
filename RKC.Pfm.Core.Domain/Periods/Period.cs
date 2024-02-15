using RKC.Pfm.Core.Domain.Communs;

namespace RKC.Pfm.Core.Domain.Periods;

public class Period: Entity, IHasUserId
{
    public string Name { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public Guid UserId { get; set; }
    public bool IsSchema { get; set; }
    
    public Guid? IdSchemaPeriod { get; set; }
    public Period? SchemaPeriod { get; set; }
    public ICollection<Period> BasedPeriods { get; set; }
}