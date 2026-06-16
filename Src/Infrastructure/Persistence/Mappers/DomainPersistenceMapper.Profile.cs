namespace U_VoluntApp_Backend.Src.Infrastructure.Persistence.Mappers;

using System;
using DomainEntities = U_VoluntApp_Backend.Src.Domain.Entities;
using PersistenceModels = U_VoluntApp_Backend.Src.Infrastructure.Persistence.Models;

public static partial class DomainPersistenceMapper
{
    public static DomainEntities.Profile.Profile ToDomain(PersistenceModels.Profile.Profile model)
    {
        return DomainEntities.Profile.Profile.Rehydrate(
            model.UvaCode,
            model.IdentityUserId,
            model.FirstName,
            model.LastName,
            model.Email,
            model.PhotoUrl,
            model.CareerCode,
            model.AddressLocation,
            model.Phone,
            model.PersonalGoalHours,
            model.StateCode,
            model.CreatedAt,
            model.UpdatedAt,
            model.DeletedAt);
    }

    public static DomainEntities.Profile.ScholarshipPerformance ToDomain(PersistenceModels.Profile.MvScholarshipPerformance model)
    {
        return DomainEntities.Profile.ScholarshipPerformance.Rehydrate(
            model.ScholarshipCode ?? string.Empty,
            model.ProfileCode ?? string.Empty,
            model.FirstName ?? string.Empty,
            model.LastName ?? string.Empty,
            model.ScholarshipType ?? string.Empty,
            model.RequiredHours ?? 0,
            model.CompletedHours ?? 0,
            model.RemainingHours ?? 0,
            model.CompletionPercentage ?? 0,
            model.ContractState ?? string.Empty,
            model.EndDate);
    }

    public static DomainEntities.Profile.ProgramAnalytics ToDomain(PersistenceModels.Profile.MvProgramAnalytic model)
    {
        return DomainEntities.Profile.ProgramAnalytics.Rehydrate(
            model.ProgramCode ?? string.Empty,
            model.ProgramName ?? string.Empty,
            (int)(model.TotalActivities ?? 0),
            (int)(model.TotalUniqueVolunteers ?? 0),
            model.TotalGeneratedHours ?? 0);
    }

    public static DomainEntities.Profile.ActivityAnalytics ToDomain(PersistenceModels.Profile.MvActivityAnalytic model)
    {
        return DomainEntities.Profile.ActivityAnalytics.Rehydrate(
            model.ActivityCode ?? string.Empty,
            model.ProgramCode ?? string.Empty,
            model.ProgramName ?? string.Empty,
            model.ActivityName ?? string.Empty,
            model.StartDate ?? DateTime.MinValue,
            model.EndDate ?? DateTime.MinValue,
            model.TotalCapacity ?? 0,
            (int)(model.TotalEnrolled ?? 0),
            (int)(model.TotalAttended ?? 0),
            model.TotalActivityHours ?? 0);
    }

    public static DomainEntities.Profile.VolunteerHistory ToDomain(PersistenceModels.Profile.MvVolunteerHistory model)
    {
        return DomainEntities.Profile.VolunteerHistory.Rehydrate(
            model.ProfileCode ?? string.Empty,
            model.FirstName ?? string.Empty,
            model.LastName ?? string.Empty,
            model.CareerName,
            model.PersonalGoalHours ?? 0,
            (int)(model.TotalActivitiesParticipated ?? 0),
            model.ValidatedHours ?? 0,
            model.TotalLoggedHours ?? 0,
            model.LastActivityDate);
    }

    public static PersistenceModels.Profile.Profile ToPersistence(DomainEntities.Profile.Profile entity)
    {
        return new PersistenceModels.Profile.Profile
        {
            UvaCode = entity.UvaCode,
            IdentityUserId = entity.IdentityUserId,
            FirstName = entity.FirstName,
            LastName = entity.LastName,
            Email = entity.Email,
            PhotoUrl = entity.PhotoUrl,
            CareerCode = entity.CareerCode,
            AddressLocation = entity.AddressLocation,
            Phone = entity.Phone,
            PersonalGoalHours = entity.PersonalGoalHours,
            StateCode = entity.StateCode,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt,
            DeletedAt = entity.DeletedAt,
        };
    }
}
