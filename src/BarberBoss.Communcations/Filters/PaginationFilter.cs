namespace BarberBoss.Communcations.Filters;

public class PaginationFilter
{
    private int _pageSize = 10;
    private const int MAX_PAGE_SIZE = 50;

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value > MAX_PAGE_SIZE ? MAX_PAGE_SIZE : value;
    }

    public int PageNumber { get; set; } = 1;
    public string? OrderBy { get; set; } = "date";
}
