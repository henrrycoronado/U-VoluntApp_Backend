namespace U_VoluntApp_Core.Src.Application.Services;

using U_VoluntApp_Core.Src.Application.DTOs;
using U_VoluntApp_Core.Src.Application.Interfaces;
using U_VoluntApp_Core.Src.Domain.Entities.Activity;
using U_VoluntApp_Core.Src.Domain.Entities.Enrollment;
using U_VoluntApp_Core.Src.Domain.Entities.Profile;
using U_VoluntApp_Core.Src.Domain.Entities.Tracking;
using U_VoluntApp_Core.Src.Domain.Utils.Configuration;
using U_VoluntApp_Core.Src.Domain.Utils.Constants;
using U_VoluntApp_Core.Src.Domain.Utils.Enums;
using U_VoluntApp_Core.Src.Infrastructure.Persistence.Interfaces.Activity;
using U_VoluntApp_Core.Src.Infrastructure.Persistence.Interfaces.Enrollment;
using U_VoluntApp_Core.Src.Infrastructure.Persistence.Interfaces.Profile;
using U_VoluntApp_Core.Src.Infrastructure.Persistence.Interfaces.Tracking;
using U_VoluntApp_Core.Src.Infrastructure.Persistence.Interfaces.VolProgram;
using U_VoluntApp_Core.Src.Infrastructure.Storage;

public class TrackingService : ITrackingService
{
    private readonly ITrackingLogRepository _trackingLogRepository;
    private readonly IEnrollmentRepository _enrollmentRepository;
    private readonly IActivityRepository _activityRepository;
    private readonly IActivityRuleRepository _activityRuleRepository;
    private readonly IVolProgramRepository _volProgramRepository;
    private readonly IProfileRepository _profileRepository;
    private readonly IEvidenceRepository _evidenceRepository;
    private readonly IStorageService _storageService;
    private readonly IProgramCollaboratorService _programCollaboratorService;

    public TrackingService(
        ITrackingLogRepository trackingLogRepository,
        IEnrollmentRepository enrollmentRepository,
        IActivityRepository activityRepository,
        IActivityRuleRepository activityRuleRepository,
        IVolProgramRepository volProgramRepository,
        IProfileRepository profileRepository,
        IEvidenceRepository evidenceRepository,
        IStorageService storageService,
        IProgramCollaboratorService programCollaboratorService)
    {
        _trackingLogRepository = trackingLogRepository;
        _enrollmentRepository = enrollmentRepository;
        _activityRepository = activityRepository;
        _activityRuleRepository = activityRuleRepository;
        _volProgramRepository = volProgramRepository;
        _profileRepository = profileRepository;
        _evidenceRepository = evidenceRepository;
        _storageService = storageService;
        _programCollaboratorService = programCollaboratorService;
    }

