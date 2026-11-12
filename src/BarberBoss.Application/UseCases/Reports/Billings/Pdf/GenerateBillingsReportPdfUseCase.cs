using AutoMapper;
using BarberBoss.Application.UseCases.Reports.Billings.Formats;
using BarberBoss.Application.UseCases.Reports.Billings.Pdf.Fonts;
using BarberBoss.Communcations.Filters;
using BarberBoss.Domain.Entities;
using BarberBoss.Domain.Extensions;
using BarberBoss.Domain.Records;
using BarberBoss.Domain.Repositories;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using PdfSharp.Fonts;
using System.Reflection;

namespace BarberBoss.Application.UseCases.Reports.Billings.Pdf;

public class GenerateBillingsReportPdfUseCase : IGenerateBillingsReportUseCase<IPdfFormat>
{
    private const int ROW_HEIGHT_BILLING_TABLE = 25;
    private readonly IReadOnlyRepository<Billing> _repository;
    private readonly IMapper _mapper;

    public GenerateBillingsReportPdfUseCase(IReadOnlyRepository<Billing> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
        GlobalFontSettings.FontResolver = new FontResolver();
    }
    public async Task<byte[]> Execute(ReportsFilter filters)
    {
        var settings = _mapper.Map<ReportSettings>(filters);
        var billings = await _repository.GetAllByPeriod(settings);
        if (billings.Count == 0) return [];
        
        // Create document and first page
        var document = CreateDocument(settings);
        var page = CreatePage(document);
        
        // Create Header with image
        CreateHeader(page);

        // Create total billings
        var totalBillings = billings.Sum(b => b.Amount);
        CreateTotalBillings(page, totalBillings, settings);

        // Create table with billings
        var index = 0;
        foreach (var billing in billings)
        {
            CreateExpenseTableItem(page, billing, index != billings.Count - 1);
            index++;
        }
        // Render and return document
        return RenderDocument(document);
    }

    private Document CreateDocument(ReportSettings settings)
    {
        var document = new Document();
        document.Info.Title = $"Faturamento de {settings.Start:dd/MM/yyyy} a {settings.End:dd/MM/yyyy}";
        document.Info.Title = "Pedro Mascarenhas";
        var style = document.Styles["Normal"];
        style!.Font.Name = FontHelper.ROBOTO_REGULAR;
        return document;
    }

    private Section CreatePage(Document document)
    {
        var section = document.AddSection();
        section.PageSetup = document.DefaultPageSetup.Clone();
        section.PageSetup.PageFormat = PageFormat.A4;
        section.PageSetup.LeftMargin = 35;
        section.PageSetup.RightMargin = 35;
        section.PageSetup.TopMargin = 55;
        section.PageSetup.BottomMargin = 55;

        return section;
    }

    private void CreateHeader(Section page)
    {
        var header = page.AddTable();
        header.AddColumn();
        var textColumn = header.AddColumn("250");
        textColumn.LeftPadding = 16;

        var row = header.AddRow();

        var assembly = Assembly.GetExecutingAssembly();
        var directory = Path.GetDirectoryName(assembly.Location);
        row.Cells[0].AddImage(Path.Combine(directory!, "UseCases", "Reports", "Billings", "Pdf", "Logo", "barber-logo.png"));
        var textCell = row.Cells[1].AddParagraph();
        textCell.AddFormattedText("BARBER BOSS", new Font
        {
            Name = FontHelper.BEBAS_NEUE_REGULAR,
            Size = 25,
        });
        row.Cells[1].VerticalAlignment = VerticalAlignment.Center;
    }

    private void CreateTotalBillings(Section page, decimal totalBillings, ReportSettings settings)
    {
        var paragraph = page.AddParagraph();
        paragraph.Format.SpaceBefore = 36;
        paragraph.Format.SpaceAfter = 64;
        paragraph.AddFormattedText($"Faturamento de {settings.Start:dd/MM/yyyy} a {settings.End:dd/MM/yyyy}", new Font
        {
            Name = FontHelper.ROBOTO_MEDIUM,
            Size = 15,
        });
        paragraph.AddLineBreak();
        paragraph.AddFormattedText($"R$ {totalBillings:F2}", new Font 
        { 
            Name = FontHelper.BEBAS_NEUE_REGULAR,
            Size = 50
        });
    }

