namespace U_VoluntApp_Core.Src.Application.Services;

using U_VoluntApp_Core.Src.Application.DTOs;
using U_VoluntApp_Core.Src.Application.Interfaces;
using U_VoluntApp_Core.Src.Domain.Entities.Profile;
using U_VoluntApp_Core.Src.Domain.Entities.VolProgram;
using U_VoluntApp_Core.Src.Domain.Utils.Configuration;
using U_VoluntApp_Core.Src.Domain.Utils.Constants;
using U_VoluntApp_Core.Src.Domain.Utils.Enums;
using U_VoluntApp_Core.Src.Infrastructure.Persistence.Interfaces.Profile;
using U_VoluntApp_Core.Src.Infrastructure.Persistence.Interfaces.VolProgram;

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
            ?? throw new KeyNotFoundException("Perfil no encontrado");

        string initialStateCode = requesterRole == RoleConstants.AdminRole
            ? ProgramState.Inactive.GetUvaCode()
            : ProgramState.Active.GetUvaCode();

        var program = VolProgram.Create(dto.Name, dto.Acronym ?? string.Empty, managerId, initialStateCode, DateTime.UtcNow);

        await _volProgramRepository.AddAsync(program);
        return MapToResponse(program, manager);
    }

    public async Task<VolProgramResponseDto> GetByCodeAsync(string uvaCode, string requesterId, string requesterRole)
    {
        var program = await _volProgramRepository.GetByCodeAsync(uvaCode)
            ?? throw new KeyNotFoundException("Programa no encontrado");

        var manager = await _profileRepository.GetByCodeAsync(program.ManagerProfileCode!)
            ?? throw new KeyNotFoundException("Manager no encontrado");

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
            ?? throw new KeyNotFoundException("Programa no encontrado");

        if (requesterRole != RoleConstants.AdminRole && requesterRole != RoleConstants.SuperUserRole && program.ManagerProfileCode != requesterId)
        {
            throw new UnauthorizedAccessException("No tienes permiso para modificar este programa");
        }

        if (program.StateCode == ProgramState.Deleted.GetUvaCode())
        {
            throw new InvalidOperationException("No se puede modificar un programa eliminado");
        }

        program.ApplyUpdate(dto.Name ?? program.Name, dto.Acronym, DateTime.UtcNow);

        if (program.VolProgramContent != null)
        {
            try
            {
                program.VolProgramContent.ApplyUpdate(
                    dto.Description,
                    program.VolProgramContent.ActivitiesDescription, // keep old
                    dto.ScheduleInfo,
                    dto.LeadershipInfo,
                    dto.ContactInfo,
                    dto.MissionStatement,
                    dto.ProfilePhotoUrl,
                    dto.CoverPhotoUrl,
                    DateTime.UtcNow);
            }
            catch (InvalidOperationException)
            {
                // Ignoring if no changes were made to the content
            }
        }
        else
        {
            program.SetVolProgramContent(U_VoluntApp_Core.Src.Domain.Entities.VolProgram.VolProgramContent.Create(
                program.UvaCode,
                dto.Description,
                null,
                dto.ScheduleInfo,
                dto.LeadershipInfo,
                dto.ContactInfo,
                dto.MissionStatement,
                dto.ProfilePhotoUrl,
                dto.CoverPhotoUrl,
                DateTime.UtcNow));
        }

        await _volProgramRepository.UpdateAsync(program);

        var manager = await _profileRepository.GetByCodeAsync(program.ManagerProfileCode!)
            ?? throw new KeyNotFoundException("Manager no encontrado");

        return MapToResponse(program, manager);
    }

    public async Task ChangeStateAsync(string uvaCode, ChangeVolProgramStateDto dto, string requesterId, string requesterRole)
    {
        if (requesterRole != RoleConstants.SuperUserRole)
        {
            throw new UnauthorizedAccessException("Solo SuperUser puede cambiar el estado de un programa");
        }

        var program = await _volProgramRepository.GetByCodeAsync(uvaCode)
            ?? throw new KeyNotFoundException("Programa no encontrado");

        if (program.StateCode == ProgramState.Deleted.GetUvaCode())
        {
            throw new InvalidOperationException("Un programa eliminado no puede cambiar de estado");
        }

        program.ChangeState(dto.StateCode, DateTime.UtcNow);

        await _volProgramRepository.UpdateAsync(program);
    }

    public async Task DeleteAsync(string uvaCode, string requesterId, string requesterRole)
    {
        var program = await _volProgramRepository.GetByCodeAsync(uvaCode)
            ?? throw new KeyNotFoundException("Programa no encontrado");

        if (requesterRole != RoleConstants.AdminRole && program.ManagerProfileCode != requesterId)
        {
            throw new UnauthorizedAccessException("No tienes permiso para eliminar este programa");
        }

        if (program.StateCode != ProgramState.Active.GetUvaCode())
        {
            throw new UnauthorizedAccessException("Solo se pueden eliminar programas en estado activo");
        }

        program.SoftDelete(DateTime.UtcNow, ProgramState.Deleted.GetUvaCode());
        await _volProgramRepository.UpdateAsync(program);
    }

    private static VolProgramResponseDto MapToResponse(VolProgram program, Profile? manager)
    {
        return new VolProgramResponseDto
        {
            UvaCode = program.UvaCode,
            Name = program.Name,
            Acronym = program.Acronym,
            Description = program.VolProgramContent?.Description,
            ProfilePhotoUrl = program.VolProgramContent?.ProfilePhotoUrl,
            CoverPhotoUrl = program.VolProgramContent?.CoverPhotoUrl,
            MissionStatement = program.VolProgramContent?.MissionStatement,
            ScheduleInfo = program.VolProgramContent?.ScheduleInfo,
            ContactInfo = program.VolProgramContent?.ContactInfo,
            LeadershipInfo = program.VolProgramContent?.LeadershipInfo,
            ManagerProfileId = program.ManagerProfileCode ?? string.Empty,
            ManagerName = manager is not null
                ? $"{manager.FirstName} {manager.LastName}"
                : "Desconocido",
            StateCode = program.StateCode,
            CreatedAt = program.CreatedAt,
        };
    }
}
