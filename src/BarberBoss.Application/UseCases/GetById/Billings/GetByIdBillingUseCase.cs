using AutoMapper;
using BarberBoss.Communcations.Responses;
using BarberBoss.Domain.Entities;
using BarberBoss.Domain.Repositories;

namespace BarberBoss.Application.UseCases.GetById.Billings;

public class GetByIdBillingUseCase : IGetByIdUseCase<ResponseBillingJson>
{
    private readonly IReadOnlyRepository<Billing> _repository;
    private readonly IMapper _mapper;

    public GetByIdBillingUseCase(IReadOnlyRepository<Billing> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    public async Task<ResponseBillingJson?> Execute(Guid id)
    {
        var billing = await _repository.GetById(id);
        return billing == null ? null : _mapper.Map<ResponseBillingJson>(billing);
    }
}
