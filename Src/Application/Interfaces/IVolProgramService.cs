namespace U_VoluntApp_Backend.Src.Application.Interfaces;

using U_VoluntApp_Backend.Src.Application.DTOs;

public interface IVolProgramService
{
    Task<VolProgramResponseDto> CreateAsync(CreateVolProgramDto dto, string managerId, string requesterRole = "Admin");

    Task<VolProgramResponseDto> GetByCodeAsync(string uvaCode, string requesterId, string requesterRole);

    Task<List<VolProgramResponseDto>> GetAllAsync(string requesterId, string requesterRole);

    Task<VolProgramResponseDto> UpdateAsync(string uvaCode, UpdateVolProgramDto dto, string requesterId, string requesterRole);

    Task ChangeStateAsync(string uvaCode, ChangeVolProgramStateDto dto, string requesterId, string requesterRole);

    Task DeleteAsync(string uvaCode, string requesterId, string requesterRole);
}
