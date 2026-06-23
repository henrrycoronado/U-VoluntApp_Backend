namespace U_VoluntApp_Backend.Src.Application.Services;

using U_VoluntApp_Backend.Src.Application.DTOs;
using U_VoluntApp_Backend.Src.Application.Interfaces;
using U_VoluntApp_Backend.Src.Domain.Entities.Activity;
using U_VoluntApp_Backend.Src.Domain.Entities.VolProgram;
using U_VoluntApp_Backend.Src.Domain.Utils.Configuration;
using U_VoluntApp_Backend.Src.Domain.Utils.Constants;
using U_VoluntApp_Backend.Src.Domain.Utils.Enums;
using U_VoluntApp_Backend.Src.Domain.Utils.Factories;
using U_VoluntApp_Backend.Src.Infrastructure.Persistence.Interfaces.Activity;
using U_VoluntApp_Backend.Src.Infrastructure.Persistence.Interfaces.Profile;
using U_VoluntApp_Backend.Src.Infrastructure.Persistence.Interfaces.VolProgram;

public class ActivityService : IActivityService
{
    private readonly IActivityRepository _activityRepository;
    private readonly IVolProgramRepository _volProgramRepository;
    private readonly IActivityRuleRepository _activityRuleRepository;
    private readonly IActivityGroupRepository _activityGroupRepository;
    private readonly IActivityFactory _activityFactory;
    private readonly IProfileRepository _profileRepository;

    public ActivityService(
        IActivityRepository activityRepository,
        IVolProgramRepository volProgramRepository,
        IActivityRuleRepository activityRuleRepository,
        IActivityGroupRepository activityGroupRepository,
        IActivityFactory activityFactory,
        IProfileRepository profileRepository)
    {
        _activityRepository = activityRepository;
        _volProgramRepository = volProgramRepository;
        _activityRuleRepository = activityRuleRepository;
        _activityGroupRepository = activityGroupRepository;
        _activityFactory = activityFactory;
        _profileRepository = profileRepository;
    }

    public async Task<ActivityResponseDto> CreateAsync(CreateActivityDto dto, string requesterId, string requesterRole)
    {
        var program = await _volProgramRepository.GetByCodeAsync(dto.ProgramCode)
            ?? throw new KeyNotFoundException("Programa no encontrado");

        if (requesterRole != RoleConstants.AdminRole && program.ManagerProfileCode != requesterId)
        {
            throw new UnauthorizedAccessException("No tienes permiso para agregar actividades a este programa");
        }

        if (program.StateCode == ProgramState.Deleted.GetUvaCode())
        {
            throw new InvalidOperationException("No se pueden agregar actividades a un programa eliminado");
        }

        var nowUtc = DateTime.UtcNow;
        var activity = Activity.Create(
            dto.ProgramCode,
            requesterId,
            dto.ActivityTypeCode,
            null,
            dto.Name,
            dto.Description,
            dto.StartDate,
            dto.EndDate,
            50, // Default radius
            ActivityState.Inactive.GetUvaCode(),
            dto.LocationLatitude ?? 0,
            dto.LocationLongitude ?? 0,
            nowUtc);

        await _activityRepository.AddAsync(activity);

        if (dto.Rule is not null)
        {
            var rule = ActivityRule.Create(
                activity.UvaCode,
                dto.RequiresEnrollment,
                dto.Rule.RequiresApproval,
                dto.CountsVolunteerHours ?? true,
                dto.PhotoUrl,
                dto.Rule.EnrollmentDeadline,
                dto.StartDate,
                dto.Rule.TotalCapacity ?? 0,
                dto.CostAmount ?? 0,
                nowUtc);

            await _activityRuleRepository.AddAsync(rule);

            foreach (var groupDto in dto.Rule.Groups)
            {
                var group = ActivityGroup.Create(
                    activity.UvaCode,
                    groupDto.Name,
                    ActivityState.Active.GetUvaCode(),
                    groupDto.Details,
                    groupDto.Capacity ?? 0,
                    groupDto.StartDate,
                    groupDto.EndDate,
                    dto.StartDate,
                    dto.EndDate,
                    nowUtc);

                await _activityGroupRepository.AddAsync(group);
            }
        }

        return await MapToResponseAsync(activity, program);
    }

