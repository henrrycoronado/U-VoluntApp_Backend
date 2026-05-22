namespace U_VoluntApp_Backend.Src.Infrastructure.Persistence.Repositories;

using Microsoft.EntityFrameworkCore;
using U_VoluntApp_Backend.Src.Domain.Entities.Contract;
using U_VoluntApp_Backend.Src.Domain.Utils.Configuration;
using U_VoluntApp_Backend.Src.Infrastructure.Persistence.Interfaces.Contract;
using U_VoluntApp_Backend.Src.Infrastructure.Persistence.Mappers;

public class UserScholarshipRepository : IUserScholarshipRepository
{
    private readonly AppDbContext _context;

    public UserScholarshipRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<UserScholarship?> GetByCodeAsync(string uvaCode)
    {
        var scholarship = await _context.UserScholarships
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.UvaCode == uvaCode);

        return scholarship is null ? null : DomainPersistenceMapper.ToDomain(scholarship);
    }

    public async Task<IEnumerable<UserScholarship>> GetAllAsync(RequestFilter filter)
    {
        var query = _context.UserScholarships.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(filter.StateName))
        {
            query = query.Where(s => s.StateCode == filter.StateName);
        }

        var scholarships = await query
            .OrderByDescending(s => s.CreatedAt)
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync();

        return scholarships.Select(DomainPersistenceMapper.ToDomain).ToList();
    }

    public async Task<IEnumerable<UserScholarship>> GetByProfileCodeAsync(string profileCode, RequestFilter filter)
    {
        var query = _context.UserScholarships
            .AsNoTracking()
            .Where(s => s.AssignedProfileCode == profileCode);

        if (!string.IsNullOrWhiteSpace(filter.StateName))
        {
            query = query.Where(s => s.StateCode == filter.StateName);
        }

        var scholarships = await query
            .OrderByDescending(s => s.CreatedAt)
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync();

        return scholarships.Select(DomainPersistenceMapper.ToDomain).ToList();
    }

    public async Task<IEnumerable<UserScholarship>> GetByCareerCodeAsync(string careerCode, RequestFilter filter)
    {
        var query = _context.UserScholarships
            .AsNoTracking()
            .Where(s => s.AssignedProfile.CareerCode == careerCode);

        if (!string.IsNullOrWhiteSpace(filter.StateName))
        {
            query = query.Where(s => s.StateCode == filter.StateName);
        }

        var scholarships = await query
            .OrderByDescending(s => s.CreatedAt)
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync();

        return scholarships.Select(DomainPersistenceMapper.ToDomain).ToList();
    }

    public async Task<IEnumerable<UserScholarship>> GetByTypeCodeAsync(string typeCode, RequestFilter filter)
    {
        var query = _context.UserScholarships
            .AsNoTracking()
            .Where(s => s.ScholarshipTypeCode == typeCode);

        if (!string.IsNullOrWhiteSpace(filter.StateName))
        {
            query = query.Where(s => s.StateCode == filter.StateName);
        }

        var scholarships = await query
            .OrderByDescending(s => s.CreatedAt)
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync();

        return scholarships.Select(DomainPersistenceMapper.ToDomain).ToList();
    }

    public async Task<IEnumerable<UserScholarship>> GetByRequiredHoursAsync(int requiredHours, RequestFilter filter)
    {
        var query = _context.UserScholarships
            .AsNoTracking()
            .Where(s => s.RequiredHours == requiredHours);

        if (!string.IsNullOrWhiteSpace(filter.StateName))
        {
            query = query.Where(s => s.StateCode == filter.StateName);
        }

        var scholarships = await query
            .OrderByDescending(s => s.CreatedAt)
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync();

        return scholarships.Select(DomainPersistenceMapper.ToDomain).ToList();
    }

    public async Task AddAsync(UserScholarship scholarship)
    {
        var model = DomainPersistenceMapper.ToPersistence(scholarship);
        await _context.UserScholarships.AddAsync(model);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(UserScholarship scholarship)
    {
        var existing = await _context.UserScholarships
            .FirstOrDefaultAsync(s => s.UvaCode == scholarship.UvaCode)
            ?? throw new InvalidOperationException("Beca no encontrada para actualizar");

        existing.EvaluatorProfileCode = scholarship.EvaluatorProfileCode;
        existing.Reason = scholarship.Reason;
        existing.RequiredHours = scholarship.RequiredHours;
        existing.StartDate = scholarship.StartDate;
        existing.EndDate = scholarship.EndDate;
        existing.StateCode = scholarship.StateCode;
        existing.UpdatedAt = scholarship.UpdatedAt;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(string uvaCode)
    {
        var scholarship = await _context.UserScholarships.FirstOrDefaultAsync(s => s.UvaCode == uvaCode);
        if (scholarship != null)
        {
            _context.UserScholarships.Remove(scholarship);
            await _context.SaveChangesAsync();
        }
    }
}
