namespace BarberBoss.Domain.Records;

public record PaginationSettings
{
    public int PageNumber { get; init; }
    public int PageSize { get; init; }
    public string? OrderBy { get; init; }
}
