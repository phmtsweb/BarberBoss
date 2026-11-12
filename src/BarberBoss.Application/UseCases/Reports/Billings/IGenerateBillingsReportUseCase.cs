using BarberBoss.Application.UseCases.Reports.Billings.Formats;
using BarberBoss.Communcations.Filters;

namespace BarberBoss.Application.UseCases.Reports.Billings;

public interface IGenerateBillingsReportUseCase<T> where T : IReportFormat
{
    Task<byte[]> Execute(ReportsFilter filters);
}
