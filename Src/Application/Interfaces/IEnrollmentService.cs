namespace U_VoluntApp_Core.Src.Application.Interfaces;

using U_VoluntApp_Core.Src.Application.DTOs;

public interface IEnrollmentService
{
    Task<EnrollmentResponseDto> EnrollAsync(CreateEnrollmentDto dto, string profileCode);

    Task<EnrollmentResponseDto> GetByCodeAsync(string uvaCode);

    Task<List<EnrollmentResponseDto>> GetByActivityAsync(string activityCode, string requesterId, string requesterRole);

    Task<List<EnrollmentResponseDto>> GetMyEnrollmentsAsync(string profileCode);

    Task ReviewAsync(string uvaCode, ReviewEnrollmentDto dto, string requesterId, string requesterRole);

    Task CancelAsync(string uvaCode, string profileCode);
}
