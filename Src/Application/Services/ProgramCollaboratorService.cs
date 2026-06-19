namespace U_VoluntApp_Backend.Src.Application.Services;

using U_VoluntApp_Backend.Src.Application.DTOs;
using U_VoluntApp_Backend.Src.Application.Interfaces;
using U_VoluntApp_Backend.Src.Domain.Entities.Profile;
using U_VoluntApp_Backend.Src.Domain.Entities.VolProgram;
using U_VoluntApp_Backend.Src.Domain.Utils.Configuration;
using U_VoluntApp_Backend.Src.Domain.Utils.Constants;
using U_VoluntApp_Backend.Src.Domain.Utils.Enums;
using U_VoluntApp_Backend.Src.Infrastructure.Persistence.Interfaces.Profile;
using U_VoluntApp_Backend.Src.Infrastructure.Persistence.Interfaces.VolProgram;

public class ProgramCollaboratorService : IProgramCollaboratorService
{
    private readonly IProgramCollaboratorRepository _collaboratorRepository;
    private readonly IVolProgramRepository _programRepository;
    private readonly IProfileRepository _profileRepository;

    public ProgramCollaboratorService(
        IProgramCollaboratorRepository collaboratorRepository,
        IVolProgramRepository programRepository,
        IProfileRepository profileRepository)
    {
        _collaboratorRepository = collaboratorRepository;
        _programRepository = programRepository;
        _profileRepository = profileRepository;
    }

    public async Task<ProgramCollaboratorResponseDto> AddAsync(
        AddProgramCollaboratorDto dto,
        string requesterId,
        string requesterRole)
    {
        var program = await _programRepository.GetByCodeAsync(dto.ProgramCode)
            ?? throw new KeyNotFoundException("Programa no encontrado");

        var profileToAdd = await _profileRepository.GetByCodeAsync(dto.ProfileCode)
            ?? throw new KeyNotFoundException("Perfil no encontrado");

        if (profileToAdd.StateCode != ContractState.Active.GetUvaCode())
        {
            throw new InvalidOperationException("El perfil del usuario no está activo");
        }

        var existingFilter = new RequestFilter { Page = 1, PageSize = 100 };
        var existingCollabs = await _collaboratorRepository.GetByProgramCodeAsync(dto.ProgramCode, existingFilter);
        if (existingCollabs.Any(c => c.ProfileCode == dto.ProfileCode && c.StateCode == ContractState.Active.GetUvaCode()))
        {
            throw new InvalidOperationException("Este perfil ya es colaborador del programa");
        }

        if (existingCollabs.Any(c => c.ProfileCode == dto.ProfileCode && c.StateCode == ContractState.Pending.GetUvaCode()))
        {
            throw new InvalidOperationException("Este perfil ya tiene una solicitud pendiente en el programa");
        }

        bool isRequesterCollaborator = existingCollabs.Any(c => c.ProfileCode == requesterId && c.StateCode == ContractState.Active.GetUvaCode());

        bool canApproveDirectly = requesterRole == RoleConstants.SuperUserRole ||
            (requesterRole == RoleConstants.AdminRole && isRequesterCollaborator) ||
            program.ManagerProfileCode == requesterId;

        if (dto.ProfileCode != requesterId && !canApproveDirectly)
        {
            throw new UnauthorizedAccessException("No tienes permisos suficientes para agregar a otros usuarios directamente al programa");
        }

        string stateCode = canApproveDirectly
            ? ContractState.Active.GetUvaCode()
            : ContractState.Pending.GetUvaCode();

        var collaborator = ProgramCollaborator.Create(
            dto.ProgramCode,
            dto.ProfileCode,
            requesterId,
            stateCode,
            DateTime.UtcNow);

        await _collaboratorRepository.AddAsync(collaborator);

        return await MapToResponseAsync(collaborator);
    }

    public async Task<ProgramCollaboratorListDto> GetByProgramIdAsync(string programCode, string requesterId, string requesterRole, string stateCode)
    {
        var program = await _programRepository.GetByCodeAsync(programCode)
            ?? throw new KeyNotFoundException("Programa no encontrado");

        if (requesterRole != RoleConstants.AdminRole && program.ManagerProfileCode != requesterId)
        {
            throw new UnauthorizedAccessException("No tienes acceso a los colaboradores de este programa");
        }

        var filter = new RequestFilter { Page = 1, PageSize = 100 };
        var collaborators = await _collaboratorRepository.GetByProgramCodeAsync(programCode, filter);

        var dtos = new List<ProgramCollaboratorResponseDto>();
        foreach (var collab in collaborators)
        {
            if (!string.IsNullOrEmpty(stateCode) && collab.StateCode != stateCode)
            {
                continue;
            }

            dtos.Add(await MapToResponseAsync(collab));
        }

        return new ProgramCollaboratorListDto
        {
            Collaborators = dtos,
            Total = dtos.Count,
        };
    }

