namespace U_VoluntApp_Core.Src.Infrastructure.Persistence.Repositories;

using Microsoft.EntityFrameworkCore;
using U_VoluntApp_Core.Src.Domain.Entities.Contract;
using U_VoluntApp_Core.Src.Domain.Utils.Configuration;
using U_VoluntApp_Core.Src.Infrastructure.Persistence.Interfaces.Contract;
using U_VoluntApp_Core.Src.Infrastructure.Persistence.Mappers;

public class RoleRequestRepository : IRoleRequestRepository
{
    private readonly AppDbContext _context;

    public RoleRequestRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<RoleRequest?> GetByCodeAsync(string uvaCode)
    {
        var roleRequest = await _context.RoleRequests
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.UvaCode == uvaCode);

        return roleRequest is null ? null : DomainPersistenceMapper.ToDomain(roleRequest);
    }

    public async Task<IEnumerable<RoleRequest>> GetAllAsync(RequestFilter filter)
    {
        var query = _context.RoleRequests
            .AsNoTracking();

        if (!string.IsNullOrWhiteSpace(filter.StateName))
        {
            query = query.Where(r => r.StateCode == filter.StateName);
        }

        var roleRequests = await query
            .OrderByDescending(r => r.CreatedAt)
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync();

        return roleRequests.Select(DomainPersistenceMapper.ToDomain).ToList();
    }

    public async Task<IEnumerable<RoleRequest>> GetAllByRoleCodeAsync(string roleCode, RequestFilter filter)
    {
        var query = _context.RoleRequests
            .AsNoTracking()
            .Where(r => r.RequestedRoleId == roleCode);

        if (!string.IsNullOrWhiteSpace(filter.StateName))
        {
            query = query.Where(r => r.StateCode == filter.StateName);
        }

        var roleRequests = await query
            .OrderByDescending(r => r.CreatedAt)
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync();

        return roleRequests.Select(DomainPersistenceMapper.ToDomain).ToList();
    }

    public async Task<IEnumerable<RoleRequest>> GetByProfileCodeAsync(string profileCode, RequestFilter filter)
    {
        var query = _context.RoleRequests
            .AsNoTracking()
            .Where(r => r.RequesterProfileCode == profileCode);

        if (!string.IsNullOrWhiteSpace(filter.StateName))
        {
            query = query.Where(r => r.StateCode == filter.StateName);
        }

        var roleRequests = await query
            .OrderByDescending(r => r.CreatedAt)
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync();

        return roleRequests.Select(DomainPersistenceMapper.ToDomain).ToList();
    }

    public async Task AddAsync(RoleRequest roleRequest)
    {
        var model = DomainPersistenceMapper.ToPersistence(roleRequest);
        await _context.RoleRequests.AddAsync(model);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(RoleRequest roleRequest)
    {
        var existing = await _context.RoleRequests
            .FirstOrDefaultAsync(r => r.UvaCode == roleRequest.UvaCode)
            ?? throw new InvalidOperationException("Solicitud de rol no encontrada para actualizar");

        existing.RequestedRoleId = roleRequest.RequestedRoleCode;
        existing.Reason = roleRequest.Reason;
        existing.DurationInMonths = roleRequest.DurationInMonths;
        existing.StateCode = roleRequest.StateCode;
        existing.ResolvedByProfileCode = roleRequest.ResolvedByProfileCode;
        existing.ResolvedAt = roleRequest.ResolvedAt;
        existing.UpdatedAt = roleRequest.UpdatedAt;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(string uvaCode)
    {
        var roleRequest = await _context.RoleRequests.FirstOrDefaultAsync(r => r.UvaCode == uvaCode);
        if (roleRequest != null)
        {
            _context.RoleRequests.Remove(roleRequest);
            await _context.SaveChangesAsync();
        }
    }
}
