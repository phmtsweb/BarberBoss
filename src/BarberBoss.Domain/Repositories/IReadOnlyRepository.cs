using BarberBoss.Domain.Records;

namespace BarberBoss.Domain.Repositories;

public interface IReadOnlyRepository<T>
{
    Task<(List<T> Items, int TotalCount)> GetAll(PaginationSettings settings);
    Task<List<T>> GetAllByPeriod(ReportSettings settings);
    Task<T?> GetById(Guid id);
}
