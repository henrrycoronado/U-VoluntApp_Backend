namespace U_VoluntApp_Core.Src.Infrastructure.Reports;

using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using U_VoluntApp_Core.Src.Application.Interfaces;
using U_VoluntApp_Core.Src.Domain.Entities.Profile;

public class ScholarshipPdfService : IPdfReportService
{
    public byte[] GenerateScholarshipPerformancePdf(IEnumerable<ScholarshipPerformance> records, string? filterType)
    {
        var recordList = records.ToList();

        var title = string.IsNullOrWhiteSpace(filterType)
            ? "Reporte de Desempeño de Becas"
            : $"Reporte de Desempeño - Beca {filterType.ToUpperInvariant()}";

        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.Letter.Landscape());
                page.MarginHorizontal(30);
                page.MarginVertical(25);
                page.DefaultTextStyle(x => x.FontSize(9));

                page.Header().Column(col =>
                {
                    col.Item().PaddingBottom(5).Text(title)
                        .FontSize(16).Bold().FontColor(Colors.Blue.Darken3);

                    col.Item().PaddingBottom(3).Text($"Fecha de generación: {DateTime.Now:dd/MM/yyyy HH:mm}")
                        .FontSize(8).FontColor(Colors.Grey.Darken1);

                    col.Item().PaddingBottom(3).Text($"Total de registros: {recordList.Count}")
                        .FontSize(8).FontColor(Colors.Grey.Darken1);

                    col.Item().PaddingBottom(8).LineHorizontal(1).LineColor(Colors.Blue.Darken3);
                });

                page.Content().Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn(2.5f);
                        columns.RelativeColumn(1.2f);
                        columns.RelativeColumn(1f);
                        columns.RelativeColumn(1f);
                        columns.RelativeColumn(1f);
                        columns.RelativeColumn(1f);
                        columns.RelativeColumn(1f);
                        columns.RelativeColumn(1.2f);
                    });

                    table.Header(header =>
                    {
                        AddHeaderCell(header, "Voluntario");
                        AddHeaderCell(header, "Tipo Beca");
                        AddHeaderCell(header, "Hrs. Req.");
                        AddHeaderCell(header, "Hrs. Comp.");
                        AddHeaderCell(header, "Hrs. Rest.");
                        AddHeaderCell(header, "% Avance");
                        AddHeaderCell(header, "Estado");
                        AddHeaderCell(header, "Fecha Fin");
                    });

                    foreach (var record in recordList)
                    {
                        AddDataCell(table, $"{record.FirstName} {record.LastName}");
                        AddDataCell(table, record.ScholarshipType);
                        AddDataCell(table, record.RequiredHours.ToString("N2"));
                        AddDataCell(table, record.CompletedHours.ToString("N2"));
                        AddDataCell(table, record.RemainingHours.ToString("N2"));
                        AddPercentageCell(table, record.CompletionPercentage);
                        AddStateCell(table, record.ContractState);
                        AddDataCell(table, record.EndDate?.ToString("dd/MM/yyyy") ?? "Sin definir");
                    }
                });

                page.Footer().AlignCenter().Text(text =>
                {
                    text.Span("U-Voluntapp | Página ");
                    text.CurrentPageNumber();
                    text.Span(" de ");
                    text.TotalPages();
                });
            });
        });

        return document.GeneratePdf();
    }

    private static void AddHeaderCell(TableCellDescriptor header, string text)
    {
        header.Cell().Background(Colors.Blue.Darken3)
            .Padding(4)
            .Text(text)
            .FontSize(8).Bold().FontColor(Colors.White);
    }

    private static void AddDataCell(TableDescriptor table, string text)
    {
        table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2)
            .Padding(4)
            .Text(text)
            .FontSize(8);
    }

    private static void AddPercentageCell(TableDescriptor table, decimal percentage)
    {
        var color = percentage switch
        {
            >= 100 => Colors.Green.Darken2,
            >= 50 => Colors.Orange.Darken2,
            _ => Colors.Red.Darken2,
        };

        table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2)
            .Padding(4)
            .Text($"{percentage:N2}%")
            .FontSize(8).Bold().FontColor(color);
    }

    private static void AddStateCell(TableDescriptor table, string state)
    {
        var color = state.ToLowerInvariant() switch
        {
            "approved" => Colors.Green.Darken2,
            "completed" => Colors.Blue.Darken2,
            "pending" => Colors.Orange.Darken2,
            "rejected" => Colors.Red.Darken2,
            _ => Colors.Grey.Darken2,
        };

        table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2)
            .Padding(4)
            .Text(state)
            .FontSize(8).FontColor(color);
    }
}
