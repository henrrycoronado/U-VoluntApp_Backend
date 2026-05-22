namespace U_VoluntApp_Backend.Src.Application.Interfaces;

using U_VoluntApp_Backend.Src.Application.DTOs;

public interface IUserScholarshipService
{
    Task<ScholarshipResponseDto> RequestAsync(CreateScholarshipRequestDto dto, string profileCode);

    Task<ScholarshipResponseDto> AssignApprovedAsync(CreateScholarshipForProfileDto dto, string evaluatorCode);

    Task<ScholarshipResponseDto> ReviewAsync(string uvaCode, ReviewScholarshipDto dto, string evaluatorCode);

    Task<ScholarshipResponseDto> CompleteAsync(string uvaCode, CompleteScholarshipDto dto, string evaluatorCode);

    Task<ScholarshipResponseDto> GetByCodeAsync(string uvaCode);

    Task<List<ScholarshipResponseDto>> GetMyAsync(string profileCode);

    Task<List<ScholarshipResponseDto>> GetByProfileCodeAsync(string profileCode);
}
