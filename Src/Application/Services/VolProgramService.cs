namespace U_VoluntApp_Backend.Src.Application.Services;

using U_VoluntApp_Backend.Src.Application.DTOs;
using U_VoluntApp_Backend.Src.Application.Interfaces;
using U_VoluntApp_Backend.Src.Domain.Entities.Profile;
using U_VoluntApp_Backend.Src.Domain.Entities.VolProgram;
using U_VoluntApp_Backend.Src.Domain.Utils.Configuration;
using U_VoluntApp_Backend.Src.Domain.Utils.Constants;
using U_VoluntApp_Backend.Src.Infrastructure.Persistence.Interfaces.Profile;
using U_VoluntApp_Backend.Src.Infrastructure.Persistence.Interfaces.VolProgram;

public class VolProgramService : IVolProgramService
{
    private readonly IVolProgramRepository _volProgramRepository;
    private readonly IProfileRepository _profileRepository;

    public VolProgramService(
        IVolProgramRepository volProgramRepository,
        IProfileRepository profileRepository)
    {
        _volProgramRepository = volProgramRepository;
        _profileRepository = profileRepository;
    }

    public async Task<VolProgramResponseDto> CreateAsync(CreateVolProgramDto dto, string managerId, string requesterRole = "Admin")
    {
        var manager = await _profileRepository.GetByCodeAsync(managerId)
            ?? throw new InvalidOperationException("Perfil no encontrado");

        string initialStateCode = requesterRole == RoleConstants.AdminRole ? ProgramStateConstants.InactiveCode : ProgramStateConstants.ActiveCode;

        var program = VolProgram.Create(dto.Name, dto.Acronym ?? string.Empty, managerId, initialStateCode, DateTime.UtcNow);

        await _volProgramRepository.AddAsync(program);
        return MapToResponse(program, manager);
    }

    public async Task<VolProgramResponseDto> GetByCodeAsync(string uvaCode, string requesterId, string requesterRole)
    {
        var program = await _volProgramRepository.GetByCodeAsync(uvaCode)
            ?? throw new InvalidOperationException("Programa no encontrado");

        if (requesterRole != RoleConstants.AdminRole && program.ManagerProfileCode != requesterId)
        {
            throw new InvalidOperationException("No tienes acceso a este programa");
        }

        var manager = await _profileRepository.GetByCodeAsync(program.ManagerProfileCode!)
            ?? throw new InvalidOperationException("Manager no encontrado");

        return MapToResponse(program, manager);
    }

    public async Task<List<VolProgramResponseDto>> GetAllAsync(string requesterId, string requesterRole)
    {
        var filter = new RequestFilter { Page = 1, PageSize = 100 }; // Default filter
        var programs = requesterRole == RoleConstants.AdminRole || requesterRole == RoleConstants.CoordinatorRole || requesterRole == RoleConstants.VolunteerRole
            ? await _volProgramRepository.GetAllAsync(filter)
            : await _volProgramRepository.GetByManagerCodeAsync(requesterId, filter);

        var result = new List<VolProgramResponseDto>();

        foreach (var program in programs)
        {
            var manager = await _profileRepository.GetByCodeAsync(program.ManagerProfileCode!);
            result.Add(MapToResponse(program, manager));
        }

        return result;
    }

    public async Task<VolProgramResponseDto> UpdateAsync(string uvaCode, UpdateVolProgramDto dto, string requesterId, string requesterRole)
    {
        var program = await _volProgramRepository.GetByCodeAsync(uvaCode)
            ?? throw new InvalidOperationException("Programa no encontrado");

        if (requesterRole != RoleConstants.AdminRole && program.ManagerProfileCode != requesterId)
        {
            throw new InvalidOperationException("No tienes permiso para modificar este programa");
        }

        if (program.StateCode == ProgramStateConstants.DeletedCode)
        {
            throw new InvalidOperationException("No se puede modificar un programa eliminado");
        }

        program.ApplyUpdate(dto.Name ?? program.Name, dto.Acronym, DateTime.UtcNow);

        await _volProgramRepository.UpdateAsync(program);

        var manager = await _profileRepository.GetByCodeAsync(program.ManagerProfileCode!)
            ?? throw new InvalidOperationException("Manager no encontrado");

        return MapToResponse(program, manager);
    }

    public async Task ChangeStateAsync(string uvaCode, ChangeVolProgramStateDto dto, string requesterId, string requesterRole)
    {
        if (requesterRole != RoleConstants.SuperUserRole)
        {
            throw new InvalidOperationException("Solo SuperUser puede cambiar el estado de un programa");
        }

        var program = await _volProgramRepository.GetByCodeAsync(uvaCode)
            ?? throw new InvalidOperationException("Programa no encontrado");

        if (program.StateCode == ProgramStateConstants.DeletedCode)
        {
            throw new InvalidOperationException("Un programa eliminado no puede cambiar de estado");
        }

        program.ChangeState(dto.StateCode, DateTime.UtcNow);

        await _volProgramRepository.UpdateAsync(program);
    }

    public async Task DeleteAsync(string uvaCode, string requesterId, string requesterRole)
    {
        var program = await _volProgramRepository.GetByCodeAsync(uvaCode)
            ?? throw new InvalidOperationException("Programa no encontrado");

        if (requesterRole != RoleConstants.AdminRole && program.ManagerProfileCode != requesterId)
        {
            throw new InvalidOperationException("No tienes permiso para eliminar este programa");
        }

        if (program.StateCode != ProgramStateConstants.ActiveCode)
        {
            throw new InvalidOperationException("Solo se pueden eliminar programas en estado activo");
        }

        program.SoftDelete(DateTime.UtcNow, ProgramStateConstants.DeletedCode);
        await _volProgramRepository.UpdateAsync(program);
    }

    private static VolProgramResponseDto MapToResponse(VolProgram program, Profile? manager)
    {
        return new VolProgramResponseDto
        {
            UvaCode = program.UvaCode,
            Name = program.Name,
            Acronym = program.Acronym,
            ManagerProfileId = program.ManagerProfileCode ?? string.Empty,
            ManagerName = manager is not null
                ? $"{manager.FirstName} {manager.LastName}"
                : "Desconocido",
            StateCode = program.StateCode,
            CreatedAt = program.CreatedAt,
        };
    }
}
