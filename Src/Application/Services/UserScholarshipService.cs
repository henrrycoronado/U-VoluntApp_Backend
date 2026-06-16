namespace U_VoluntApp_Backend.Src.Application.Services;

using U_VoluntApp_Backend.Src.Application.DTOs;
using U_VoluntApp_Backend.Src.Application.Interfaces;
using U_VoluntApp_Backend.Src.Domain.Entities.Contract;
using U_VoluntApp_Backend.Src.Domain.Entities.Profile;
using U_VoluntApp_Backend.Src.Domain.Utils.Configuration;
using U_VoluntApp_Backend.Src.Domain.Utils.Enums;
using U_VoluntApp_Backend.Src.Infrastructure.Persistence.Interfaces.Contract;
using U_VoluntApp_Backend.Src.Infrastructure.Persistence.Interfaces.Profile;

public class UserScholarshipService : IUserScholarshipService
{
    private readonly IUserScholarshipRepository _scholarshipRepository;
    private readonly IProfileRepository _profileRepository;

    public UserScholarshipService(
        IUserScholarshipRepository scholarshipRepository,
        IProfileRepository profileRepository)
    {
        _scholarshipRepository = scholarshipRepository;
        _profileRepository = profileRepository;
    }

    public async Task<ScholarshipResponseDto> RequestAsync(CreateScholarshipRequestDto dto, string profileCode)
    {
        return await CreatePendingAsync(
            profileCode,
            dto.ScholarshipTypeCode,
            dto.Reason,
            dto.StartDate ?? DateTime.UtcNow,
            dto.EndDate ?? DateTime.UtcNow.AddYears(1),
            null);
    }

    public async Task<ScholarshipResponseDto> AssignApprovedAsync(
        CreateScholarshipForProfileDto dto,
        string evaluatorCode)
    {
        if (dto.RequiredHours <= 0)
        {
            throw new InvalidOperationException("Las horas requeridas son obligatorias");
        }

        var filter = new RequestFilter { Page = 1, PageSize = 100 };
        var active = await _scholarshipRepository.GetByProfileCodeAsync(dto.ProfileCode, filter);
        if (active.Any(s => s.StateCode == ContractState.Active.GetUvaCode()))
        {
            throw new InvalidOperationException("El voluntario ya tiene una beca activa");
        }

        var profile = await _profileRepository.GetByCodeAsync(dto.ProfileCode)
            ?? throw new InvalidOperationException("Perfil no encontrado");

        var scholarship = UserScholarship.CreateApproved(
            dto.ProfileCode,
            evaluatorCode,
            dto.StartDate ?? DateTime.UtcNow,
            dto.EndDate ?? DateTime.UtcNow.AddYears(1),
            dto.ScholarshipTypeCode,
            dto.RequiredHours,
            dto.Reason,
            ContractState.Active.GetUvaCode(),
            DateTime.UtcNow);

        await _scholarshipRepository.AddAsync(scholarship);

        return MapToResponse(scholarship, profile);
    }

    public async Task<ScholarshipResponseDto> ReviewAsync(string uvaCode, ReviewScholarshipDto dto, string evaluatorCode)
    {
        var scholarship = await _scholarshipRepository.GetByCodeAsync(uvaCode)
            ?? throw new InvalidOperationException("Beca no encontrada");

        if (dto.Approve)
        {
            var filter = new RequestFilter { Page = 1, PageSize = 100 };
            var active = await _scholarshipRepository.GetByProfileCodeAsync(scholarship.AssignedProfileCode, filter);
            if (active.Any(s => s.StateCode == ContractState.Active.GetUvaCode()))
            {
                throw new InvalidOperationException("El voluntario ya tiene una beca activa");
            }
        }

        string newStateCode = dto.Approve ? ContractState.Active.GetUvaCode() : ContractState.Rejected.GetUvaCode();
        scholarship.Review(newStateCode, evaluatorCode, dto.RequiredHours ?? scholarship.RequiredHours, DateTime.UtcNow);

        await _scholarshipRepository.UpdateAsync(scholarship);

        return MapToResponse(scholarship, null);
    }

