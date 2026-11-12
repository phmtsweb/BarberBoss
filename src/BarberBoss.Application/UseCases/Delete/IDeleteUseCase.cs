namespace BarberBoss.Application.UseCases.Delete;

public interface IDeleteUseCase
{
    Task Execute(Guid id);
}
