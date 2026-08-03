namespace U_VoluntApp_Core.Src.Infrastructure.Persistence.Repositories;

using Microsoft.EntityFrameworkCore;
using U_VoluntApp_Core.Src.Domain.Entities.Profile;
using U_VoluntApp_Core.Src.Infrastructure.Persistence.Interfaces.Profile;
using U_VoluntApp_Core.Src.Infrastructure.Persistence.Mappers;

public class ReportRepository : IReportRepository
{
    private readonly AppDbContext _context;

    public ReportRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ScholarshipPerformance>> GetScholarshipPerformanceAsync()
    {
        var records = await _context.MvScholarshipPerformances
            .AsNoTracking()
            .ToListAsync();
        return records.Select(DomainPersistenceMapper.ToDomain).ToList();
    }

    public async Task<IEnumerable<ScholarshipPerformance>> GetScholarshipPerformanceByTypeAsync(string scholarshipType)
    {
        var normalizedType = scholarshipType.Trim().ToLower();

        var records = await _context.MvScholarshipPerformances
            .AsNoTracking()
            .Where(s => s.ScholarshipType != null && s.ScholarshipType.ToLower() == normalizedType)
            .ToListAsync();
        return records.Select(DomainPersistenceMapper.ToDomain).ToList();
    }

    public async Task<IEnumerable<ProgramAnalytics>> GetProgramAnalyticsAsync()
    {
        var records = await _context.MvProgramAnalytics
            .AsNoTracking()
            .ToListAsync();
        return records.Select(DomainPersistenceMapper.ToDomain).ToList();
    }