    public async Task<TrackingLogResponseDto> CheckInAsync(CheckInDto dto, string profileCode)
    {
        var enrollment = await _enrollmentRepository.GetByCodeAsync(dto.EnrollmentCode)
            ?? throw new KeyNotFoundException("Inscripción no encontrada");

        if (enrollment.EnrolledProfileCode != profileCode)
        {
            throw new InvalidOperationException("No puedes hacer check-in de otra persona");
        }

        if (enrollment.StateCode != EnrollmentState.Active.GetUvaCode())
        {
            throw new InvalidOperationException("La inscripción debe estar aprobada para hacer check-in");
        }

        var filter = new RequestFilter { Page = 1, PageSize = 100 };
        var existing = await _trackingLogRepository.GetByEnrollmentCodeAsync(dto.EnrollmentCode, filter);
        if (existing.Any(l => l.ExitTime == null))
        {
            throw new InvalidOperationException("Ya tienes un check-in activo para esta inscripción");
        }

        if (dto.Evidence is null || dto.Evidence.Length == 0)
        {
            throw new InvalidOperationException("La evidencia de check-in es obligatoria");
        }

        var activity = await _activityRepository.GetByCodeAsync(enrollment.ActivityCode)
            ?? throw new KeyNotFoundException("Actividad no encontrada");

        if (activity.RegistrationRadiusMeters > 0)
        {
            if (dto.Latitude is null || dto.Longitude is null)
            {
                throw new InvalidOperationException("Esta actividad requiere ubicación para el check-in");
            }

            var distance = CalculateDistance(
                dto.Latitude.Value, dto.Longitude.Value, activity.LocationLatitude, activity.LocationLongitude);

            if (distance > activity.RegistrationRadiusMeters)
            {
                throw new InvalidOperationException($"Estás fuera del radio permitido ({activity.RegistrationRadiusMeters}m). Distancia actual: {distance:F0}m");
            }
        }

        var log = TrackingLog.Create(dto.EnrollmentCode, null, TrackingState.Active.GetUvaCode(), DateTime.UtcNow);
        log.CheckIn(DateTime.UtcNow, activity.StartDate, activity.EndDate, profileCode, DateTime.UtcNow);

        await _trackingLogRepository.AddAsync(log);

        var evidencePath = await _storageService.UploadAsync(dto.Evidence, StorageConstants.EvidenceFolder);
        var evidence = Evidence.Create(
            log.UvaCode,
            evidencePath,
            EvidenceType.CheckIn.GetUvaCode(),
            TrackingType.Scanning.GetUvaCode(),
            dto.Latitude ?? 0,
            dto.Longitude ?? 0,
            DateTime.UtcNow);
        await _evidenceRepository.AddAsync(evidence);

        var profile = await _profileRepository.GetByCodeAsync(profileCode);
        return await MapToResponseWithAuditAsync(log, activity, profile);
    }

    public async Task<TrackingLogResponseDto> CheckOutAsync(CheckOutDto dto, string profileCode)
    {
        var log = await _trackingLogRepository.GetByCodeAsync(dto.TrackingLogCode)
            ?? throw new KeyNotFoundException("Registro de tracking no encontrado");

        var enrollment = await _enrollmentRepository.GetByCodeAsync(log.EnrollmentCode)
            ?? throw new KeyNotFoundException("Inscripción no encontrada");

        if (enrollment.EnrolledProfileCode != profileCode)
        {
            throw new InvalidOperationException("No puedes hacer check-out de otra persona");
        }

        if (log.StateCode != TrackingState.Active.GetUvaCode())
        {
            throw new InvalidOperationException("Este registro ya fue completado o eliminado");
        }

        if (dto.Evidence is null || dto.Evidence.Length == 0)
        {
            throw new InvalidOperationException("La evidencia de check-out es obligatoria");
        }

        var activity = await _activityRepository.GetByCodeAsync(enrollment.ActivityCode)
            ?? throw new KeyNotFoundException("Actividad no encontrada");

        if (activity.RegistrationRadiusMeters > 0)
        {
            if (dto.Latitude is null || dto.Longitude is null)
            {
                throw new InvalidOperationException("Esta actividad requiere ubicación para el check-out");
            }

            var distance = CalculateDistance(
                dto.Latitude.Value, dto.Longitude.Value, activity.LocationLatitude, activity.LocationLongitude);

            if (distance > activity.RegistrationRadiusMeters)
            {
                throw new InvalidOperationException($"Estás fuera del radio permitido ({activity.RegistrationRadiusMeters}m). Distancia actual: {distance:F0}m");
            }
        }

        log.CheckOut(DateTime.UtcNow, activity.StartDate, activity.EndDate, profileCode, DateTime.UtcNow);

        await _trackingLogRepository.UpdateAsync(log);

        var evidencePath = await _storageService.UploadAsync(dto.Evidence, StorageConstants.EvidenceFolder);
        var evidence = Evidence.Create(
            log.UvaCode,
            evidencePath,
            EvidenceType.CheckOut.GetUvaCode(),
            TrackingType.Scanning.GetUvaCode(),
            dto.Latitude ?? 0,
            dto.Longitude ?? 0,
            DateTime.UtcNow);
        await _evidenceRepository.AddAsync(evidence);

        var profile = await _profileRepository.GetByCodeAsync(profileCode);
        return await MapToResponseWithAuditAsync(log, activity, profile);
    }

