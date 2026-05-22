namespace U_VoluntApp_Backend.Src.Infrastructure.Persistence.Repositories;

using Microsoft.EntityFrameworkCore;
using U_VoluntApp_Backend.Src.Domain.Entities.Profile;
using U_VoluntApp_Backend.Src.Infrastructure.Persistence.Interfaces.Profile;
using U_VoluntApp_Backend.Src.Infrastructure.Persistence.Mappers;

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

    public async Task RefreshMaterializedViewsAsync()
    {
        await _context.Database.ExecuteSqlRawAsync(
            "REFRESH MATERIALIZED VIEW CONCURRENTLY public.mv_scholarship_performance");
        await _context.Database.ExecuteSqlRawAsync(
            "REFRESH MATERIALIZED VIEW CONCURRENTLY public.mv_program_analytics");
        await _context.Database.ExecuteSqlRawAsync(
            "REFRESH MATERIALIZED VIEW CONCURRENTLY public.mv_activity_analytics");
        await _context.Database.ExecuteSqlRawAsync(
            "REFRESH MATERIALIZED VIEW CONCURRENTLY public.mv_volunteer_history");
    }
}
