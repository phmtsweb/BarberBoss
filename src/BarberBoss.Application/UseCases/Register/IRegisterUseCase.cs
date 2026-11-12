namespace BarberBoss.Application.UseCases.Register;

public interface IRegisterUseCase<TRequest, TResponse>
{
    Task<TResponse> Execute(TRequest request);
}
