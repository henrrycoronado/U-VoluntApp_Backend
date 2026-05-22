namespace U_VoluntApp_Backend.Src.Infrastructure.Persistence.Repositories;

using Microsoft.EntityFrameworkCore;
using U_VoluntApp_Backend.Src.Domain.Entities.Enrollment;
using U_VoluntApp_Backend.Src.Domain.Utils.Configuration;
using U_VoluntApp_Backend.Src.Infrastructure.Persistence.Interfaces.Enrollment;
using U_VoluntApp_Backend.Src.Infrastructure.Persistence.Mappers;

public class EnrollmentRepository : IEnrollmentRepository
{
    private readonly AppDbContext _context;

    public EnrollmentRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Enrollment?> GetByCodeAsync(string uvaCode)
    {
        var enrollment = await _context.Enrollments
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.UvaCode == uvaCode);

        return enrollment is null ? null : DomainPersistenceMapper.ToDomain(enrollment);
    }

    public async Task<IEnumerable<Enrollment>> GetByActivityCodeAsync(string activityCode, RequestFilter filter)
    {
        var query = _context.Enrollments
            .AsNoTracking()
            .Where(e => e.ActivityCode == activityCode);

        if (!string.IsNullOrWhiteSpace(filter.StateName))
        {
            query = query.Where(e => e.StateCode == filter.StateName);
        }

        var enrollments = await query
            .OrderByDescending(e => e.CreatedAt)
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync();

        return enrollments.Select(DomainPersistenceMapper.ToDomain).ToList();
    }

    public async Task<IEnumerable<Enrollment>> GetByProfileCodeAsync(string profileCode, RequestFilter filter)
    {
        var query = _context.Enrollments
            .AsNoTracking()
            .Where(e => e.EnrolledProfileCode == profileCode);

        if (!string.IsNullOrWhiteSpace(filter.StateName))
        {
            query = query.Where(e => e.StateCode == filter.StateName);
        }

        var enrollments = await query
            .OrderByDescending(e => e.CreatedAt)
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync();

        return enrollments.Select(DomainPersistenceMapper.ToDomain).ToList();
    }

    public async Task AddAsync(Enrollment enrollment)
    {
        var model = DomainPersistenceMapper.ToPersistence(enrollment);
        await _context.Enrollments.AddAsync(model);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Enrollment enrollment)
    {
        var model = DomainPersistenceMapper.ToPersistence(enrollment);
        _context.Enrollments.Update(model);
        await _context.SaveChangesAsync();
    }
}
