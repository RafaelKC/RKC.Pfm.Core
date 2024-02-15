namespace RKC.Pfm.Core.Application.Communs;

public class PagedResult<T>
{
    public long TotalCount { get; set; }
    public List<T> Items { get; set; }
}