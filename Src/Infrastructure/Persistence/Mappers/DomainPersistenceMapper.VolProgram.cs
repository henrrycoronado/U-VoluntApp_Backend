namespace U_VoluntApp_Core.Src.Infrastructure.Persistence.Mappers;

using DomainEntities = U_VoluntApp_Core.Src.Domain.Entities;
using PersistenceModels = U_VoluntApp_Core.Src.Infrastructure.Persistence.Models;

public static partial class DomainPersistenceMapper
{
    public static DomainEntities.VolProgram.VolProgram ToDomain(PersistenceModels.VolProgram.VolProgram model)
    {
        var content = model.ProgramContent != null ? ToDomain(model.ProgramContent) : null;
        return DomainEntities.VolProgram.VolProgram.Rehydrate(
            model.UvaCode,
            model.Name,
            model.Acronym,
            model.ManagerProfileCode,
            model.StateCode,
            model.CreatedAt,
            model.UpdatedAt,
            model.DeletedAt,
            content);
    }

    public static DomainEntities.VolProgram.ProgramContent ToDomain(PersistenceModels.VolProgram.ProgramContent model)
    {
        return DomainEntities.VolProgram.ProgramContent.Rehydrate(
            model.UvaCode,
            model.ProgramCode,
            model.Description,
            model.ActivitiesDescription,
            model.ScheduleInfo,
            model.LeadershipInfo,
            model.ContactInfo,
            model.MissionStatement,
            model.ProfilePhotoUrl,
            model.CoverPhotoUrl,
            model.CreatedAt,
            model.UpdatedAt);
    }

    public static DomainEntities.VolProgram.ProgramCollaborator ToDomain(PersistenceModels.VolProgram.ProgramCollaborator model)
    {
        return DomainEntities.VolProgram.ProgramCollaborator.Rehydrate(
            model.UvaCode,
            model.ProgramCode,
            model.ProfileCode,
            model.AssignedByProfileCode,
            model.StateCode,
            model.CreatedAt,
            model.UpdatedAt,
            model.DeletedAt);
    }

    public static PersistenceModels.VolProgram.VolProgram ToPersistence(DomainEntities.VolProgram.VolProgram entity)
    {
        return new PersistenceModels.VolProgram.VolProgram
        {
            UvaCode = entity.UvaCode,
            Name = entity.Name,
            Acronym = entity.Acronym,
            ManagerProfileCode = entity.ManagerProfileCode,
            StateCode = entity.StateCode,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt,
            DeletedAt = entity.DeletedAt,
        };
    }

    public static PersistenceModels.VolProgram.ProgramContent ToPersistence(DomainEntities.VolProgram.ProgramContent entity)
    {
        return new PersistenceModels.VolProgram.ProgramContent
        {
            UvaCode = entity.UvaCode,
            ProgramCode = entity.ProgramCode,
            Description = entity.Description,
            ActivitiesDescription = entity.ActivitiesDescription,
            ScheduleInfo = entity.ScheduleInfo,
            LeadershipInfo = entity.LeadershipInfo,
            ContactInfo = entity.ContactInfo,
            MissionStatement = entity.MissionStatement,
            ProfilePhotoUrl = entity.ProfilePhotoUrl,
            CoverPhotoUrl = entity.CoverPhotoUrl,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt,
        };
    }

    public static PersistenceModels.VolProgram.ProgramCollaborator ToPersistence(DomainEntities.VolProgram.ProgramCollaborator entity)
    {
        return new PersistenceModels.VolProgram.ProgramCollaborator
        {
            UvaCode = entity.UvaCode,
            ProgramCode = entity.ProgramCode,
            ProfileCode = entity.ProfileCode,
            AssignedByProfileCode = entity.AssignedByProfileCode,
            StateCode = entity.StateCode,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt,
            DeletedAt = entity.DeletedAt,
        };
    }
}
