using BarberBoss.Domain.Entities;
using BarberBoss.Domain.Repositories;
using BarberBoss.Exceptions.ExceptionsBase;
using BarberBoss.Exceptions.Resources;

namespace BarberBoss.Application.UseCases.Delete.Billings;

public class DeleteBillingUseCase(IWriteOnlyRepository<Billing> repository, IUnitOfWork unitOfWork) : IDeleteUseCase
{
    private readonly IWriteOnlyRepository<Billing> _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    public async Task Execute(Guid id)
    {
        var result = await _repository.Delete(id);
        if (!result)
            throw new NotFoundException(string.Format(ResourceErrorMessages.RESOURCE_NOT_FOUND, id));
        await _unitOfWork.Commit();
    }
}
