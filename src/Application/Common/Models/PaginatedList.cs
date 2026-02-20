using System.Text.Json.Serialization;

namespace Application.Common.Models;

public class PaginatedListWithCount<T>
{
    public int PageNumber { get; }
    public int TotalPages { get; }
    public int TotalCount { get; }
    public IReadOnlyCollection<T> Items { get; }

    [JsonConstructor]
    public PaginatedListWithCount(
    int pageNumber,
    int totalPages,
    int totalCount,
    IReadOnlyCollection<T> items)
    {
        Items = items;
        PageNumber = pageNumber;
        TotalPages = totalPages;
        TotalCount = totalCount;
    }


    public PaginatedListWithCount(IReadOnlyCollection<T> items, int count, int pageNumber, int pageSize)
    {
        PageNumber = pageNumber;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        TotalCount = count;
        Items = items;
    }

    public bool HasPreviousPage => PageNumber > 1;

    public bool HasNextPage => PageNumber < TotalPages;

    public static async Task<PaginatedListWithCount<T>> CreateAsync(IQueryable<T> source, int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        var count = await source.CountAsync(cancellationToken);
        var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);

        return new PaginatedListWithCount<T>(items, count, pageNumber, pageSize);
    }
}

public class PaginatedList<T>
{
    public int PageNumber { get; }
    public int PageSize { get; }
    public IReadOnlyCollection<T> Items { get; }

    [JsonConstructor]
    public PaginatedList(
    int pageNumber,
    int pageSize,
    IReadOnlyCollection<T> items)
    {
        Items = items;
        PageNumber = pageNumber;
        PageSize = pageSize;
    }


    public PaginatedList(IReadOnlyCollection<T> items, int pageNumber, int pageSize)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
        Items = items;
    }

    public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);

        return new PaginatedList<T>(items, pageNumber, pageSize);
    }

}