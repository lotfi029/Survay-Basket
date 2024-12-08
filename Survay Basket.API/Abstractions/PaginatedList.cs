namespace Survay_Basket.API.Abstractions;

public class PaginatedList<T>(List<T> items, int pageNumber, int count, int pageSize)
{
    public List<T> Items { get; private set; } = items;
    public int TotalPage { get; set; } = (int)Math.Ceiling(count / (double)pageSize);
    public int PageNumber { get; set; } = pageNumber;
    public bool HasPreviousPage => PageNumber > 1;
    public bool HasNextPage => TotalPage > PageNumber;

    public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source,int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        var count = await source.CountAsync(cancellationToken);
        var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);

        return new PaginatedList<T>(items, pageNumber, count, pageSize); 
    }
}
