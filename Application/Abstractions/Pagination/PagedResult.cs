namespace Application.Abstractions.Pagination;

public class PagedResult<T>(List<T> items, int pageIndex, int totalCount, int pageSize)
{
    public List<T> Items { get; set; } = items;
    public int TotalCount { get; set; } = totalCount;
    public int PageIndex { get; set; } = pageIndex;
    public int TotalPages { get; set; } = (int)Math.Ceiling(totalCount / (double)pageSize);

    public static PagedResult<T> Default
        => default!;
}
