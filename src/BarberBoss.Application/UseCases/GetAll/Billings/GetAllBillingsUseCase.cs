using AutoMapper;
using BarberBoss.Application.Validators.Utils;
using BarberBoss.Communcations.Filters;
using BarberBoss.Communcations.Responses;
using BarberBoss.Domain.Entities;
using BarberBoss.Domain.Records;
using BarberBoss.Domain.Repositories;
using BarberBoss.Exceptions.ExceptionsBase;

namespace BarberBoss.Application.UseCases.GetAll.Billings;

public class GetAllBillingsUseCase : IGetAllUseCase<ResponseShortBillingJson>
{
    private readonly IReadOnlyRepository<Billing> _repository;
    private readonly IMapper _mapper;

    public GetAllBillingsUseCase(IReadOnlyRepository<Billing> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<PagedResponseJson<ResponseShortBillingJson>> Execute(PaginationFilter filters)
    {
        ValidateFilters(filters);
        var settings = _mapper.Map<PaginationSettings>(filters);
        var (Items, TotalCount) = await _repository.GetAll(settings);
        var response = new PagedResponseJson<ResponseShortBillingJson>
        {
            Data = Items.Select(b => _mapper.Map<ResponseShortBillingJson>(b)).ToList(),
            TotalCount = TotalCount,
            CurrentPage = settings.PageNumber,
            PageSize = settings.PageSize
            
        };
        return response;
    }

    private void ValidateFilters(PaginationFilter filters)
    {
        var validator = new PaginationFilterValidation();
        var result = validator.Validate(filters);
        if (!result.IsValid)
        {
            var errors = result.Errors.Select(e => e.ErrorMessage).ToList();
            throw new ErrorOnValidationException(errors);
        }
    }
}
