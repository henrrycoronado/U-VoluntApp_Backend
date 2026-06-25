namespace U_VoluntApp_Backend.Src.Application.Interfaces;

using U_VoluntApp_Backend.Src.Application.DTOs;

public interface IReportService
{
    Task<List<ScholarshipPerformanceDto>> GetScholarshipPerformanceAsync();

    Task<List<ScholarshipPerformanceDto>> GetScholarshipPerformanceByTypeAsync(string scholarshipType);

    Task<List<ProgramAnalyticsDto>> GetProgramAnalyticsAsync();

    Task<ProgramAnalyticsDto> GetProgramAnalyticsByCodeAsync(string programCode);

    Task<List<ActivityAnalyticsDto>> GetActivityAnalyticsAsync();

    Task<List<ActivityAnalyticsDto>> GetActivityAnalyticsByProgramAsync(string programCode);

    Task<List<VolunteerHistoryDto>> GetVolunteerHistoryAsync();

    Task<VolunteerHistoryDto> GetVolunteerHistoryByProfileCodeAsync(string profileCode);

    Task<HomeSummaryDto> GetHomeSummaryAsync(string profileCode, int? year, int? month);

    Task<byte[]> GenerateScholarshipPdfAsync(string? scholarshipType);

    Task RefreshAnalyticsAsync();
}
