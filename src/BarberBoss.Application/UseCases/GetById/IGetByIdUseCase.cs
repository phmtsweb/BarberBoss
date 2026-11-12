using BarberBoss.Communcations.Responses;

namespace BarberBoss.Application.UseCases.GetById;

public interface IGetByIdUseCase<T>
{
    Task<T?> Execute(Guid id);
}
