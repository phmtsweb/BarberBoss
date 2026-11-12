using BarberBoss.Application.Mappers.Billings;
using BarberBoss.Application.Mappers.Pagination;
using BarberBoss.Application.Mappers.Reports;
using BarberBoss.Application.UseCases.Delete;
using BarberBoss.Application.UseCases.Delete.Billings;
using BarberBoss.Application.UseCases.GetAll;
using BarberBoss.Application.UseCases.GetAll.Billings;
using BarberBoss.Application.UseCases.GetById;
using BarberBoss.Application.UseCases.GetById.Billings;
using BarberBoss.Application.UseCases.Register;
using BarberBoss.Application.UseCases.Register.Billings;
using BarberBoss.Application.UseCases.Reports.Billings;
using BarberBoss.Application.UseCases.Reports.Billings.Excel;
using BarberBoss.Application.UseCases.Reports.Billings.Formats;
using BarberBoss.Application.UseCases.Reports.Billings.Pdf;
using BarberBoss.Application.UseCases.Update;
using BarberBoss.Application.UseCases.Update.Billings;
using BarberBoss.Communcations.Requests;
using BarberBoss.Communcations.Responses;
using Microsoft.Extensions.DependencyInjection;

namespace BarberBoss.Application;

public static class DependecyInjectionExtensions
{
    private static void AddAutoMappers(IServiceCollection services)
    {
        services.AddAutoMapper(config => config.AddProfile<BillingMapper>());
        services.AddAutoMapper(config => config.AddProfile<PaginationMapper>());
        services.AddAutoMapper(config => config.AddProfile<ReportsMapper>());
    }
    private static void AddUseCases(IServiceCollection services)
    {
        services.AddScoped<IRegisterUseCase<RequestRegisterBillingJson, ResponseBillingJson>, RegisterBillingUseCase>();
        services.AddScoped<IGetAllUseCase<ResponseShortBillingJson>, GetAllBillingsUseCase>();
        services.AddScoped<IGetByIdUseCase<ResponseBillingJson>, GetByIdBillingUseCase>();
        services.AddScoped<IUpdateUseCase<RequestRegisterBillingJson>, UpdateBillingUseCase>();
        services.AddScoped<IDeleteUseCase, DeleteBillingUseCase>();
        services.AddScoped<IGenerateBillingsReportUseCase<IExcelFormat>, GenerateBillingsReportExcelUseCase>();
        services.AddScoped<IGenerateBillingsReportUseCase<IPdfFormat>, GenerateBillingsReportPdfUseCase>();
    }

    public static void AddApplication(this IServiceCollection services)
    {
        AddAutoMappers(services);
        AddUseCases(services);
    }
}