    public async Task<ProgramCollaboratorResponseDto?> GetByCodeAsync(string uvaCode)
    {
        var collaborator = await _collaboratorRepository.GetByCodeAsync(uvaCode);
        if (collaborator is null)
        {
            return null;
        }

        return await MapToResponseAsync(collaborator);
    }

    public async Task<bool> CanUserAccessProgramAsync(string userId, string programCode, string minStateCode = "stage-2")
    {
        var profile = await _profileRepository.GetByCodeAsync(userId);
        if (profile is null)
        {
            return false;
        }

        var filter = new RequestFilter { Page = 1, PageSize = 100 };
        var collaborators = await _collaboratorRepository.GetByProgramCodeAsync(programCode, filter);
        var collaborator = collaborators.FirstOrDefault(c => c.ProfileCode == userId);

        if (collaborator is null)
        {
            return false;
        }

        return collaborator.StateCode == minStateCode;
    }

    public async Task<ProgramCollaboratorResponseDto> UpdateAsync(
        string uvaCode,
        UpdateProgramCollaboratorDto dto,
        string requesterId,
        string requesterRole)
    {
        var collaborator = await _collaboratorRepository.GetByCodeAsync(uvaCode)
            ?? throw new KeyNotFoundException("Colaborador no encontrado");

        if (requesterRole != RoleConstants.AdminRole)
        {
            var filter = new RequestFilter { Page = 1, PageSize = 100 };
            var collaborators = await _collaboratorRepository.GetByProgramCodeAsync(collaborator.ProgramCode, filter);
            var requesterAsCollaborator = collaborators.FirstOrDefault(c => c.ProfileCode == requesterId);

            if (requesterAsCollaborator == null || requesterAsCollaborator.StateCode != ContractState.Active.GetUvaCode())
            {
                throw new UnauthorizedAccessException(
                    "Solo Admin o manager del programa pueden actualizar colaboradores");
            }
        }

        collaborator.ChangeState(dto.StateCode, DateTime.UtcNow);

        await _collaboratorRepository.UpdateAsync(collaborator);

        return await MapToResponseAsync(collaborator);
    }

    public async Task DeleteAsync(string uvaCode, string requesterId, string requesterRole)
    {
        var collaborator = await _collaboratorRepository.GetByCodeAsync(uvaCode)
            ?? throw new KeyNotFoundException("Colaborador no encontrado");

        if (requesterRole != RoleConstants.AdminRole)
        {
            var filter = new RequestFilter { Page = 1, PageSize = 100 };
            var collaborators = await _collaboratorRepository.GetByProgramCodeAsync(collaborator.ProgramCode, filter);
            var requesterAsCollaborator = collaborators.FirstOrDefault(c => c.ProfileCode == requesterId);

            if (requesterAsCollaborator == null || requesterAsCollaborator.StateCode != ContractState.Active.GetUvaCode())
            {
                throw new UnauthorizedAccessException(
                    "Solo Admin o manager del programa pueden eliminar colaboradores");
            }
        }

        await _collaboratorRepository.DeleteAsync(uvaCode);
    }

    private async Task<ProgramCollaboratorResponseDto> MapToResponseAsync(ProgramCollaborator collaborator)
    {
        var profile = await _profileRepository.GetByCodeAsync(collaborator.ProfileCode);

        var assignedByProfile = collaborator.AssignedByProfileCode != null
            ? await _profileRepository.GetByCodeAsync(collaborator.AssignedByProfileCode)
            : null;

        return new ProgramCollaboratorResponseDto
        {
            UvaCode = collaborator.UvaCode,
            ProgramCode = collaborator.ProgramCode,
            ProfileCode = collaborator.ProfileCode,
            ProfileName = profile is not null
                ? $"{profile.FirstName} {profile.LastName}"
                : null,
            AssignedByProfileCode = collaborator.AssignedByProfileCode,
            AssignedByName = assignedByProfile is not null
                ? $"{assignedByProfile.FirstName} {assignedByProfile.LastName}"
                : null,
            StateCode = collaborator.StateCode,
            StateName = collaborator.StateCode, // Should map to name if possible
            CreatedAt = collaborator.CreatedAt,
            UpdatedAt = collaborator.UpdatedAt ?? collaborator.CreatedAt,
        };
    }
}
