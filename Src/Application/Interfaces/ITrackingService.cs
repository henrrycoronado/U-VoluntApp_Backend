namespace U_VoluntApp_Backend.Src.Application.Interfaces;

using U_VoluntApp_Backend.Src.Application.DTOs;

public interface ITrackingService
{
    Task<TrackingLogResponseDto> CheckInAsync(CheckInDto dto, string profileCode);

    Task<TrackingLogResponseDto> CheckOutAsync(CheckOutDto dto, string profileCode);

    Task<TrackingLogResponseDto> ManualCheckInAsync(ManualCheckInDto dto, string requesterId, string requesterRole);

    Task<TrackingLogResponseDto> ManualCheckOutAsync(ManualCheckOutDto dto, string requesterId, string requesterRole);

    Task<TrackingLogResponseDto> GetByCodeAsync(string uvaCode);

    Task<List<TrackingLogResponseDto>> GetByEnrollmentAsync(string enrollmentCode);
}
