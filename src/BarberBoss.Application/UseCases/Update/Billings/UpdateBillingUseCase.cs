using AutoMapper;
using BarberBoss.Communcations.Requests;
using BarberBoss.Domain.Entities;
using BarberBoss.Domain.Repositories;
using BarberBoss.Exceptions.ExceptionsBase;
using BarberBoss.Exceptions.Resources;

namespace BarberBoss.Application.UseCases.Update.Billings;

public class UpdateBillingUseCase : IUpdateUseCase<RequestRegisterBillingJson>
{
    private readonly IUpdateOnlyRepository<Billing> _repository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateBillingUseCase(IUpdateOnlyRepository<Billing> repository, IMapper mapper, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }
    public async Task Execute(Guid id, RequestRegisterBillingJson request)
    {
        var billing = await _repository.GetForUpdateById(id) ?? throw new NotFoundException(string.Format(ResourceErrorMessages.RESOURCE_NOT_FOUND, id));
        _mapper.Map(request, billing);
        billing.Id = id;
        _repository.Update(billing);
        await _unitOfWork.Commit();
    }
}
