namespace U_VoluntApp_Backend.Src.Application.Services;

using U_VoluntApp_Backend.Src.Application.DTOs;
using U_VoluntApp_Backend.Src.Application.Interfaces;
using U_VoluntApp_Backend.Src.Domain.Entities.Activity;
using U_VoluntApp_Backend.Src.Domain.Entities.Enrollment;
using U_VoluntApp_Backend.Src.Domain.Entities.Profile;
using U_VoluntApp_Backend.Src.Domain.Utils.Configuration;
using U_VoluntApp_Backend.Src.Domain.Utils.Constants;
using U_VoluntApp_Backend.Src.Domain.Utils.Enums;
using U_VoluntApp_Backend.Src.Infrastructure.Persistence.Interfaces.Activity;
using U_VoluntApp_Backend.Src.Infrastructure.Persistence.Interfaces.Enrollment;
using U_VoluntApp_Backend.Src.Infrastructure.Persistence.Interfaces.Profile;
using U_VoluntApp_Backend.Src.Infrastructure.Persistence.Interfaces.VolProgram;

public class EnrollmentService : IEnrollmentService
{
    private readonly IEnrollmentRepository _enrollmentRepository;
    private readonly IActivityRepository _activityRepository;
    private readonly IActivityRuleRepository _activityRuleRepository;
    private readonly IActivityGroupRepository _activityGroupRepository;
    private readonly IVolProgramRepository _volProgramRepository;
    private readonly IProfileRepository _profileRepository;

    public EnrollmentService(
        IEnrollmentRepository enrollmentRepository,
        IActivityRepository activityRepository,
        IActivityRuleRepository activityRuleRepository,
        IActivityGroupRepository activityGroupRepository,
        IVolProgramRepository volProgramRepository,
        IProfileRepository profileRepository)
    {
        _enrollmentRepository = enrollmentRepository;
        _activityRepository = activityRepository;
        _activityRuleRepository = activityRuleRepository;
        _activityGroupRepository = activityGroupRepository;
        _volProgramRepository = volProgramRepository;
        _profileRepository = profileRepository;
    }

    public async Task<EnrollmentResponseDto> EnrollAsync(CreateEnrollmentDto dto, string profileCode)
    {
        var activity = await _activityRepository.GetByCodeAsync(dto.ActivityCode)
            ?? throw new KeyNotFoundException("Actividad no encontrada");

        if (activity.StateCode != ActivityState.Active.GetUvaCode())
        {
            throw new UnauthorizedAccessException("Solo se puede inscribir en actividades activas");
        }

        var filter = new RequestFilter { Page = 1, PageSize = 100 };
        var existingEnrollments = await _enrollmentRepository.GetByProfileCodeAsync(profileCode, filter);
        var existingEnrollment = existingEnrollments.FirstOrDefault(e => e.ActivityCode == dto.ActivityCode);

        if (existingEnrollment != null && 
            existingEnrollment.StateCode != EnrollmentState.Canceled.GetUvaCode() &&
            existingEnrollment.StateCode != EnrollmentState.Rejected.GetUvaCode())
        {
            throw new InvalidOperationException("Ya estás inscrito en esta actividad");
        }

        var rule = await _activityRuleRepository.GetByActivityCodeAsync(dto.ActivityCode);

        if (rule is not null)
        {
            if (rule.EnrollmentDeadline.HasValue && DateTime.UtcNow > rule.EnrollmentDeadline.Value)
            {
                throw new InvalidOperationException("El plazo de inscripción ha vencido");
            }

            if (rule.TotalCapacity > 0)
            {
                var currentEnrollments = await _enrollmentRepository.GetByActivityCodeAsync(dto.ActivityCode, filter);
                var approvedCount = currentEnrollments.Count(e => e.StateCode == EnrollmentState.Active.GetUvaCode());
                if (approvedCount >= rule.TotalCapacity)
                {
                    throw new InvalidOperationException("La actividad ha alcanzado su capacidad máxima");
                }
            }

            if (dto.ActivityGroupCode != null)
            {
                var group = await _activityGroupRepository.GetByCodeAsync(dto.ActivityGroupCode)
                    ?? throw new KeyNotFoundException("Grupo no encontrado");

                if (group.ActivityCode != activity.UvaCode)
                {
                    throw new InvalidOperationException("El grupo no pertenece a esta actividad");
                }
            }
        }

        string stateCode = (rule is not null && rule.RequiresApproval)
            ? EnrollmentState.Pending.GetUvaCode()
            : EnrollmentState.Active.GetUvaCode();

        if (existingEnrollment != null)
        {
            existingEnrollment.ChangeState(stateCode, DateTime.UtcNow);
            await _enrollmentRepository.UpdateAsync(existingEnrollment);
            var profile = await _profileRepository.GetByCodeAsync(profileCode);
            return MapToResponse(existingEnrollment, activity, profile);
        }
        else
        {
            var enrollment = Enrollment.Create(dto.ActivityCode, profileCode, stateCode, DateTime.UtcNow);
            await _enrollmentRepository.AddAsync(enrollment);
            var profile = await _profileRepository.GetByCodeAsync(profileCode);
            return MapToResponse(enrollment, activity, profile);
        }
    }

