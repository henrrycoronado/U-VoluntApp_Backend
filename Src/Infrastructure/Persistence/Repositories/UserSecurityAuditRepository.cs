namespace U_VoluntApp_Core.Src.Infrastructure.Persistence.Repositories;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using U_VoluntApp_Core.Src.Domain.Entities.Auth;
using U_VoluntApp_Core.Src.Infrastructure.Persistence;
using U_VoluntApp_Core.Src.Infrastructure.Persistence.Interfaces.Auth;
using U_VoluntApp_Core.Src.Infrastructure.Persistence.Mappers;

public class UserSecurityAuditRepository : IUserSecurityAuditRepository
{
    private readonly AppDbContext _context;

    public UserSecurityAuditRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<UserSecurityAudit?> GetByProfileAndFingerprintAsync(string profileCode, string fingerprint)
    {
        var audit = await _context.UserSecurityAudits
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.ProfileCode == profileCode && a.DeviceFingerprint == fingerprint);

        return audit is null ? null : DomainPersistenceMapper.ToDomain(audit);
    }

    public async Task<UserSecurityAudit?> GetByCodeAsync(string profileCode, string deviceCode)
    {
        var audit = await _context.UserSecurityAudits
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.ProfileCode == profileCode && a.UvaCode == deviceCode);

        return audit is null ? null : DomainPersistenceMapper.ToDomain(audit);
    }

    public async Task<List<UserSecurityAudit>> GetByProfileCodeAsync(string profileCode)
    {
        var audits = await _context.UserSecurityAudits
            .AsNoTracking()
            .Where(a => a.ProfileCode == profileCode)
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync();

        return audits.Select(DomainPersistenceMapper.ToDomain).ToList();
    }

    public async Task AddAsync(UserSecurityAudit audit)
    {
        var model = DomainPersistenceMapper.ToPersistence(audit);
        await _context.UserSecurityAudits.AddAsync(model);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(UserSecurityAudit audit)
    {
        var existing = await _context.UserSecurityAudits
            .FirstOrDefaultAsync(a => a.ProfileCode == audit.ProfileCode && a.DeviceFingerprint == audit.DeviceFingerprint)
            ?? throw new InvalidOperationException("Registro de auditoría no encontrado para actualizar");

        existing.LastIpAddress = audit.LastIpAddress;
        existing.LastCodeSentAt = audit.LastCodeSentAt;
        existing.IsTrusted = audit.IsTrusted;
        existing.UpdatedAt = audit.UpdatedAt;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(string profileCode, string deviceCode)
    {
        var audit = await _context.UserSecurityAudits
            .FirstOrDefaultAsync(a => a.ProfileCode == profileCode && a.UvaCode == deviceCode);

        if (audit != null)
        {
            _context.UserSecurityAudits.Remove(audit);
            await _context.SaveChangesAsync();
        }
    }
}
