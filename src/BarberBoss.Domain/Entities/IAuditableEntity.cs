namespace BarberBoss.Domain.Entities;

public interface IAuditableEntity
{
    Guid Id { get; set; }
    DateTime CreatedAt { get; set; }
    DateTime UpdatedAt { get; set; }
}
