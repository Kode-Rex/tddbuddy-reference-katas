namespace Pagination;

public sealed record PageRequest
{
    public int PageNumber { get; }
    public int PageSize { get; }
    public int TotalItems { get; }

    public PageRequest(int pageNumber, int pageSize, int totalItems)
    {
        if (totalItems < 0)
            throw new ArgumentException("totalItems must be >= 0", nameof(totalItems));
        if (pageSize < 1)
            throw new ArgumentException("pageSize must be >= 1", nameof(pageSize));

        PageSize = pageSize;
        TotalItems = totalItems;
        PageNumber = ClampPageNumber(pageNumber, TotalPagesFor(totalItems, pageSize));
    }

    public int TotalPages => TotalPagesFor(TotalItems, PageSize);

    public int StartItem => TotalItems == 0 ? 0 : (PageNumber - 1) * PageSize + 1;

    public int EndItem => TotalItems == 0 ? 0 : Math.Min(PageNumber * PageSize, TotalItems);

    public bool HasPrevious => PageNumber > 1;

    public bool HasNext => PageNumber < TotalPages;

    public IReadOnlyList<int> PageWindow(int windowSize)
    {
        if (windowSize <= 0 || TotalPages == 0) return Array.Empty<int>();

        var size = Math.Min(windowSize, TotalPages);
        var half = size / 2;
        var start = PageNumber - half;
        var end = start + size - 1;

        if (start < 1) { start = 1; end = size; }
        if (end > TotalPages) { end = TotalPages; start = end - size + 1; }

        var window = new int[size];
        for (var i = 0; i < size; i++) window[i] = start + i;
        return window;
    }

    private static int TotalPagesFor(int totalItems, int pageSize) =>
        totalItems == 0 ? 0 : (totalItems + pageSize - 1) / pageSize;

    private static int ClampPageNumber(int requested, int totalPages)
    {
        if (totalPages == 0) return 1;
        if (requested < 1) return 1;
        if (requested > totalPages) return totalPages;
        return requested;
    }
}
