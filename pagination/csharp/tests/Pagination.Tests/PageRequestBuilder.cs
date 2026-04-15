namespace Pagination.Tests;

public class PageRequestBuilder
{
    private int _pageNumber = PageDefaults.DefaultPageNumber;
    private int _pageSize = PageDefaults.DefaultPageSize;
    private int _totalItems = PageDefaults.DefaultTotalItems;

    public PageRequestBuilder PageNumber(int n) { _pageNumber = n; return this; }
    public PageRequestBuilder PageSize(int n) { _pageSize = n; return this; }
    public PageRequestBuilder TotalItems(int n) { _totalItems = n; return this; }

    public PageRequest Build() => new(_pageNumber, _pageSize, _totalItems);
}
