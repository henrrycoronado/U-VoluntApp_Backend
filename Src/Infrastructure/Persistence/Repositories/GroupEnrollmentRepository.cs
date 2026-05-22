namespace U_VoluntApp_Backend.Src.Infrastructure.Persistence.Repositories;

using Microsoft.EntityFrameworkCore;
using U_VoluntApp_Backend.Src.Domain.Entities.Enrollment;
using U_VoluntApp_Backend.Src.Domain.Utils.Configuration;
using U_VoluntApp_Backend.Src.Infrastructure.Persistence.Interfaces.Enrollment;
using U_VoluntApp_Backend.Src.Infrastructure.Persistence.Mappers;

public class GroupEnrollmentRepository : IGroupEnrollmentRepository
{
    private readonly AppDbContext _context;

    public GroupEnrollmentRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<GroupEnrollment?> GetByCodeAsync(string uvaCode)
    {
        var groupEnrollment = await _context.GroupEnrollments
            .AsNoTracking()
            .FirstOrDefaultAsync(ge => ge.UvaCode == uvaCode);

        return groupEnrollment is null ? null : DomainPersistenceMapper.ToDomain(groupEnrollment);
    }

    public async Task<IEnumerable<GroupEnrollment>> GetByActivityGroupCodeAsync(string activityGroupCode, RequestFilter filter)
    {
        var query = _context.GroupEnrollments
            .AsNoTracking()
            .Where(ge => ge.ActivityGroupCode == activityGroupCode);

        if (!string.IsNullOrWhiteSpace(filter.StateName))
        {
            query = query.Where(ge => ge.StateCode == filter.StateName);
        }

        var groupEnrollments = await query
            .OrderByDescending(ge => ge.CreatedAt)
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync();

        return groupEnrollments.Select(DomainPersistenceMapper.ToDomain).ToList();
    }

    public async Task<IEnumerable<GroupEnrollment>> GetByGroupActivityCodeAsync(string groupActivityCode, RequestFilter filter)
    {
        // Assuming groupActivityCode is same as activityGroupCode based on interface naming vs model naming
        return await GetByActivityGroupCodeAsync(groupActivityCode, filter);
    }

    public async Task<IEnumerable<GroupEnrollment>> GetByEnrollmentCodeAsync(string enrollmentCode, RequestFilter filter)
    {
        var query = _context.GroupEnrollments
            .AsNoTracking()
            .Where(ge => ge.EnrollmentCode == enrollmentCode);

        if (!string.IsNullOrWhiteSpace(filter.StateName))
        {
            query = query.Where(ge => ge.StateCode == filter.StateName);
        }

        var groupEnrollments = await query
            .OrderByDescending(ge => ge.CreatedAt)
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync();

        return groupEnrollments.Select(DomainPersistenceMapper.ToDomain).ToList();
    }

    public async Task<IEnumerable<GroupEnrollment>> GetByProfileCodeAsync(string profileCode, RequestFilter filter)
    {
        var query = _context.GroupEnrollments
            .AsNoTracking()
            .Include(ge => ge.Enrollment)
            .Where(ge => ge.Enrollment != null && ge.Enrollment.EnrolledProfileCode == profileCode);

        if (!string.IsNullOrWhiteSpace(filter.StateName))
        {
            query = query.Where(ge => ge.StateCode == filter.StateName);
        }

        var groupEnrollments = await query
            .OrderByDescending(ge => ge.CreatedAt)
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync();

        return groupEnrollments.Select(DomainPersistenceMapper.ToDomain).ToList();
    }

    public async Task AddAsync(GroupEnrollment groupEnrollment)
    {
        var model = DomainPersistenceMapper.ToPersistence(groupEnrollment);
        await _context.GroupEnrollments.AddAsync(model);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(GroupEnrollment groupEnrollment)
    {
        var existing = await _context.GroupEnrollments
            .FirstOrDefaultAsync(ge => ge.UvaCode == groupEnrollment.UvaCode)
            ?? throw new InvalidOperationException("Inscripción de grupo no encontrada para actualizar");

        existing.StateCode = groupEnrollment.StateCode;
        existing.UpdatedAt = groupEnrollment.UpdatedAt;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(string uvaCode)
    {
        var groupEnrollment = await _context.GroupEnrollments.FirstOrDefaultAsync(ge => ge.UvaCode == uvaCode);
        if (groupEnrollment != null)
        {
            _context.GroupEnrollments.Remove(groupEnrollment);
            await _context.SaveChangesAsync();
        }
    }
}
