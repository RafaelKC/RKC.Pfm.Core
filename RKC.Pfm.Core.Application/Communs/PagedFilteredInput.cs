namespace RKC.Pfm.Core.Application.Communs;

public class PagedFilteredInput
{
    public int SkipCount { get; set; }
    public int MaxResultCount { get; set; }
    public string Filter { get; set; }
}