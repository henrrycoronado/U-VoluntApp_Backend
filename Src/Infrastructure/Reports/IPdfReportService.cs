namespace U_VoluntApp_Backend.Src.Infrastructure.Reports;

using U_VoluntApp_Backend.Src.Domain.Entities.Profile;

public interface IPdfReportService
{
    byte[] GenerateScholarshipPerformancePdf(IEnumerable<ScholarshipPerformance> records, string? filterType);
}
