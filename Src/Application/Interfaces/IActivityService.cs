namespace U_VoluntApp_Backend.Src.Application.Interfaces;

using U_VoluntApp_Backend.Src.Application.DTOs;

public interface IActivityService
{
    Task<ActivityResponseDto> CreateAsync(CreateActivityDto dto, string requesterId, string requesterRole);

    Task<ActivityResponseDto> CreateSimpleAsync(CreateActivitySimpleDto dto, string requesterId, string requesterRole);

    Task<ActivityResponseDto> GetByCodeAsync(string uvaCode, string requesterId, string requesterRole);

    Task<List<ActivityResponseDto>> GetByProgramAsync(string programCode, string requesterId, string requesterRole);

    Task<ActivityResponseDto> UpdateAsync(string uvaCode, UpdateActivityDto dto, string requesterId, string requesterRole);

    Task ChangeStateAsync(string uvaCode, ChangeActivityStateDto dto, string requesterId, string requesterRole);

    Task DeleteAsync(string uvaCode, string requesterId, string requesterRole);
}
