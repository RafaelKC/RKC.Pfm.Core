using RKC.Pfm.Core.Application.Communs;

namespace RKC.Pfm.Core.Application.Periods.Dtos;

public class PeriodGetListInput: PagedFilteredInput
{
    public DateTime? EndOnOrBeforeFilter { get; set; }
    public bool OrderAscending { get; set; } = true;
    public bool Schemas { get; set; } = false;
}