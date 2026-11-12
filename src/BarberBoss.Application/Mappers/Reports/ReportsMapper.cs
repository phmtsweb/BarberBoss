using AutoMapper;
using BarberBoss.Communcations.Filters;
using BarberBoss.Domain.Records;

namespace BarberBoss.Application.Mappers.Reports;

public class ReportsMapper : Profile
{
    public ReportsMapper()
    {
        ConfiguringReportsMapping();
    }

    private void ConfiguringReportsMapping()
    {
        CreateMap<ReportsFilter, ReportSettings>();
    }
}