    public async Task<ProgramAnalytics?> GetProgramAnalyticsByCodeAsync(string programCode)
    {
        var record = await _context.MvProgramAnalytics
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.ProgramCode == programCode);
        return record is null ? null : DomainPersistenceMapper.ToDomain(record);
    }

    public async Task<IEnumerable<ActivityAnalytics>> GetActivityAnalyticsAsync()
    {
        var records = await _context.MvActivityAnalytics.AsNoTracking().ToListAsync();
        return records.Select(DomainPersistenceMapper.ToDomain).ToList();
    }

    public async Task<IEnumerable<ActivityAnalytics>> GetActivityAnalyticsByProgramCodeAsync(string programCode)
    {
        var records = await _context.MvActivityAnalytics
            .AsNoTracking()
            .Where(a => a.ProgramCode == programCode)
            .ToListAsync();
        return records.Select(DomainPersistenceMapper.ToDomain).ToList();
    }

    public async Task<IEnumerable<VolunteerHistory>> GetVolunteerHistoryAsync()
    {
        var records = await _context.MvVolunteerHistories.AsNoTracking().ToListAsync();
        return records.Select(DomainPersistenceMapper.ToDomain).ToList();
    }

    public async Task<VolunteerHistory?> GetVolunteerHistoryByProfileCodeAsync(string profileCode)
    {
        var record = await _context.MvVolunteerHistories
            .AsNoTracking()
            .FirstOrDefaultAsync(v => v.ProfileCode == profileCode);
        return record is null ? null : DomainPersistenceMapper.ToDomain(record);
    }

    public async Task<VolunteerHistory?> GetLiveVolunteerHistoryByProfileCodeAsync(string profileCode)
    {
        var profile = await _context.Profiles
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.UvaCode == profileCode && p.DeletedAt == null);

        if (profile == null)
        {
            return null;
        }

        var enrollments = await _context.Enrollments
            .AsNoTracking()
            .Where(e => e.EnrolledProfileCode == profileCode && e.StateCode == "stage-2" && e.DeletedAt == null)
            .Select(e => e.UvaCode)
            .ToListAsync();

        var trackingLogs = await _context.TrackingLogs
            .AsNoTracking()
            .Where(t => enrollments.Contains(t.EnrollmentCode) && t.DeletedAt == null)
            .ToListAsync();

        int totalActivitiesParticipated = await _context.Enrollments
            .AsNoTracking()
            .Where(e => e.EnrolledProfileCode == profileCode && e.StateCode == "stage-2" && e.DeletedAt == null)
            .Select(e => e.ActivityCode)
            .Distinct()
            .CountAsync();

        decimal validatedHours = trackingLogs
            .Where(t => t.StateCode == "stage-2")
            .Sum(t => t.CalculatedHours);

        decimal totalLoggedHours = trackingLogs
            .Where(t => t.StateCode == "stage-2" || t.StateCode == "stage-1")
            .Sum(t => t.CalculatedHours);

        DateTime? lastActivityDate = trackingLogs.Any()
            ? trackingLogs.Max(t => t.CreatedAt)
            : null;

        var volunteerHistory = VolunteerHistory.Rehydrate(
            profileCode: profile.UvaCode,
            firstName: profile.FirstName,
            lastName: profile.LastName,
            careerName: profile.CareerCode,
            personalGoalHours: profile.PersonalGoalHours,
            totalActivitiesParticipated: totalActivitiesParticipated,
            validatedHours: validatedHours,
            totalLoggedHours: totalLoggedHours,
            lastActivityDate: lastActivityDate);

        return volunteerHistory;
    }

    public async Task<HomeSummary?> GetLiveHomeSummaryByProfileCodeAsync(string profileCode, int year, int month)
    {
        var profile = await _context.Profiles
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.UvaCode == profileCode && p.DeletedAt == null);

        if (profile == null)
        {
            return null;
        }

        var activeScholarship = await _context.UserScholarships
            .AsNoTracking()
            .OrderByDescending(s => s.CreatedAt)
            .FirstOrDefaultAsync(s => s.AssignedProfileCode == profileCode && s.StateCode != "stage-4" && s.DeletedAt == null);

        var enrollments = await _context.Enrollments
            .AsNoTracking()
            .Where(e => e.EnrolledProfileCode == profileCode && e.DeletedAt == null)
            .Select(e => e.UvaCode)
            .ToListAsync();

        var trackingLogs = await _context.TrackingLogs
            .AsNoTracking()
            .Where(t => enrollments.Contains(t.EnrollmentCode) && t.DeletedAt == null && t.StateCode == "stage-2")
            .ToListAsync();

        decimal totalLoggedHours = trackingLogs.Sum(t => t.CalculatedHours);

        var currentMonthLogs = trackingLogs
            .Where(t => t.EntryTime.HasValue && t.EntryTime.Value.Year == year && t.EntryTime.Value.Month == month)
            .ToList();

        decimal monthLoggedHours = currentMonthLogs.Sum(t => t.CalculatedHours);

        var dailyActivities = currentMonthLogs
            .GroupBy(t => t.EntryTime!.Value.Day)
            .Select(g => DailyActivity.Create(g.Key, g.Sum(t => t.CalculatedHours)))
            .OrderBy(d => d.Day)
            .ToList();

        return HomeSummary.Create(
            personalGoalHours: profile.PersonalGoalHours,
            scholarshipGoalHours: activeScholarship?.RequiredHours ?? 0m,
            monthLoggedHours: monthLoggedHours,
            totalLoggedHours: totalLoggedHours,
            currentMonthDailyActivities: dailyActivities);
    }

    public async Task RefreshMaterializedViewsAsync()
    {
        await _context.Database.ExecuteSqlRawAsync(
            "REFRESH MATERIALIZED VIEW CONCURRENTLY public.mv_scholarship_performance");
        await _context.Database.ExecuteSqlRawAsync(
            "REFRESH MATERIALIZED VIEW CONCURRENTLY public.mv_program_analytics");
        await _context.Database.ExecuteSqlRawAsync(
            "REFRESH MATERIALIZED VIEW CONCURRENTLY public.mv_volunteer_history");
    }

    public async Task<List<Tuple<string, decimal>>> GetMonthlyAttendanceAsync(int months)
    {
        var startDate = DateTime.UtcNow.AddMonths(-months + 1);
        var startOfFirstMonth = new DateTime(startDate.Year, startDate.Month, 1, 0, 0, 0, DateTimeKind.Utc);

        var logs = await _context.TrackingLogs
            .AsNoTracking()
            .Where(t => t.StateCode == "stage-2" && t.DeletedAt == null && t.EntryTime >= startOfFirstMonth)
            .ToListAsync();

        var result = new List<Tuple<string, decimal>>();
        var culture = new System.Globalization.CultureInfo("es-ES");

        for (int i = months - 1; i >= 0; i--)
        {
            var targetDate = DateTime.UtcNow.AddMonths(-i);
            var monthName = culture.DateTimeFormat.GetAbbreviatedMonthName(targetDate.Month);
            var capitalizedMonth = char.ToUpper(monthName[0]) + monthName.Substring(1);

            var hours = logs
                .Where(t => t.EntryTime.HasValue && t.EntryTime.Value.Year == targetDate.Year && t.EntryTime.Value.Month == targetDate.Month)
                .Sum(t => t.CalculatedHours);

            result.Add(new Tuple<string, decimal>(capitalizedMonth, hours));
        }

        return result;
    }

    public async Task<long> GetTotalProfilesAsync()
    {
        return await _context.Profiles.CountAsync(p => p.DeletedAt == null);
    }
}
