namespace U_VoluntApp_Core.Src.Infrastructure.Reports;

using U_VoluntApp_Core.Src.Domain.Entities.Profile;

public interface IPdfReportService
{
    byte[] GenerateScholarshipPerformancePdf(IEnumerable<ScholarshipPerformance> records, string? filterType);
}
