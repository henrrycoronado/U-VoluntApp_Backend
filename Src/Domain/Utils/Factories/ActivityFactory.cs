namespace U_VoluntApp_Backend.Src.Domain.Utils.Factories;

using System;
using U_VoluntApp_Backend.Src.Application.DTOs;
using U_VoluntApp_Backend.Src.Domain.Entities.Activity;
using U_VoluntApp_Backend.Src.Domain.Utils.Constants;

public class ActivityFactory : IActivityFactory
{
    private const int DefaultRegistrationRadius = 50;

    public (Activity Activity, ActivityRule Rule) CreateWorkshop(CreateActivitySimpleDto dto)
    {
        return CreateWithOptionalRule(dto);
    }

    public (Activity Activity, ActivityRule Rule) CreateMentoring(CreateActivitySimpleDto dto)
    {
        return CreateWithOptionalRule(dto);
    }

    public (Activity Activity, ActivityRule Rule) CreateBrigade(CreateActivitySimpleDto dto)
    {
        return CreateWithOptionalRule(dto);
    }

    public (Activity Activity, ActivityRule Rule) CreateMultiDay(CreateActivitySimpleDto dto)
    {
        return CreateWithOptionalRule(dto);
    }

    public (Activity Activity, ActivityRule Rule) CreateEvent(CreateActivitySimpleDto dto)
    {
        return CreateWithOptionalRule(dto);
    }

    public (Activity Activity, ActivityRule Rule) CloneFromMultiDayTemplate(CreateActivitySimpleDto simpleDto)
    {
        return CreateWithOptionalRule(simpleDto);
    }

    private static (Activity Activity, ActivityRule Rule) CreateWithOptionalRule(CreateActivitySimpleDto dto)
    {
        var nowUtc = DateTime.UtcNow;

        var activity = Activity.Create(
            dto.ProgramCode,
            null,
            dto.ActivityTypeCode,
            null,
            dto.Name,
            dto.Description,
            dto.StartDate,
            dto.EndDate,
            DefaultRegistrationRadius,
            ActivityStateConstants.InactiveCode,
            dto.LocationLatitude ?? 0,
            dto.LocationLongitude ?? 0,
            nowUtc);

        ActivityRule? rule = null;

        // Create a rule when enrollment/approval/capacity/deadline/settings are provided
        if (dto.RequiresEnrollment || dto.RequiresApproval || dto.Capacity.HasValue || dto.EnrollmentDeadline.HasValue)
        {
            rule = ActivityRule.Create(
                activity.UvaCode,
                dto.RequiresEnrollment,
                dto.RequiresApproval,
                true, // counts volunteer hours by default
                null,
                dto.EnrollmentDeadline,
                dto.StartDate,
                dto.Capacity ?? 0,
                0m,
                nowUtc);
        }

        return (activity, rule!);
    }
}
