using BarberBoss.Communcations.Enums;

namespace BarberBoss.Communcations.Responses;

public class ResponseBillingJson
{
    public Guid Id { get; set; }
    public DateOnly Date { get; set; }
    public string BarberName { get; set; } = string.Empty;
    public string ClientName { get; set; } = string.Empty;
    public string ServiceName { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public Status Status { get; set; }
}
