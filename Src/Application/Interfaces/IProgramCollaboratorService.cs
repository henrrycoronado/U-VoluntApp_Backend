namespace U_VoluntApp_Core.Src.Application.Interfaces;

using U_VoluntApp_Core.Src.Application.DTOs;

public interface IProgramCollaboratorService
{
    Task<ProgramCollaboratorResponseDto> AddAsync(AddProgramCollaboratorDto dto, string requesterId, string requesterRole);

    Task<ProgramCollaboratorListDto> GetByProgramIdAsync(string programCode, string requesterId, string requesterRole, string stateCode);

    Task<ProgramCollaboratorResponseDto?> GetByCodeAsync(string uvaCode);

    Task<bool> CanUserAccessProgramAsync(string userId, string programCode, string minStateCode = "stage-2");

    Task<ProgramCollaboratorResponseDto> UpdateAsync(string uvaCode, UpdateProgramCollaboratorDto dto, string requesterId, string requesterRole);

    Task DeleteAsync(string uvaCode, string requesterId, string requesterRole);
}
