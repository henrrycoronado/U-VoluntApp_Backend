namespace U_VoluntApp_Backend.Src.Infrastructure.Persistence.Mappers;

using DomainEntities = U_VoluntApp_Backend.Src.Domain.Entities;
using PersistenceModels = U_VoluntApp_Backend.Src.Infrastructure.Persistence.Models;

public static partial class DomainPersistenceMapper
{
    public static DomainEntities.Enrollment.Enrollment ToDomain(PersistenceModels.Enrollment.Enrollment model)
    {
        return DomainEntities.Enrollment.Enrollment.Rehydrate(
            model.UvaCode,
            model.ActivityCode,
            model.EnrolledProfileCode,
            model.StateCode,
            model.CreatedAt,
            model.UpdatedAt,
            model.DeletedAt);
    }

    public static DomainEntities.Enrollment.GroupEnrollment ToDomain(PersistenceModels.Enrollment.GroupEnrollment model)
    {
        return DomainEntities.Enrollment.GroupEnrollment.Rehydrate(
            model.UvaCode,
            model.ActivityGroupCode,
            model.EnrollmentCode,
            model.StateCode,
            model.CreatedAt,
            model.UpdatedAt,
            model.DeletedAt);
    }

    public static PersistenceModels.Enrollment.Enrollment ToPersistence(DomainEntities.Enrollment.Enrollment entity)
    {
        return new PersistenceModels.Enrollment.Enrollment
        {
            UvaCode = entity.UvaCode,
            ActivityCode = entity.ActivityCode,
            EnrolledProfileCode = entity.EnrolledProfileCode,
            StateCode = entity.StateCode,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt,
            DeletedAt = entity.DeletedAt,
        };
    }

    public static PersistenceModels.Enrollment.GroupEnrollment ToPersistence(DomainEntities.Enrollment.GroupEnrollment entity)
    {
        return new PersistenceModels.Enrollment.GroupEnrollment
        {
            UvaCode = entity.UvaCode,
            ActivityGroupCode = entity.ActivityGroupCode,
            EnrollmentCode = entity.EnrollmentCode,
            StateCode = entity.StateCode,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt,
            DeletedAt = entity.DeletedAt,
        };
    }
}