    public async Task<ActivityResponseDto> CreateSimpleAsync(CreateActivitySimpleDto dto, string requesterId, string requesterRole)
    {
        var program = await _volProgramRepository.GetByCodeAsync(dto.ProgramCode)
            ?? throw new KeyNotFoundException("Programa no encontrado");

        if (requesterRole != RoleConstants.AdminRole && program.ManagerProfileCode != requesterId)
        {
            throw new UnauthorizedAccessException("No tienes permiso para agregar actividades a este programa");
        }

        if (program.StateCode == ProgramState.Deleted.GetUvaCode())
        {
            throw new InvalidOperationException("No se pueden agregar actividades a un programa eliminado");
        }

        var (activity, rule) = dto.ActivityTypeCode switch
        {
            var code when code == ActivityType.Workshop.GetUvaCode() => _activityFactory.CreateWorkshop(dto),
            var code when code == ActivityType.Mentoring.GetUvaCode() => _activityFactory.CreateMentoring(dto),
            var code when code == ActivityType.Brigade.GetUvaCode() => _activityFactory.CreateBrigade(dto),
            var code when code == ActivityType.Event.GetUvaCode() => _activityFactory.CreateEvent(dto),
            _ => throw new InvalidOperationException("Tipo de actividad no válido")
        };

        await _activityRepository.AddAsync(activity);

        if (rule is not null)
        {
            await _activityRuleRepository.AddAsync(rule);
        }

        return await MapToResponseAsync(activity, program);
    }

    public async Task<ActivityResponseDto> GetByCodeAsync(string uvaCode, string requesterId, string requesterRole)
    {
        var activity = await _activityRepository.GetByCodeAsync(uvaCode)
            ?? throw new KeyNotFoundException("Actividad no encontrada");

        var program = await _volProgramRepository.GetByCodeAsync(activity.ProgramCode)
            ?? throw new KeyNotFoundException("Programa no encontrado");

        return await MapToResponseAsync(activity, program);
    }

    public async Task<List<ActivityResponseDto>> GetByProgramAsync(string programCode, string requesterId, string requesterRole)
    {
        var program = await _volProgramRepository.GetByCodeAsync(programCode)
            ?? throw new KeyNotFoundException("Programa no encontrado");

        var filter = new RequestFilter { Page = 1, PageSize = 100 };
        var activities = await _activityRepository.GetByProgramCodeAsync(programCode, filter);
        var result = new List<ActivityResponseDto>();

        foreach (var activity in activities)
        {
            result.Add(await MapToResponseAsync(activity, program));
        }

        return result;
    }

    public async Task<ActivityResponseDto> UpdateAsync(string uvaCode, UpdateActivityDto dto, string requesterId, string requesterRole)
    {
        var activity = await _activityRepository.GetByCodeAsync(uvaCode)
            ?? throw new KeyNotFoundException("Actividad no encontrada");

        var program = await _volProgramRepository.GetByCodeAsync(activity.ProgramCode)
            ?? throw new KeyNotFoundException("Programa no encontrado");

        if (requesterRole != RoleConstants.AdminRole && program.ManagerProfileCode != requesterId)
        {
            throw new UnauthorizedAccessException("No tienes permiso para modificar esta actividad");
        }

        if (activity.StateCode == ActivityState.Canceled.GetUvaCode())
        {
            throw new InvalidOperationException("No se puede modificar una actividad cancelada");
        }

        var nowUtc = DateTime.UtcNow;
        activity.ApplyUpdate(
            activity.ResponsibleProfileCode,
            dto.ActivityTypeCode ?? activity.ActivityTypeCode,
            activity.ActivityRecurrencePatternCode,
            dto.Name ?? activity.Name,
            dto.Description ?? activity.Description,
            dto.StartDate ?? activity.StartDate,
            dto.EndDate ?? activity.EndDate,
            dto.LocationLatitude ?? activity.LocationLatitude,
            dto.LocationLongitude ?? activity.LocationLongitude,
            activity.RegistrationRadiusMeters,
            nowUtc);

        await _activityRepository.UpdateAsync(activity);
        return await MapToResponseAsync(activity, program);
    }

