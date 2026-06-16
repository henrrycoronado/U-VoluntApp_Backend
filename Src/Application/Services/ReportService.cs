namespace U_VoluntApp_Backend.Src.Application.Services;

using U_VoluntApp_Backend.Src.Application.DTOs;
using U_VoluntApp_Backend.Src.Application.Interfaces;
using U_VoluntApp_Backend.Src.Domain.Entities.Profile;
using U_VoluntApp_Backend.Src.Infrastructure.Persistence.Interfaces.Profile;

public class ReportService : IReportService
{
    private readonly IReportRepository _reportRepository;
    private readonly IPdfReportService _pdfReportService;

    public ReportService(
        IReportRepository reportRepository,
        IPdfReportService pdfReportService)
    {
        _reportRepository = reportRepository;
        _pdfReportService = pdfReportService;
    }

    public async Task<List<ScholarshipPerformanceDto>> GetScholarshipPerformanceAsync()
    {
        var records = await _reportRepository.GetScholarshipPerformanceAsync();
        return records.Select(MapToDto).ToList();
    }

    public async Task<List<ScholarshipPerformanceDto>> GetScholarshipPerformanceByTypeAsync(string scholarshipType)
    {
        if (string.IsNullOrWhiteSpace(scholarshipType))
        {
            throw new InvalidOperationException("Debes indicar el tipo de beca");
        }

        var records = await _reportRepository.GetScholarshipPerformanceByTypeAsync(scholarshipType);
        return records.Select(MapToDto).ToList();
    }

    public async Task<List<ProgramAnalyticsDto>> GetProgramAnalyticsAsync()
    {
        var records = await _reportRepository.GetProgramAnalyticsAsync();
        return records.Select(MapToDto).ToList();
    }

    public async Task<ProgramAnalyticsDto> GetProgramAnalyticsByCodeAsync(string programCode)
    {
        var record = await _reportRepository.GetProgramAnalyticsByCodeAsync(programCode)
            ?? throw new InvalidOperationException("Programa no encontrado en analitica");

        return MapToDto(record);
    }

    public async Task<List<ActivityAnalyticsDto>> GetActivityAnalyticsAsync()
    {
        var records = await _reportRepository.GetActivityAnalyticsAsync();
        return records.Select(MapToDto).ToList();
    }

    public async Task<List<ActivityAnalyticsDto>> GetActivityAnalyticsByProgramAsync(string programCode)
    {
        var records = await _reportRepository.GetActivityAnalyticsByProgramCodeAsync(programCode);
        return records.Select(MapToDto).ToList();
    }

    public async Task<List<VolunteerHistoryDto>> GetVolunteerHistoryAsync()
    {
        var records = await _reportRepository.GetVolunteerHistoryAsync();
        return records.Select(MapToDto).ToList();
    }

    public async Task<VolunteerHistoryDto> GetVolunteerHistoryByProfileCodeAsync(string profileCode)
    {
        var record = await _reportRepository.GetVolunteerHistoryByProfileCodeAsync(profileCode)
            ?? throw new InvalidOperationException("Historial de voluntario no encontrado");

        return MapToDto(record);
    }

    public async Task<byte[]> GenerateScholarshipPdfAsync(string? scholarshipType)
    {
        IEnumerable<ScholarshipPerformance> records;
        var normalizedType = string.IsNullOrWhiteSpace(scholarshipType) ? null : scholarshipType.Trim();

        if (!string.IsNullOrWhiteSpace(normalizedType))
        {
            records = await _reportRepository.GetScholarshipPerformanceByTypeAsync(normalizedType);
        }
        else
        {
            records = await _reportRepository.GetScholarshipPerformanceAsync();
        }

        return _pdfReportService.GenerateScholarshipPerformancePdf(records, normalizedType);
    }

    public async Task RefreshAnalyticsAsync()
    {
        await _reportRepository.RefreshMaterializedViewsAsync();
    }

    private static ScholarshipPerformanceDto MapToDto(ScholarshipPerformance entity)
    {
        return new ScholarshipPerformanceDto
        {
            ProfileCode = entity.ProfileCode,
            FullName = $"{entity.FirstName} {entity.LastName}",
            ScholarshipType = entity.ScholarshipType,
            RequiredHours = entity.RequiredHours,
            CompletedHours = entity.CompletedHours,
            RemainingHours = entity.RemainingHours,
            CompletionPercentage = entity.CompletionPercentage,
            ContractState = entity.ContractState,
            EndDate = entity.EndDate,
        };
    }

    private static ProgramAnalyticsDto MapToDto(ProgramAnalytics entity)
    {
        return new ProgramAnalyticsDto
        {
            ProgramCode = entity.ProgramCode,
            ProgramName = entity.ProgramName,
            TotalActivities = entity.TotalActivities,
            TotalUniqueVolunteers = entity.TotalUniqueVolunteers,
            TotalGeneratedHours = entity.TotalGeneratedHours,
        };
    }

    private static ActivityAnalyticsDto MapToDto(ActivityAnalytics entity)
    {
        return new ActivityAnalyticsDto
        {
            ActivityCode = entity.ActivityCode,
            ProgramCode = entity.ProgramCode,
            ProgramName = entity.ProgramName,
            ActivityName = entity.ActivityName,
            StartDate = entity.StartDate,
            EndDate = entity.EndDate,
            TotalCapacity = entity.TotalCapacity,
            TotalEnrolled = entity.TotalEnrolled,
            TotalAttended = entity.TotalAttended,
            TotalActivityHours = entity.TotalActivityHours,
        };
    }

    private static VolunteerHistoryDto MapToDto(VolunteerHistory entity)
    {
        return new VolunteerHistoryDto
        {
            ProfileCode = entity.ProfileCode,
            FullName = $"{entity.FirstName} {entity.LastName}",
            CareerName = entity.CareerName,
            PersonalGoalHours = entity.PersonalGoalHours,
            TotalActivitiesParticipated = entity.TotalActivitiesParticipated,
            ValidatedHours = entity.ValidatedHours,
            TotalLoggedHours = entity.TotalLoggedHours,
            LastActivityDate = entity.LastActivityDate,
        };
    }
}
