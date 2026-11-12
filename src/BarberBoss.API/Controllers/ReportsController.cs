using BarberBoss.Application.UseCases.Reports.Billings;
using BarberBoss.Application.UseCases.Reports.Billings.Formats;
using BarberBoss.Communcations.Constants;
using BarberBoss.Communcations.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace BarberBoss.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ReportsController : ControllerBase
{
    [HttpGet("excel")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetExcelReport(
        [FromServices] IGenerateBillingsReportUseCase<IExcelFormat> useCase,
        [FromQuery] ReportsFilter filters)
    {
        var file = await useCase.Execute(filters);
        return File(file, CustomMediaTypeNames.Application.ExcelXlsx, "report.xlsx");
    }

    [HttpGet("pdf")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetPdfReport(
        [FromServices] IGenerateBillingsReportUseCase<IPdfFormat> useCase,
        [FromQuery] ReportsFilter filters)
    {
        var file = await useCase.Execute(filters);
        return File(file, MediaTypeNames.Application.Pdf, "report.pdf");
    }
}