    public async Task<TrackingLogResponseDto> ManualCheckInAsync(ManualCheckInDto dto, string requesterId, string requesterRole)
    {
        var enrollment = await _enrollmentRepository.GetByCodeAsync(dto.EnrollmentCode)
            ?? throw new KeyNotFoundException("Inscripción no encontrada");

        var activity = await _activityRepository.GetByCodeAsync(enrollment.ActivityCode)
            ?? throw new KeyNotFoundException("Actividad no encontrada");

        if (requesterRole != RoleConstants.AdminRole)
        {
            var hasAccess = await _programCollaboratorService.CanUserAccessProgramAsync(
                requesterId, activity.ProgramCode, ContractState.Active.GetUvaCode());

            if (!hasAccess)
            {
                throw new UnauthorizedAccessException("No tienes acceso a este programa");
            }
        }

        if (enrollment.StateCode != EnrollmentState.Active.GetUvaCode())
        {
            throw new InvalidOperationException("La inscripción debe estar aprobada");
        }

        var filter = new RequestFilter { Page = 1, PageSize = 100 };
        var existing = await _trackingLogRepository.GetByEnrollmentCodeAsync(dto.EnrollmentCode, filter);
        if (existing.Any(l => l.ExitTime == null))
        {
            throw new InvalidOperationException("Ya existe un registro activo para esta inscripción");
        }

        var log = TrackingLog.Create(dto.EnrollmentCode, null, TrackingState.Active.GetUvaCode(), DateTime.UtcNow);
        log.CheckIn(dto.EntryTime, activity.StartDate, activity.EndDate, requesterId, DateTime.UtcNow);

        if (dto.ExitTime.HasValue)
        {
            log.CheckOut(dto.ExitTime.Value, activity.StartDate, activity.EndDate, requesterId, DateTime.UtcNow);
        }

        await _trackingLogRepository.AddAsync(log);

        var profile = await _profileRepository.GetByCodeAsync(enrollment.EnrolledProfileCode);
        return await MapToResponseWithAuditAsync(log, activity, profile);
    }

    public async Task<TrackingLogResponseDto> ManualCheckOutAsync(ManualCheckOutDto dto, string requesterId, string requesterRole)
    {
        var log = await _trackingLogRepository.GetByCodeAsync(dto.TrackingLogCode)
            ?? throw new KeyNotFoundException("Registro de tracking no encontrado");

        var enrollment = await _enrollmentRepository.GetByCodeAsync(log.EnrollmentCode)
            ?? throw new KeyNotFoundException("Inscripción no encontrada");

        var activity = await _activityRepository.GetByCodeAsync(enrollment.ActivityCode)
            ?? throw new KeyNotFoundException("Actividad no encontrada");

        if (requesterRole != RoleConstants.AdminRole)
        {
            var hasAccess = await _programCollaboratorService.CanUserAccessProgramAsync(
                requesterId, activity.ProgramCode, ContractState.Active.GetUvaCode());

            if (!hasAccess)
            {
                throw new UnauthorizedAccessException("No tienes acceso a este programa");
            }
        }

        if (log.StateCode != TrackingState.Active.GetUvaCode())
        {
            throw new InvalidOperationException("Este registro ya fue completado o eliminado");
        }

        log.CheckOut(dto.ExitTime ?? DateTime.UtcNow, activity.StartDate, activity.EndDate, requesterId, DateTime.UtcNow);

        await _trackingLogRepository.UpdateAsync(log);

        var profile = await _profileRepository.GetByCodeAsync(enrollment.EnrolledProfileCode);
        return await MapToResponseWithAuditAsync(log, activity, profile);
    }

