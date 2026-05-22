namespace U_VoluntApp_Backend.Src.Application.Services;

using Microsoft.EntityFrameworkCore;
using U_VoluntApp_Backend.Src.Application.DTOs;
using U_VoluntApp_Backend.Src.Application.Interfaces;
using U_VoluntApp_Backend.Src.Infrastructure.Persistence;
using U_VoluntApp_Backend.Src.Infrastructure.Persistence.Models.States;
using U_VoluntApp_Backend.Src.Infrastructure.Persistence.Models.Types;

public class ReferenceCatalogService : IReferenceCatalogService
{
    private readonly AppDbContext _context;

    public ReferenceCatalogService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<ReferenceStateDto>> GetStatesAsync(string stateGroup)
    {
        stateGroup = NormalizeGroup(stateGroup);

        return stateGroup switch
        {
            "activity" => await _context.ActivityStates.AsNoTracking()
                .OrderBy(x => x.Name)
                .Select(x => new ReferenceStateDto { UvaCode = x.UvaCode, Name = x.Name })
                .ToListAsync(),
            "program" => await _context.ProgramStates.AsNoTracking()
                .OrderBy(x => x.Name)
                .Select(x => new ReferenceStateDto { UvaCode = x.UvaCode, Name = x.Name })
                .ToListAsync(),
            "profile" => await _context.ProfileStates.AsNoTracking()
                .OrderBy(x => x.Name)
                .Select(x => new ReferenceStateDto { UvaCode = x.UvaCode, Name = x.Name })
                .ToListAsync(),
            "enrollment" => await _context.EnrollmentStates.AsNoTracking()
                .OrderBy(x => x.Name)
                .Select(x => new ReferenceStateDto { UvaCode = x.UvaCode, Name = x.Name })
                .ToListAsync(),
            "tracking" => await _context.TrackingStates.AsNoTracking()
                .OrderBy(x => x.Name)
                .Select(x => new ReferenceStateDto { UvaCode = x.UvaCode, Name = x.Name })
                .ToListAsync(),
            "contract" => await _context.ContractStates.AsNoTracking()
                .OrderBy(x => x.Name)
                .Select(x => new ReferenceStateDto { UvaCode = x.UvaCode, Name = x.Name })
                .ToListAsync(),
            "role-request" => await _context.RoleRequestStates.AsNoTracking()
                .OrderBy(x => x.Name)
                .Select(x => new ReferenceStateDto { UvaCode = x.UvaCode, Name = x.Name })
                .ToListAsync(),
            _ => throw new InvalidOperationException("Grupo de states no valido"),
        };
    }

    public async Task<ReferenceStateDto> UpdateStateNameAsync(string stateGroup, string stateCode, string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
        {
            throw new InvalidOperationException("El nombre del state es obligatorio");
        }

        stateGroup = NormalizeGroup(stateGroup);

        return stateGroup switch
        {
            "activity" => await UpdateStateNameAsync(_context.ActivityStates, stateCode, newName),
            "program" => await UpdateStateNameAsync(_context.ProgramStates, stateCode, newName),
            "profile" => await UpdateStateNameAsync(_context.ProfileStates, stateCode, newName),
            "enrollment" => await UpdateStateNameAsync(_context.EnrollmentStates, stateCode, newName),
            "tracking" => await UpdateStateNameAsync(_context.TrackingStates, stateCode, newName),
            "contract" => await UpdateStateNameAsync(_context.ContractStates, stateCode, newName),
            "role-request" => await UpdateStateNameAsync(_context.RoleRequestStates, stateCode, newName),
            _ => throw new InvalidOperationException("Grupo de states no valido"),
        };
    }

    public async Task<List<ReferenceTypeDto>> GetTypesAsync(string typeGroup)
    {
        typeGroup = NormalizeGroup(typeGroup);

        return typeGroup switch
        {
            "activity" => await _context.ActivityTypes.AsNoTracking()
                .OrderBy(x => x.Name)
                .Select(x => new ReferenceTypeDto { UvaCode = x.UvaCode, Name = x.Name, IsActive = x.IsActive })
                .ToListAsync(),
            "evidence" => await _context.EvidenceTypes.AsNoTracking()
                .OrderBy(x => x.Name)
                .Select(x => new ReferenceTypeDto { UvaCode = x.UvaCode, Name = x.Name, IsActive = x.IsActive })
                .ToListAsync(),
            "tracking" => await _context.TrackingTypes.AsNoTracking()
                .OrderBy(x => x.Name)
                .Select(x => new ReferenceTypeDto { UvaCode = x.UvaCode, Name = x.Name, IsActive = x.IsActive })
                .ToListAsync(),
            "career" => await _context.CareerTypes.AsNoTracking()
                .OrderBy(x => x.Name)
                .Select(x => new ReferenceTypeDto { UvaCode = x.UvaCode, Name = x.Name, IsActive = x.IsActive })
                .ToListAsync(),
            "scholarship" => await _context.ScholarshipTypes.AsNoTracking()
                .OrderBy(x => x.Name)
                .Select(x => new ReferenceTypeDto { UvaCode = x.UvaCode, Name = x.Name, IsActive = x.IsActive })
                .ToListAsync(),
            _ => throw new InvalidOperationException("Grupo de types no valido"),
        };
    }