    private Table CreateExpenseTableItem(Section page, Billing billing, bool addMarginBottom = true)
    {
        var table = page.AddTable();
        var col1 = table.AddColumn("195");
        col1.Format.Alignment = ParagraphAlignment.Left;
        col1.LeftPadding = 20;
        var col2 = table.AddColumn("80");
        col2.Format.Alignment = ParagraphAlignment.Center;
        var col3 = table.AddColumn("120");
        col3.Format.Alignment = ParagraphAlignment.Center;
        var col4 = table.AddColumn("120");
        col4.Format.Alignment = ParagraphAlignment.Right;
        col4.RightPadding = 20;

        var row1 = table.AddRow();
        row1.Height = ROW_HEIGHT_BILLING_TABLE;
        row1[0].AddParagraph(billing.ServiceName.ToUpper());
        row1[0].Format.Font = new Font 
        {
            Name = FontHelper.BEBAS_NEUE_REGULAR,
            Size = 15,
            Color = Color.Parse("#FFFFFF")
        };
        row1[0].Shading.Color = Color.Parse("#1A2B2B");
        row1[0].VerticalAlignment = VerticalAlignment.Center;
        row1[0].MergeRight = 2;
        row1[3].AddParagraph("AMOUNT");
        row1[3].Format.Font = new Font
        {
            Name = FontHelper.BEBAS_NEUE_REGULAR,
            Size = 15,
            Color = Color.Parse("#FFFFFF")
        };
        row1[3].Shading.Color = Color.Parse("#205858");
        row1[3].VerticalAlignment = VerticalAlignment.Center;
        row1[3].Format.Alignment = ParagraphAlignment.Right;

        var row2 = table.AddRow();
        row2.Height = ROW_HEIGHT_BILLING_TABLE;
        row2[0].AddParagraph(billing.Date.ToDateTime(TimeOnly.MinValue).ToString("D"));
        row2[0].Format.Font = new Font 
        {
            Name = FontHelper.ROBOTO_REGULAR,
            Size = 10,
            Color = Color.Parse("#000000")
        };
        row2[0].Shading.Color = Color.Parse("#D7DFDF");
        row2[0].Format.Alignment = ParagraphAlignment.Left;
        row2[0].VerticalAlignment = VerticalAlignment.Center;

        row2[1].AddParagraph(billing.Date.ToDateTime(TimeOnly.MinValue).ToString("T"));
        row2[1].Format.Font = new Font
        {
            Name = FontHelper.ROBOTO_REGULAR,
            Size = 10,
            Color = Color.Parse("#000000")
        };
        row2[1].Shading.Color = Color.Parse("#D7DFDF");
        row2[1].Format.Alignment = ParagraphAlignment.Center;
        row2[1].VerticalAlignment = VerticalAlignment.Center;

        row2[2].AddParagraph(billing.PaymentMethod.PaymentMethodToString());
        row2[2].Format.Font = new Font
        {
            Name = FontHelper.ROBOTO_REGULAR,
            Size = 10,
            Color = Color.Parse("#000000")
        };
        row2[2].Shading.Color = Color.Parse("#D7DFDF");
        row2[2].Format.Alignment = ParagraphAlignment.Center;
        row2[2].VerticalAlignment = VerticalAlignment.Center;

        row2[2].Shading.Color = Color.Parse("#D7DFDF");
        row2[2].Format.Alignment = ParagraphAlignment.Center;
        row2[2].VerticalAlignment = VerticalAlignment.Center;

        row2[3].AddParagraph($"R$ {billing.Amount:F2}");
        row2[3].Format.Font = new Font
        {
            Name = FontHelper.ROBOTO_REGULAR,
            Size = 10,
            Color = Color.Parse("#000000")
        };
        row2[3].Format.Alignment = ParagraphAlignment.Right;
        row2[3].VerticalAlignment = VerticalAlignment.Center;

        if (!string.IsNullOrWhiteSpace(billing.Notes))
        {
            var row3 = table.AddRow();
            row3.Height = ROW_HEIGHT_BILLING_TABLE;
            row3[0].AddParagraph(billing.Notes.Length > 100 ? billing.Notes[..100] : billing.Notes);
            row3[0].Format.Font = new Font 
            {
                Name = FontHelper.ROBOTO_REGULAR,
                Size = 9,
                Color = Color.Parse("#7D7D7D")
            };
            row3[0].Shading.Color = Color.Parse("#EDF3F3");
            row3[0].Format.Alignment = ParagraphAlignment.Left;
            row3[0].VerticalAlignment = VerticalAlignment.Center;
            row3[0].MergeRight = 2;
            row2[3].MergeDown = 1;
        }

        if (addMarginBottom)
        {
            table.AddRow().Height = 30;
        }

        return table;
    }

    private byte[] RenderDocument(Document document)
    {
        using var stream = new MemoryStream();
        var renderer = new PdfDocumentRenderer
        {
            Document = document
        };
        renderer.RenderDocument();
        renderer.PdfDocument.Save(stream);
        return stream.ToArray();
    }
}