    public async Task<ScholarshipResponseDto> CompleteAsync(string uvaCode, CompleteScholarshipDto dto, string evaluatorCode)
    {
        var scholarship = await _scholarshipRepository.GetByCodeAsync(uvaCode)
            ?? throw new InvalidOperationException("Beca no encontrada");

        scholarship.Complete(dto.EndDate ?? DateTime.UtcNow, ContractState.Canceled.GetUvaCode(), DateTime.UtcNow); // Use a better state for completed if available

        await _scholarshipRepository.UpdateAsync(scholarship);

        return MapToResponse(scholarship, null);
    }

    public async Task<ScholarshipResponseDto> GetByCodeAsync(string uvaCode)
    {
        var scholarship = await _scholarshipRepository.GetByCodeAsync(uvaCode)
            ?? throw new InvalidOperationException("Beca no encontrada");

        return MapToResponse(scholarship, null);
    }

    public async Task<List<ScholarshipResponseDto>> GetMyAsync(string profileCode)
    {
        var filter = new RequestFilter { Page = 1, PageSize = 100 };
        var scholarships = await _scholarshipRepository.GetByProfileCodeAsync(profileCode, filter);
        return scholarships.Select(s => MapToResponse(s, null)).ToList();
    }

    public async Task<List<ScholarshipResponseDto>> GetByProfileCodeAsync(string profileCode)
    {
        var filter = new RequestFilter { Page = 1, PageSize = 100 };
        var scholarships = await _scholarshipRepository.GetByProfileCodeAsync(profileCode, filter);
        return scholarships.Select(s => MapToResponse(s, null)).ToList();
    }

    private static ScholarshipResponseDto MapToResponse(UserScholarship scholarship, Profile? profile)
    {
        var volunteerName = profile is not null
            ? $"{profile.FirstName} {profile.LastName}"
            : "Desconocido";

        return new ScholarshipResponseDto
        {
            UvaCode = scholarship.UvaCode,
            ProfileCode = scholarship.AssignedProfileCode,
            VolunteerName = volunteerName,
            ScholarshipTypeCode = scholarship.ScholarshipTypeCode,
            ScholarshipType = scholarship.ScholarshipTypeCode, // Map to name if possible
            Reason = scholarship.Reason,
            RequiredHours = scholarship.RequiredHours,
            StartDate = scholarship.StartDate,
            EndDate = scholarship.EndDate,
            StateCode = scholarship.StateCode,
            EvaluatorProfileCode = scholarship.EvaluatorProfileCode,
            CreatedAt = scholarship.CreatedAt,
            UpdatedAt = scholarship.UpdatedAt ?? scholarship.CreatedAt,
        };
    }

    private async Task<ScholarshipResponseDto> CreatePendingAsync(
        string profileCode,
        string scholarshipTypeCode,
        string reason,
        DateTime startDate,
        DateTime endDate,
        string? evaluatorCode)
    {
        var filter = new RequestFilter { Page = 1, PageSize = 100 };
        var active = await _scholarshipRepository.GetByProfileCodeAsync(profileCode, filter);
        if (active.Any(s => s.StateCode == ContractState.Active.GetUvaCode()))
        {
            throw new InvalidOperationException("Ya tienes una beca activa");
        }

        var profile = await _profileRepository.GetByCodeAsync(profileCode)
            ?? throw new InvalidOperationException("Perfil no encontrado");

        var scholarship = UserScholarship.CreatePending(
            profileCode,
            startDate,
            endDate,
            scholarshipTypeCode,
            100.00m, // Default hours
            reason,
            ContractState.Pending.GetUvaCode(),
            DateTime.UtcNow);

        await _scholarshipRepository.AddAsync(scholarship);

        return MapToResponse(scholarship, profile);
    }
}