    public async Task<ReferenceTypeDto> CreateTypeAsync(string typeGroup, CreateReferenceTypeDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
        {
            throw new InvalidOperationException("El nombre del type es obligatorio");
        }

        typeGroup = NormalizeGroup(typeGroup);

        return typeGroup switch
        {
            "activity" => await CreateTypeAsync(_context.ActivityTypes, x => new ActivityType { UvaCode = x, Name = dto.Name.Trim(), IsActive = dto.IsActive }),
            "evidence" => await CreateTypeAsync(_context.EvidenceTypes, x => new EvidenceType { UvaCode = x, Name = dto.Name.Trim(), IsActive = dto.IsActive }),
            "tracking" => await CreateTypeAsync(_context.TrackingTypes, x => new TrackingType { UvaCode = x, Name = dto.Name.Trim(), IsActive = dto.IsActive }),
            "career" => await CreateTypeAsync(_context.CareerTypes, x => new CareerType { UvaCode = x, Name = dto.Name.Trim(), IsActive = dto.IsActive }),
            "scholarship" => await CreateTypeAsync(_context.ScholarshipTypes, x => new ScholarshipType { UvaCode = x, Name = dto.Name.Trim(), IsActive = dto.IsActive }),
            _ => throw new InvalidOperationException("Grupo de types no valido"),
        };
    }

    public async Task<ReferenceTypeDto> UpdateTypeAsync(string typeGroup, string typeCode, UpdateReferenceTypeDto dto)
    {
        typeGroup = NormalizeGroup(typeGroup);

        if (dto.Name is null && dto.IsActive is null)
        {
            throw new InvalidOperationException("Debes enviar al menos un campo a actualizar");
        }

        return typeGroup switch
        {
            "activity" => await UpdateTypeAsync(_context.ActivityTypes, typeCode, dto),
            "evidence" => await UpdateTypeAsync(_context.EvidenceTypes, typeCode, dto),
            "tracking" => await UpdateTypeAsync(_context.TrackingTypes, typeCode, dto),
            "career" => await UpdateTypeAsync(_context.CareerTypes, typeCode, dto),
            "scholarship" => await UpdateTypeAsync(_context.ScholarshipTypes, typeCode, dto),
            _ => throw new InvalidOperationException("Grupo de types no valido"),
        };
    }

    private string NormalizeGroup(string group)
    {
        return group.Trim().ToLowerInvariant();
    }

    private async Task<ReferenceStateDto> UpdateStateNameAsync<TState>(DbSet<TState> set, string code, string newName)
        where TState : class
    {
        dynamic? entity = await set.FirstOrDefaultAsync(x => EF.Property<string>(x, "UvaCode") == code);
        if (entity is null)
        {
            throw new InvalidOperationException("State no encontrado");
        }

        entity.Name = newName.Trim();
        await _context.SaveChangesAsync();

        return new ReferenceStateDto
        {
            UvaCode = entity.UvaCode,
            Name = entity.Name,
        };
    }

    private async Task<ReferenceTypeDto> CreateTypeAsync<TType>(DbSet<TType> set, Func<string, TType> factory)
        where TType : class
    {
        var nextCode = await GetNextTypeCodeAsync(set);
        var entity = factory(nextCode);

        await set.AddAsync(entity);
        await _context.SaveChangesAsync();

        dynamic created = entity;
        return new ReferenceTypeDto
        {
            UvaCode = created.UvaCode,
            Name = created.Name,
            IsActive = created.IsActive,
        };
    }

    private async Task<ReferenceTypeDto> UpdateTypeAsync<TType>(DbSet<TType> set, string code, UpdateReferenceTypeDto dto)
        where TType : class
    {
        dynamic? entity = await set.FirstOrDefaultAsync(x => EF.Property<string>(x, "UvaCode") == code);
        if (entity is null)
        {
            throw new InvalidOperationException("Type no encontrado");
        }

        if (!string.IsNullOrWhiteSpace(dto.Name))
        {
            entity.Name = dto.Name.Trim();
        }

        if (dto.IsActive.HasValue)
        {
            entity.IsActive = dto.IsActive.Value;
        }

        await _context.SaveChangesAsync();

        return new ReferenceTypeDto
        {
            UvaCode = entity.UvaCode,
            Name = entity.Name,
            IsActive = entity.IsActive,
        };
    }

    private async Task<string> GetNextTypeCodeAsync<TType>(DbSet<TType> set)
        where TType : class
    {
        var existingCodes = await set.AsNoTracking()
            .Select(x => EF.Property<string>(x, "UvaCode"))
            .ToListAsync();

        var max = 0;
        foreach (var code in existingCodes)
        {
            if (!code.StartsWith("type-", StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            var suffix = code[5..];
            if (int.TryParse(suffix, out var value) && value > max)
            {
                max = value;
            }
        }

        return $"type-{max + 1}";
    }
}