    public async Task<EnrollmentResponseDto> GetByCodeAsync(string uvaCode)
    {
        var enrollment = await _enrollmentRepository.GetByCodeAsync(uvaCode)
            ?? throw new KeyNotFoundException("Inscripción no encontrada");

        var activity = await _activityRepository.GetByCodeAsync(enrollment.ActivityCode)
            ?? throw new KeyNotFoundException("Actividad no encontrada");

        var profile = await _profileRepository.GetByCodeAsync(enrollment.EnrolledProfileCode);
        return MapToResponse(enrollment, activity, profile);
    }

    public async Task<List<EnrollmentResponseDto>> GetByActivityAsync(string activityCode, string requesterId, string requesterRole)
    {
        var activity = await _activityRepository.GetByCodeAsync(activityCode)
            ?? throw new KeyNotFoundException("Actividad no encontrada");

        var program = await _volProgramRepository.GetByCodeAsync(activity.ProgramCode)
            ?? throw new KeyNotFoundException("Programa no encontrado");

        if (requesterRole != RoleConstants.AdminRole && program.ManagerProfileCode != requesterId)
        {
            throw new UnauthorizedAccessException("No tienes acceso a las inscripciones de esta actividad");
        }

        var filter = new RequestFilter { Page = 1, PageSize = 100 };
        var enrollments = await _enrollmentRepository.GetByActivityCodeAsync(activityCode, filter);
        var result = new List<EnrollmentResponseDto>();

        foreach (var enrollment in enrollments)
        {
            var profile = await _profileRepository.GetByCodeAsync(enrollment.EnrolledProfileCode);
            result.Add(MapToResponse(enrollment, activity, profile));
        }

        return result;
    }

    public async Task<List<EnrollmentResponseDto>> GetMyEnrollmentsAsync(string profileCode)
    {
        var filter = new RequestFilter { Page = 1, PageSize = 100 };
        var enrollments = await _enrollmentRepository.GetByProfileCodeAsync(profileCode, filter);
        var result = new List<EnrollmentResponseDto>();

        foreach (var enrollment in enrollments)
        {
            var activity = await _activityRepository.GetByCodeAsync(enrollment.ActivityCode);
            var profile = await _profileRepository.GetByCodeAsync(profileCode);
            if (activity is not null)
            {
                result.Add(MapToResponse(enrollment, activity, profile));
            }
        }

        return result;
    }

    public async Task ReviewAsync(string uvaCode, ReviewEnrollmentDto dto, string requesterId, string requesterRole)
    {
        var enrollment = await _enrollmentRepository.GetByCodeAsync(uvaCode)
            ?? throw new KeyNotFoundException("Inscripción no encontrada");

        if (enrollment.StateCode != EnrollmentState.Pending.GetUvaCode())
        {
            throw new UnauthorizedAccessException("Solo se pueden revisar inscripciones en estado Pendiente");
        }

        var activity = await _activityRepository.GetByCodeAsync(enrollment.ActivityCode)
            ?? throw new KeyNotFoundException("Actividad no encontrada");

        var program = await _volProgramRepository.GetByCodeAsync(activity.ProgramCode)
            ?? throw new KeyNotFoundException("Programa no encontrado");

        if (requesterRole != RoleConstants.AdminRole && program.ManagerProfileCode != requesterId)
        {
            throw new UnauthorizedAccessException("No tienes permiso para revisar esta inscripción");
        }

        string newStateCode = dto.Approved ? EnrollmentState.Active.GetUvaCode() : EnrollmentState.Rejected.GetUvaCode();
        enrollment.ChangeState(newStateCode, DateTime.UtcNow);

        await _enrollmentRepository.UpdateAsync(enrollment);
    }

    public async Task CancelAsync(string uvaCode, string profileCode)
    {
        var enrollment = await _enrollmentRepository.GetByCodeAsync(uvaCode)
            ?? throw new KeyNotFoundException("Inscripción no encontrada");

        if (enrollment.EnrolledProfileCode != profileCode)
        {
            throw new InvalidOperationException("No puedes cancelar una inscripción que no es tuya");
        }

        if (enrollment.StateCode == EnrollmentState.Rejected.GetUvaCode()
            || enrollment.StateCode == EnrollmentState.Canceled.GetUvaCode())
        {
            throw new InvalidOperationException("No se puede cancelar una inscripción rechazada o ya cancelada");
        }

        enrollment.ChangeState(EnrollmentState.Canceled.GetUvaCode(), DateTime.UtcNow);

        await _enrollmentRepository.UpdateAsync(enrollment);
    }

    private static EnrollmentResponseDto MapToResponse(Enrollment enrollment, Activity activity, Profile? profile)
    {
        return new EnrollmentResponseDto
        {
            UvaCode = enrollment.UvaCode,
            ActivityCode = enrollment.ActivityCode,
            ActivityName = activity.Name,
            EnrolledProfileCode = enrollment.EnrolledProfileCode,
            EnrolledProfileName = profile is not null
                ? $"{profile.FirstName} {profile.LastName}"
                : "Desconocido",
            StateCode = enrollment.StateCode,
            CreatedAt = enrollment.CreatedAt,
        };
    }
}
