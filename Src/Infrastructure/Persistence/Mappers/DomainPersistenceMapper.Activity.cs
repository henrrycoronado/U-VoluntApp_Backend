namespace U_VoluntApp_Core.Src.Infrastructure.Persistence.Mappers;

using DomainEntities = U_VoluntApp_Core.Src.Domain.Entities;
using PersistenceModels = U_VoluntApp_Core.Src.Infrastructure.Persistence.Models;

public static partial class DomainPersistenceMapper
{
    public static DomainEntities.Activity.Activity ToDomain(PersistenceModels.Activity.Activity model)
    {
        return DomainEntities.Activity.Activity.Rehydrate(
            model.UvaCode,
            model.ProgramCode,
            model.ResponsibleProfileCode,
            model.ActivityTypeCode,
            model.VolProgramPatternCode,
            model.Name,
            model.Description,
            model.StartDate,
            model.EndDate,
            model.LocationLatitude,
            model.LocationLongitude,
            model.RegistrationRadiusMeters,
            model.StateCode,
            model.CreatedAt,
            model.UpdatedAt,
            model.DeletedAt);
    }

    public static DomainEntities.Activity.ActivityRule ToDomain(PersistenceModels.Activity.ActivityRule model)
    {
        return DomainEntities.Activity.ActivityRule.Rehydrate(
            model.UvaCode,
            model.ActivityCode,
            model.EnrollmentDeadline,
            model.RequiresApproval,
            model.TotalCapacity ?? 0,
            model.CostAmount,
            model.CountsVolunteerHours,
            model.RequiresEnrollment,
            model.PhotoUrl,
            model.CreatedAt,
            model.UpdatedAt);
    }

    public static DomainEntities.Activity.ActivityGroup ToDomain(PersistenceModels.Activity.ActivityGroup model)
    {
        return DomainEntities.Activity.ActivityGroup.Rehydrate(
            model.UvaCode,
            model.ActivityCode,
            model.Name,
            model.TotalCapacity ?? 0,
            model.Details,
            model.StartDate,
            model.EndDate,
            model.StateCode,
            model.CreatedAt,
            model.UpdatedAt,
            model.DeletedAt);
    }

    public static DomainEntities.VolProgram.VolProgramPattern ToDomain(PersistenceModels.VolProgram.VolProgramPattern model)
    {
        return DomainEntities.VolProgram.VolProgramPattern.Rehydrate(
            model.UvaCode,
            model.ProgramCode,
            model.Name,
            model.RecurrenceType,
            model.StateCode,
            model.CreatedAt,
            model.UpdatedAt,
            model.DeletedAt);
    }

    public static DomainEntities.VolProgram.VolProgramPatternDetail ToDomain(PersistenceModels.VolProgram.VolProgramPatternDetail model)
    {
        return DomainEntities.VolProgram.VolProgramPatternDetail.Rehydrate(
            model.UvaCode,
            model.VolProgramPatternCode,
            model.DayOfWeek,
            model.DayOfMonth,
            model.WeekOfMonth,
            model.StartHour,
            model.EndHour,
            model.StateCode,
            model.CreatedAt,
            model.UpdatedAt,
            model.DeletedAt);
    }

    public static PersistenceModels.Activity.Activity ToPersistence(DomainEntities.Activity.Activity entity)
    {
        return new PersistenceModels.Activity.Activity
        {
            UvaCode = entity.UvaCode,
            ProgramCode = entity.ProgramCode,
            ResponsibleProfileCode = entity.ResponsibleProfileCode,
            ActivityTypeCode = entity.ActivityTypeCode,
            VolProgramPatternCode = entity.VolProgramPatternCode,
            Name = entity.Name,
            Description = entity.Description,
            StartDate = entity.StartDate,
            EndDate = entity.EndDate,
            LocationLatitude = entity.LocationLatitude,
            LocationLongitude = entity.LocationLongitude,
            RegistrationRadiusMeters = entity.RegistrationRadiusMeters,
            StateCode = entity.StateCode,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt,
            DeletedAt = entity.DeletedAt,
        };
    }

    public static PersistenceModels.Activity.ActivityRule ToPersistence(DomainEntities.Activity.ActivityRule entity)
    {
        return new PersistenceModels.Activity.ActivityRule
        {
            UvaCode = entity.UvaCode,
            ActivityCode = entity.ActivityCode,
            RequiresEnrollment = entity.RequiresEnrollment,
            EnrollmentDeadline = entity.EnrollmentDeadline,
            RequiresApproval = entity.RequiresApproval,
            TotalCapacity = entity.TotalCapacity,
            CostAmount = entity.CostAmount,
            CountsVolunteerHours = entity.CountsVolunteerHours,
            PhotoUrl = entity.PhotoUrl,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt,
        };
    }

    public static PersistenceModels.Activity.ActivityGroup ToPersistence(DomainEntities.Activity.ActivityGroup entity)
    {
        return new PersistenceModels.Activity.ActivityGroup
        {
            UvaCode = entity.UvaCode,
            ActivityCode = entity.ActivityCode,
            Name = entity.Name,
            Details = entity.Details,
            TotalCapacity = entity.TotalCapacity,
            StartDate = entity.StartDate,
            EndDate = entity.EndDate,
            StateCode = entity.StateCode,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt,
            DeletedAt = entity.DeletedAt,
        };
    }

    public static PersistenceModels.VolProgram.VolProgramPattern ToPersistence(DomainEntities.VolProgram.VolProgramPattern entity)
    {
        return new PersistenceModels.VolProgram.VolProgramPattern
        {
            UvaCode = entity.UvaCode,
            ProgramCode = entity.ProgramCode,
            Name = entity.Name,
            RecurrenceType = entity.RecurrenceType,
            StateCode = entity.StateCode,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt,
            DeletedAt = entity.DeletedAt,
        };
    }

    public static PersistenceModels.VolProgram.VolProgramPatternDetail ToPersistence(DomainEntities.VolProgram.VolProgramPatternDetail entity)
    {
        return new PersistenceModels.VolProgram.VolProgramPatternDetail
        {
            UvaCode = entity.UvaCode,
            VolProgramPatternCode = entity.VolProgramPatternCode,
            DayOfWeek = entity.DayOfWeek,
            DayOfMonth = entity.DayOfMonth,
            WeekOfMonth = entity.WeekOfMonth,
            StartHour = entity.StartHour,
            EndHour = entity.EndHour,
            StateCode = entity.StateCode,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt,
            DeletedAt = entity.DeletedAt,
        };
    }
}
