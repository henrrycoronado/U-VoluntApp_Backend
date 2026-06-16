namespace U_VoluntApp_Backend.Src.Application.Interfaces;

using Microsoft.AspNetCore.Http;
using U_VoluntApp_Backend.Src.Application.DTOs;

public interface IProfileService
{
    Task<ProfileResponseDto> GetByCodeAsync(string uvaCode);

    Task<ProfileResponseDto> UpdateAsync(string uvaCode, UpdateProfileDto dto);

    Task<ProfileResponseDto> UpdatePhotoAsync(string uvaCode, IFormFile photo);

    Task DeleteAsync(string uvaCode);
}
