namespace U_VoluntApp_Backend.Src.Application.Services;

using Microsoft.AspNetCore.Http;
using U_VoluntApp_Backend.Src.Application.DTOs;
using U_VoluntApp_Backend.Src.Application.Interfaces;
using U_VoluntApp_Backend.Src.Domain.Entities.Profile;
using U_VoluntApp_Backend.Src.Domain.Utils.Configuration;
using U_VoluntApp_Backend.Src.Domain.Utils.Constants;
using U_VoluntApp_Backend.Src.Domain.Utils.Enums;
using U_VoluntApp_Backend.Src.Infrastructure.Persistence.Interfaces.Profile;
using U_VoluntApp_Backend.Src.Infrastructure.Storage;

public class ProfileService : IProfileService
{
    private readonly IProfileRepository _profileRepository;
    private readonly IStorageService _storageService;

    public ProfileService(
        IProfileRepository profileRepository,
        IStorageService storageService)
    {
        _profileRepository = profileRepository;
        _storageService = storageService;
    }

    public async Task<ProfileResponseDto> GetByCodeAsync(string uvaCode)
    {
        var profile = await _profileRepository.GetByCodeAsync(uvaCode)
            ?? throw new KeyNotFoundException("Perfil no encontrado");

        return MapToResponse(profile);
    }

    public async Task<ProfileResponseDto> UpdateAsync(string uvaCode, UpdateProfileDto dto)
    {
        var profile = await _profileRepository.GetByCodeAsync(uvaCode)
            ?? throw new KeyNotFoundException("Perfil no encontrado");

        profile.ApplyUpdate(
            dto.FirstName ?? profile.FirstName,
            dto.LastName ?? profile.LastName,
            dto.Phone ?? profile.Phone,
            dto.HousingLocation ?? profile.AddressLocation,
            dto.CareerCode ?? profile.CareerCode,
            dto.PersonalGoalHours ?? profile.PersonalGoalHours,
            DateTime.UtcNow);

        await _profileRepository.UpdateAsync(profile);
        return MapToResponse(profile);
    }

    public async Task<ProfileResponseDto> UpdatePhotoAsync(string uvaCode, IFormFile photo)
    {
        var profile = await _profileRepository.GetByCodeAsync(uvaCode)
            ?? throw new KeyNotFoundException("Perfil no encontrado");

        var photoUrl = await _storageService.UploadAsync(photo, StorageConstants.ProfileFolder);

        profile.UpdatePhoto(photoUrl, DateTime.UtcNow);

        await _profileRepository.UpdateAsync(profile);
        return MapToResponse(profile);
    }

    public async Task DeleteAsync(string uvaCode)
    {
        var profile = await _profileRepository.GetByCodeAsync(uvaCode)
            ?? throw new KeyNotFoundException("Perfil no encontrado");

        profile.SoftDelete(DateTime.UtcNow, ProfileState.Deleted.GetUvaCode());

        await _profileRepository.UpdateAsync(profile);
    }

    private static ProfileResponseDto MapToResponse(Profile profile)
    {
        return new ProfileResponseDto
        {
            UvaCode = profile.UvaCode,
            IdentityUserId = profile.IdentityUserId,
            FirstName = profile.FirstName,
            LastName = profile.LastName,
            Email = profile.Email,
            PhotoUrl = profile.PhotoUrl,
            Phone = profile.Phone,
            HousingLocation = profile.AddressLocation,
            CareerCode = profile.CareerCode,
            CareerName = null,
            PersonalGoalHours = profile.PersonalGoalHours,
            StateCode = profile.StateCode,
            CreatedAt = profile.CreatedAt,
        };
    }
}
