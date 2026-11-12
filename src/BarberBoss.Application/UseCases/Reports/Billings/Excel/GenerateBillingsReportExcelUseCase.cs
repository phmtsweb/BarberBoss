using AutoMapper;
using BarberBoss.Application.UseCases.Reports.Billings.Formats;
using BarberBoss.Communcations.Filters;
using BarberBoss.Domain.Entities;
using BarberBoss.Domain.Extensions;
using BarberBoss.Domain.Records;
using BarberBoss.Domain.Repositories;
using BarberBoss.Domain.Resources.Reports;
using ClosedXML.Excel;

namespace BarberBoss.Application.UseCases.Reports.Billings.Excel;

public class GenerateBillingsReportExcelUseCase(IReadOnlyRepository<Billing> repository, IMapper mapper) : IGenerateBillingsReportUseCase<IExcelFormat>
{
    private readonly IReadOnlyRepository<Billing> _repository = repository;
    private readonly IMapper _mapper = mapper;
    private const string CURRENCY_FORMAT = "R$ #,##0.00";
    public async Task<byte[]> Execute(ReportsFilter filters)
    {

        var reportSettings = _mapper.Map<ReportSettings>(filters);
        var billings = await _repository.GetAllByPeriod(reportSettings);

        if (billings.Count == 0)
        {
            return [];
        }

        using var workbook = new XLWorkbook 
        {
            Author = "Pedro Mascarenhas"
        };
        workbook.Style.Font.FontSize = 12;
        workbook.Style.Font.FontName = "Times New Roman";
        var worksheet = workbook.Worksheets.Add($"{filters.Start:yyyyMMdd}_{filters.End:yyyyMMdd}");
        
        InsertHeader(worksheet);

        var line = 2;

        foreach (var billing in billings)
        {
            worksheet.Cells($"A{line}").Value = billing.ServiceName;
            worksheet.Cells($"B{line}").Value = billing.Date.ToDateTime(TimeOnly.MinValue);
            worksheet.Cells($"C{line}").Value = billing.PaymentMethod.PaymentMethodToString();
            worksheet.Cells($"D{line}").Value = billing.Amount;
            worksheet.Cells($"E{line}").Value = billing.Notes;
            //Styles
            worksheet.Cells($"D{line}").Style.NumberFormat.Format = CURRENCY_FORMAT;
            worksheet.Cells($"D{line}").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
            line++;
        }

        worksheet.Columns().AdjustToContents();

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }

    private void InsertHeader(IXLWorksheet worksheet)
    {
        worksheet.Cells("A1").Value = ResourceReportMessages.TITLE.ToUpper();
        worksheet.Cells("B1").Value = ResourceReportMessages.DATE.ToUpper();
        worksheet.Cells("C1").Value = ResourceReportMessages.PAYMENT_METHOD.ToUpper();
        worksheet.Cells("D1").Value = ResourceReportMessages.AMOUNT.ToUpper();
        worksheet.Cells("E1").Value = ResourceReportMessages.DESCRIPTION.ToUpper();

        //Styles
        var headerRange = worksheet.Range("A1:E1"); 
        headerRange.Style.Font.Bold = true;
        headerRange.Style.Font.FontColor = XLColor.FromHtml("#FFFFFF");
        headerRange.Style.Fill.BackgroundColor = XLColor.FromHtml("#205858");
        headerRange.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        worksheet.Cells("D1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
    }
}
