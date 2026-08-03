namespace U_VoluntApp_Core.Src.Application.Services;

using U_VoluntApp_Core.Src.Application.DTOs;
using U_VoluntApp_Core.Src.Application.Interfaces;
using U_VoluntApp_Core.Src.Domain.Entities.Profile;
using U_VoluntApp_Core.Src.Infrastructure.Persistence.Interfaces.Profile;
using U_VoluntApp_Core.Src.Infrastructure.Reports;

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
            ?? throw new KeyNotFoundException("Programa no encontrado en analitica");

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
        var record = await _reportRepository.GetLiveVolunteerHistoryByProfileCodeAsync(profileCode)
            ?? throw new KeyNotFoundException("Historial de voluntario no encontrado");

        return MapToDto(record);
    }

    public async Task<HomeSummaryDto> GetHomeSummaryAsync(string profileCode, int? year, int? month)
    {
        var targetYear = year ?? DateTime.UtcNow.Year;
        var targetMonth = month ?? DateTime.UtcNow.Month;

        var record = await _reportRepository.GetLiveHomeSummaryByProfileCodeAsync(profileCode, targetYear, targetMonth)
            ?? throw new KeyNotFoundException("Resumen de inicio no encontrado para el voluntario");

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

    public async Task<AdminHomeSummaryDto> GetAdminHomeSummaryAsync()
    {
        var programs = await _reportRepository.GetProgramAnalyticsAsync();
        var totalVolunteers = await _reportRepository.GetTotalProfilesAsync();
        var scholarships = await _reportRepository.GetScholarshipPerformanceAsync();

        var attendanceData = await _reportRepository.GetMonthlyAttendanceAsync(6);
        var alerts = scholarships
            .Where(s => s.RequiredHours > 0 && s.CompletionPercentage < 50 && s.EndDate.HasValue && (s.EndDate.Value - DateTime.UtcNow).TotalDays < 30)
            .OrderBy(s => s.CompletionPercentage)
            .Take(5)
            .Select(s => new ScholarshipAlertDto
            {
                ProfileCode = s.ProfileCode,
                FullName = $"{s.FirstName} {s.LastName}",
                AvatarUrl = null,
                ScholarshipType = s.ScholarshipType,
                AlertReason = "Riesgo de incumplimiento"
            })
            .ToList();

        var monthlyAttendance = attendanceData.Select(a => new MonthlyAttendanceDto
        {
            Month = a.Item1,
            Hours = a.Item2
        }).ToList();

        return new AdminHomeSummaryDto
        {
            TotalVolunteers = totalVolunteers,
            MonthlyLoggedHours = monthlyAttendance.LastOrDefault()?.Hours ?? 0,
            ActivePrograms = programs.Count(),
            ActiveScholarships = scholarships.Count(),
            MonthlyAttendance = monthlyAttendance,
            ScholarshipAlerts = alerts
        };
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

    private static HomeSummaryDto MapToDto(HomeSummary entity)
    {
        return new HomeSummaryDto
        {
            PersonalGoalHours = entity.PersonalGoalHours,
            ScholarshipGoalHours = entity.ScholarshipGoalHours,
            MonthLoggedHours = entity.MonthLoggedHours,
            TotalLoggedHours = entity.TotalLoggedHours,
            CurrentMonthDailyActivities = entity.CurrentMonthDailyActivities.Select(d => new DailyActivityDto
            {
                Day = d.Day,
                Hours = d.Hours
            }).ToList()
        };
    }
}
