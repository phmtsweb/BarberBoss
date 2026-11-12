using AutoMapper;
using BarberBoss.Communcations.Filters;
using BarberBoss.Domain.Records;

namespace BarberBoss.Application.Mappers.Pagination;

public class PaginationMapper : Profile
{
    public PaginationMapper()
    {
        ConfigurePaginationMapping();
    }

    private void ConfigurePaginationMapping()
    {
        CreateMap<PaginationFilter, PaginationSettings>();
    }
}