    public async Task ChangeStateAsync(string uvaCode, ChangeActivityStateDto dto, string requesterId, string requesterRole)
    {
        var activity = await _activityRepository.GetByCodeAsync(uvaCode)
            ?? throw new KeyNotFoundException("Actividad no encontrada");

        var program = await _volProgramRepository.GetByCodeAsync(activity.ProgramCode)
            ?? throw new KeyNotFoundException("Programa no encontrado");

        if (requesterRole != RoleConstants.AdminRole && program.ManagerProfileCode != requesterId)
        {
            throw new UnauthorizedAccessException("No tienes permiso para cambiar el estado de esta actividad");
        }

        if (activity.StateCode == ActivityState.Deleted.GetUvaCode())
        {
            throw new InvalidOperationException("Una actividad eliminada no puede cambiar de estado");
        }

        var nowUtc = DateTime.UtcNow;
        activity.ChangeState(dto.StateCode, nowUtc);

        await _activityRepository.UpdateAsync(activity);
    }

    public async Task DeleteAsync(string uvaCode, string requesterId, string requesterRole)
    {
        var activity = await _activityRepository.GetByCodeAsync(uvaCode)
            ?? throw new KeyNotFoundException("Actividad no encontrada");

        var program = await _volProgramRepository.GetByCodeAsync(activity.ProgramCode)
            ?? throw new KeyNotFoundException("Programa no encontrado");

        if (requesterRole != RoleConstants.AdminRole && program.ManagerProfileCode != requesterId)
        {
            throw new UnauthorizedAccessException("No tienes permiso para eliminar esta actividad");
        }

        if (activity.StateCode != ActivityState.Inactive.GetUvaCode())
        {
            throw new UnauthorizedAccessException("Solo se pueden eliminar actividades en estado Inactivo");
        }

        activity.SoftDelete(ActivityState.Deleted.GetUvaCode(), DateTime.UtcNow);
        await _activityRepository.UpdateAsync(activity);
    }

    private async Task<ActivityResponseDto> MapToResponseAsync(Activity activity, VolProgram program)
    {
        var rule = await _activityRuleRepository.GetByActivityCodeAsync(activity.UvaCode);
        ActivityRuleResponseDto? ruleDto = null;

        if (rule is not null)
        {
            var filter = new RequestFilter { Page = 1, PageSize = 100 };
            var groups = await _activityGroupRepository.GetByActivityCodeAsync(activity.UvaCode, filter);
            ruleDto = new ActivityRuleResponseDto
            {
                UvaCode = rule.UvaCode,
                RegistrationRadiusMeters = activity.RegistrationRadiusMeters,
                EnrollmentDeadline = rule.EnrollmentDeadline,
                RequiresApproval = rule.RequiresApproval,
                TotalCapacity = rule.TotalCapacity,
                Groups = groups.Select(g => new ActivityGroupResponseDto
                {
                    UvaCode = g.UvaCode,
                    Name = g.Name,
                    Details = g.Details,
                    StartDate = g.StartDate ?? DateTime.MinValue,
                    EndDate = g.EndDate ?? DateTime.MinValue,
                    Capacity = g.TotalCapacity,
                }).ToList(),
            };
        }

        return new ActivityResponseDto
        {
            UvaCode = activity.UvaCode,
            ProgramCode = activity.ProgramCode,
            ProgramName = program.Name,
            Name = activity.Name,
            Description = activity.Description,
            StartDate = activity.StartDate,
            EndDate = activity.EndDate,
            LocationLatitude = activity.LocationLatitude,
            LocationLongitude = activity.LocationLongitude,
            RequiresEnrollment = activity.Rule != null && activity.Rule.RequiresEnrollment,
            State = activity.StateCode, // Should map to name if possible, but StateCode is fine for now
            StateCode = activity.StateCode,
            Rule = ruleDto,
            CreatedAt = activity.CreatedAt,
            UpdatedAt = activity.UpdatedAt ?? activity.CreatedAt,
        };
    }
}
