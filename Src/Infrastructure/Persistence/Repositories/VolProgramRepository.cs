namespace U_VoluntApp_Core.Src.Infrastructure.Persistence.Repositories;

using Microsoft.EntityFrameworkCore;
using U_VoluntApp_Core.Src.Domain.Entities.VolProgram;
using U_VoluntApp_Core.Src.Domain.Utils.Configuration;
using U_VoluntApp_Core.Src.Infrastructure.Persistence.Interfaces.VolProgram;
using U_VoluntApp_Core.Src.Infrastructure.Persistence.Mappers;

public class VolProgramRepository : IVolProgramRepository
{
    private readonly AppDbContext _context;

    public VolProgramRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<VolProgram?> GetByCodeAsync(string uvaCode)
    {
        var program = await _context.VolPrograms
            .AsNoTracking()
            .Include(p => p.VolProgramContent)
            .FirstOrDefaultAsync(p => p.UvaCode == uvaCode);

        return program is null ? null : DomainPersistenceMapper.ToDomain(program);
    }

    public async Task<IEnumerable<VolProgram>> GetAllAsync(RequestFilter filter)
    {
        var query = _context.VolPrograms.AsNoTracking().Include(p => p.VolProgramContent).AsQueryable();

        if (!string.IsNullOrWhiteSpace(filter.StateName))
        {
            query = query.Where(p => p.StateCode == filter.StateName);
        }

        var programs = await query
            .OrderBy(p => p.Name)
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync();

        return programs.Select(DomainPersistenceMapper.ToDomain).ToList();
    }

    public async Task<IEnumerable<VolProgram>> GetByManagerCodeAsync(string profileCode, RequestFilter filter)
    {
        var query = _context.VolPrograms
            .AsNoTracking()
            .Include(p => p.VolProgramContent)
            .AsQueryable()
            .Where(p => p.ManagerProfileCode == profileCode);

        if (!string.IsNullOrWhiteSpace(filter.StateName))
        {
            query = query.Where(p => p.StateCode == filter.StateName);
        }

        var programs = await query
            .OrderBy(p => p.Name)
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync();

        return programs.Select(DomainPersistenceMapper.ToDomain).ToList();
    }

    public async Task AddAsync(VolProgram program)
    {
        var model = DomainPersistenceMapper.ToPersistence(program);
        await _context.VolPrograms.AddAsync(model);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(VolProgram program)
    {
        var existing = await _context.VolPrograms
            .FirstOrDefaultAsync(p => p.UvaCode == program.UvaCode)
            ?? throw new InvalidOperationException("Programa no encontrado para actualizar");

        existing.Name = program.Name;
        existing.Acronym = program.Acronym;
        existing.StateCode = program.StateCode;
        existing.UpdatedAt = program.UpdatedAt;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(string uvaCode)
    {
        var program = await _context.VolPrograms.FirstOrDefaultAsync(p => p.UvaCode == uvaCode);
        if (program != null)
        {
            _context.VolPrograms.Remove(program);
            await _context.SaveChangesAsync();
        }
    }
}
