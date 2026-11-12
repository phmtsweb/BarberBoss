namespace BarberBoss.Communcations.Responses;

public class PagedResponseJson<T>
{
    public int TotalCount { get; set; }
    public int CurrentPage { get; set; }
    public int PageSize { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    public List<T> Data { get; set; } = [];
}
