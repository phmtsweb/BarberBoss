namespace BarberBoss.Application.UseCases.Update;

public interface IUpdateUseCase<T>
{
    Task Execute(Guid id, T request);
}
