using AutoMapper;
using BarberBoss.Application.Validators.Billings;
using BarberBoss.Communcations.Requests;
using BarberBoss.Communcations.Responses;
using BarberBoss.Domain.Entities;
using BarberBoss.Domain.Repositories;
using BarberBoss.Exceptions.ExceptionsBase;

namespace BarberBoss.Application.UseCases.Register.Billings;

internal class RegisterBillingUseCase : IRegisterUseCase<RequestRegisterBillingJson, ResponseBillingJson>
{
    private readonly IWriteOnlyRepository<Billing> _repository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterBillingUseCase(
        IWriteOnlyRepository<Billing> repository, 
        IMapper mapper,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }
    public async Task<ResponseBillingJson> Execute(RequestRegisterBillingJson request)
    {
        Validate(request);
        var billing = _mapper.Map<Billing>(request);
        _repository.Add(billing);
        await _unitOfWork.Commit();
        return _mapper.Map<ResponseBillingJson>(billing);
    }

    private void Validate(RequestRegisterBillingJson request)
    {
        var validator = new BillingsValidator();
        var result = validator.Validate(request);
        if (!result.IsValid)
        {
            List<string> errors = [.. result.Errors.Select(e => e.ErrorMessage)];
            throw new ErrorOnValidationException(errors);
        };
    }
}
