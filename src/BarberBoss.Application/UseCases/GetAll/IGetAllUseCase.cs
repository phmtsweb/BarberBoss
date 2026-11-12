using BarberBoss.Communcations.Filters;
using BarberBoss.Communcations.Responses;

namespace BarberBoss.Application.UseCases.GetAll;

public interface IGetAllUseCase<T>
{
    Task<PagedResponseJson<T>> Execute(PaginationFilter filters);
}
