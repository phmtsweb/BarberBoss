namespace BarberBoss.Domain.Repositories;

public interface IUpdateOnlyRepository<T>
{
    void Update(T entity);
    Task<T?> GetForUpdateById(Guid id);
}
