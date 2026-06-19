namespace U_VoluntApp_Backend.Src.Infrastructure.Persistence.Repositories;

using Microsoft.EntityFrameworkCore;
using U_VoluntApp_Backend.Src.Domain.Entities.Tracking;
using U_VoluntApp_Backend.Src.Domain.Utils.Configuration;
using U_VoluntApp_Backend.Src.Infrastructure.Persistence.Interfaces.Tracking;
using U_VoluntApp_Backend.Src.Infrastructure.Persistence.Mappers;

public class EvidenceRepository : IEvidenceRepository
{
    private readonly AppDbContext _context;

    public EvidenceRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Evidence?> GetByCodeAsync(string uvaCode)
    {
        var evidence = await _context.Evidences
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.UvaCode == uvaCode);

        return evidence is null ? null : DomainPersistenceMapper.ToDomain(evidence);
    }

    public async Task<IEnumerable<Evidence>> GetByTrackingLogCodeAsync(string trackingLogCode, RequestFilter filter)
    {
        var evidences = await _context.Evidences
            .AsNoTracking()
            .Where(e => e.TrackingLogCode == trackingLogCode)
            .OrderByDescending(e => e.CreatedAt)
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync();

        return evidences.Select(DomainPersistenceMapper.ToDomain).ToList();
    }

    public async Task AddAsync(Evidence evidence)
    {
        var model = DomainPersistenceMapper.ToPersistence(evidence);
        await _context.Evidences.AddAsync(model);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Evidence evidence)
    {
        var existing = await _context.Evidences
            .FirstOrDefaultAsync(e => e.UvaCode == evidence.UvaCode)
            ?? throw new InvalidOperationException("Evidencia no encontrada para actualizar");

        existing.Observations = evidence.Observations;
        existing.UpdatedAt = evidence.UpdatedAt;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(string uvaCode)
    {
        var evidence = await _context.Evidences.FirstOrDefaultAsync(e => e.UvaCode == uvaCode);
        if (evidence != null)
        {
            _context.Evidences.Remove(evidence);
            await _context.SaveChangesAsync();
        }
    }
}
