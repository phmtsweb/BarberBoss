namespace BarberBoss.Domain.Records;

public record ReportSettings
{
    public DateOnly Start { get; init; }
    public DateOnly End { get; init; }
}
