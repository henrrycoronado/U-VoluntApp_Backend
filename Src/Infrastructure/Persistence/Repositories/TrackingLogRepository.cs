namespace U_VoluntApp_Backend.Src.Infrastructure.Persistence.Repositories;

using Microsoft.EntityFrameworkCore;
using U_VoluntApp_Backend.Src.Domain.Entities.Tracking;
using U_VoluntApp_Backend.Src.Domain.Utils.Configuration;
using U_VoluntApp_Backend.Src.Infrastructure.Persistence.Interfaces.Tracking;
using U_VoluntApp_Backend.Src.Infrastructure.Persistence.Mappers;

public class TrackingLogRepository : ITrackingLogRepository
{
    private readonly AppDbContext _context;

    public TrackingLogRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<TrackingLog?> GetByCodeAsync(string uvaCode)
    {
        var log = await _context.TrackingLogs
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.UvaCode == uvaCode);

        return log is null ? null : DomainPersistenceMapper.ToDomain(log);
    }

    public async Task<IEnumerable<TrackingLog>> GetByActivityCodeAsync(string activityCode, RequestFilter filter)
    {
        var query = _context.TrackingLogs
            .AsNoTracking()
            .Where(t => t.Enrollment.ActivityCode == activityCode);

        if (!string.IsNullOrWhiteSpace(filter.StateName))
        {
            query = query.Where(t => t.StateCode == filter.StateName);
        }

        var logs = await query
            .OrderByDescending(t => t.CreatedAt)
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync();

        return logs.Select(DomainPersistenceMapper.ToDomain).ToList();
    }

    public async Task<IEnumerable<TrackingLog>> GetByGroupActivityCodeAsync(string groupActivityCode, RequestFilter filter)
    {
        var query = _context.TrackingLogs
            .AsNoTracking()
            .Where(t => t.GroupEnrollment != null && t.GroupEnrollment.ActivityGroupCode == groupActivityCode);

        if (!string.IsNullOrWhiteSpace(filter.StateName))
        {
            query = query.Where(t => t.StateCode == filter.StateName);
        }

        var logs = await query
            .OrderByDescending(t => t.CreatedAt)
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync();

        return logs.Select(DomainPersistenceMapper.ToDomain).ToList();
    }

    public async Task<IEnumerable<TrackingLog>> GetByEnrollmentCodeAsync(string enrollmentCode, RequestFilter filter)
    {
        var query = _context.TrackingLogs
            .AsNoTracking()
            .Where(t => t.EnrollmentCode == enrollmentCode);

        if (!string.IsNullOrWhiteSpace(filter.StateName))
        {
            query = query.Where(t => t.StateCode == filter.StateName);
        }

        var logs = await query
            .OrderByDescending(t => t.CreatedAt)
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync();

        return logs.Select(DomainPersistenceMapper.ToDomain).ToList();
    }

    public async Task<IEnumerable<TrackingLog>> GetByGroupEnrollmentCodeAsync(string groupEnrollmentCode, RequestFilter filter)
    {
        var query = _context.TrackingLogs
            .AsNoTracking()
            .Where(t => t.GroupEnrollmentCode == groupEnrollmentCode);

        if (!string.IsNullOrWhiteSpace(filter.StateName))
        {
            query = query.Where(t => t.StateCode == filter.StateName);
        }

        var logs = await query
            .OrderByDescending(t => t.CreatedAt)
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync();

        return logs.Select(DomainPersistenceMapper.ToDomain).ToList();
    }

    public async Task<IEnumerable<TrackingLog>> GetByProfileCodeAsync(string profileCode, RequestFilter filter)
    {
        var query = _context.TrackingLogs
            .AsNoTracking()
            .Where(t => t.Enrollment.EnrolledProfileCode == profileCode);

        if (!string.IsNullOrWhiteSpace(filter.StateName))
        {
            query = query.Where(t => t.StateCode == filter.StateName);
        }

        var logs = await query
            .OrderByDescending(t => t.CreatedAt)
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync();

        return logs.Select(DomainPersistenceMapper.ToDomain).ToList();
    }

    public async Task AddAsync(TrackingLog log)
    {
        var model = DomainPersistenceMapper.ToPersistence(log);
        await _context.TrackingLogs.AddAsync(model);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(TrackingLog log)
    {
        var existing = await _context.TrackingLogs
            .FirstOrDefaultAsync(t => t.UvaCode == log.UvaCode)
            ?? throw new InvalidOperationException("Registro de seguimiento no encontrado para actualizar");

        existing.EntryTime = log.EntryTime;
        existing.ExitTime = log.ExitTime;
        existing.CalculatedHours = log.CalculatedHours;
        existing.StateCode = log.StateCode;
        existing.CheckInRegisteredByCode = log.CheckInRegisteredByCode;
        existing.CheckOutRegisteredByCode = log.CheckOutRegisteredByCode;
        existing.UpdatedAt = log.UpdatedAt;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(string uvaCode)
    {
        var log = await _context.TrackingLogs.FirstOrDefaultAsync(t => t.UvaCode == uvaCode);
        if (log != null)
        {
            _context.TrackingLogs.Remove(log);
            await _context.SaveChangesAsync();
        }
    }
}