    public async Task<TrackingLogResponseDto> GetByCodeAsync(string uvaCode)
    {
        var log = await _trackingLogRepository.GetByCodeAsync(uvaCode)
            ?? throw new KeyNotFoundException("Registro no encontrado");

        var enrollment = await _enrollmentRepository.GetByCodeAsync(log.EnrollmentCode)
            ?? throw new KeyNotFoundException("Inscripción no encontrada");

        var activity = await _activityRepository.GetByCodeAsync(enrollment.ActivityCode)
            ?? throw new KeyNotFoundException("Actividad no encontrada");

        var profile = await _profileRepository.GetByCodeAsync(enrollment.EnrolledProfileCode);
        return await MapToResponseWithAuditAsync(log, activity, profile);
    }

    public async Task<List<TrackingLogResponseDto>> GetByEnrollmentAsync(string enrollmentCode)
    {
        var enrollment = await _enrollmentRepository.GetByCodeAsync(enrollmentCode)
            ?? throw new KeyNotFoundException("Inscripción no encontrada");

        var activity = await _activityRepository.GetByCodeAsync(enrollment.ActivityCode)
            ?? throw new KeyNotFoundException("Actividad no encontrada");

        var filter = new RequestFilter { Page = 1, PageSize = 100 };
        var logs = await _trackingLogRepository.GetByEnrollmentCodeAsync(enrollmentCode, filter);
        var profile = await _profileRepository.GetByCodeAsync(enrollment.EnrolledProfileCode);

        var result = new List<TrackingLogResponseDto>();
        foreach (var log in logs)
        {
            result.Add(await MapToResponseWithAuditAsync(log, activity, profile));
        }

        return result;
    }

    private static double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
    {
        const double earthRadiusMeters = 6371000;
        var dLat = ToRadians(lat2 - lat1);
        var dLon = ToRadians(lon2 - lon1);
        var a = (Math.Sin(dLat / 2) * Math.Sin(dLat / 2))
            + (Math.Cos(ToRadians(lat1)) * Math.Cos(ToRadians(lat2))
            * Math.Sin(dLon / 2) * Math.Sin(dLon / 2));
        return earthRadiusMeters * 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
    }

    private static double ToRadians(double degrees) => degrees * Math.PI / 180;

    private static TrackingLogResponseDto MapToResponse(TrackingLog log, Activity activity, Profile? profile)
    {
        return new TrackingLogResponseDto
        {
            UvaCode = log.UvaCode,
            EnrollmentCode = log.EnrollmentCode,
            VolunteerName = profile is not null
                ? $"{profile.FirstName} {profile.LastName}"
                : "Desconocido",
            ActivityName = activity.Name,
            EntryTime = log.EntryTime,
            ExitTime = log.ExitTime,
            CalculatedHours = log.CalculatedHours,
            TypeCode = "type-1", // Should map from actual type
            StateCode = log.StateCode,
            CreatedAt = log.CreatedAt,
            CheckInRegisteredByCode = log.CheckInRegisteredByCode,
            CheckOutRegisteredByCode = log.CheckOutRegisteredByCode,
        };
    }

    private async Task<TrackingLogResponseDto> MapToResponseWithAuditAsync(TrackingLog log, Activity activity, Profile? profile)
    {
        var response = MapToResponse(log, activity, profile);

        if (!string.IsNullOrEmpty(log.CheckInRegisteredByCode))
        {
            var checkInRegistrant = await _profileRepository.GetByCodeAsync(log.CheckInRegisteredByCode);
            response.CheckInRegisteredByName = checkInRegistrant is not null
                ? $"{checkInRegistrant.FirstName} {checkInRegistrant.LastName}"
                : null;
        }

        if (!string.IsNullOrEmpty(log.CheckOutRegisteredByCode))
        {
            var checkOutRegistrant = await _profileRepository.GetByCodeAsync(log.CheckOutRegisteredByCode);
            response.CheckOutRegisteredByName = checkOutRegistrant is not null
                ? $"{checkOutRegistrant.FirstName} {checkOutRegistrant.LastName}"
                : null;
        }

        return response;
    }
}
