namespace U_VoluntApp_Core.Src.Application.Interfaces;

using U_VoluntApp_Core.Src.Application.DTOs;

public interface IVolProgramCollaboratorService
{
    Task<VolProgramCollaboratorResponseDto> AddAsync(AddVolProgramCollaboratorDto dto, string requesterId, string requesterRole);

    Task<VolProgramCollaboratorListDto> GetByProgramIdAsync(string programCode, string requesterId, string requesterRole, string stateCode);

    Task<VolProgramCollaboratorResponseDto?> GetByCodeAsync(string uvaCode);

    Task<bool> CanUserAccessProgramAsync(string userId, string programCode, string minStateCode = "stage-2");

    Task<VolProgramCollaboratorResponseDto> UpdateAsync(string uvaCode, UpdateVolProgramCollaboratorDto dto, string requesterId, string requesterRole);

    Task DeleteAsync(string uvaCode, string requesterId, string requesterRole);
}
