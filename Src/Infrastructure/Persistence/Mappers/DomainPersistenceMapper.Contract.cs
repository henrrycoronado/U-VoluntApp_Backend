namespace U_VoluntApp_Core.Src.Infrastructure.Persistence.Mappers;

using DomainEntities = U_VoluntApp_Core.Src.Domain.Entities;
using PersistenceModels = U_VoluntApp_Core.Src.Infrastructure.Persistence.Models;

public static partial class DomainPersistenceMapper
{
    public static DomainEntities.Contract.UserScholarship ToDomain(PersistenceModels.Contract.UserScholarship model)
    {
        return DomainEntities.Contract.UserScholarship.Rehydrate(
            model.UvaCode,
            model.AssignedProfileCode,
            model.EvaluatorProfileCode,
            model.ScholarshipTypeCode,
            model.Reason,
            model.RequiredHours,
            model.StartDate,
            model.EndDate,
            model.StateCode,
            model.CreatedAt,
            model.UpdatedAt,
            model.DeletedAt);
    }

    public static DomainEntities.Contract.RoleRequest ToDomain(PersistenceModels.Contract.RoleRequest model)
    {
        return DomainEntities.Contract.RoleRequest.Rehydrate(
            model.UvaCode,
            model.RequesterProfileCode,
            model.RequestedRoleId,
            model.Reason,
            model.DurationInMonths,
            model.StateCode,
            model.ResolvedByProfileCode,
            model.CreatedAt,
            model.UpdatedAt,
            model.DeletedAt,
            model.ResolvedAt);
    }

    public static PersistenceModels.Contract.UserScholarship ToPersistence(DomainEntities.Contract.UserScholarship entity)
    {
        return new PersistenceModels.Contract.UserScholarship
        {
            UvaCode = entity.UvaCode,
            AssignedProfileCode = entity.AssignedProfileCode,
            EvaluatorProfileCode = entity.EvaluatorProfileCode,
            ScholarshipTypeCode = entity.ScholarshipTypeCode,
            Reason = entity.Reason,
            RequiredHours = entity.RequiredHours,
            StartDate = entity.StartDate,
            EndDate = entity.EndDate,
            StateCode = entity.StateCode,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt,
            DeletedAt = entity.DeletedAt,
        };
    }

    public static PersistenceModels.Contract.RoleRequest ToPersistence(DomainEntities.Contract.RoleRequest entity)
    {
        return new PersistenceModels.Contract.RoleRequest
        {
            UvaCode = entity.UvaCode,
            RequesterProfileCode = entity.RequesterProfileCode,
            RequestedRoleId = entity.RequestedRoleCode,
            Reason = entity.Reason,
            DurationInMonths = entity.DurationInMonths,
            StateCode = entity.StateCode,
            ResolvedByProfileCode = entity.ResolvedByProfileCode,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt,
            DeletedAt = entity.DeletedAt,
            ResolvedAt = entity.ResolvedAt,
        };
    }
}
