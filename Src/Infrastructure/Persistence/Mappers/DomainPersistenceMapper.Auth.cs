namespace U_VoluntApp_Core.Src.Infrastructure.Persistence.Mappers;

using DomainEntities = U_VoluntApp_Core.Src.Domain.Entities;
using PersistenceModels = U_VoluntApp_Core.Src.Infrastructure.Persistence.Models;

public static partial class DomainPersistenceMapper
{
    public static DomainEntities.Auth.UserSecurityAudit ToDomain(PersistenceModels.Auth.UserSecurityAudit model)
    {
        return DomainEntities.Auth.UserSecurityAudit.Rehydrate(
            model.UvaCode,
            model.ProfileCode,
            model.LastIpAddress,
            model.DeviceFingerprint,
            model.LastCodeSentAt,
            model.IsTrusted,
            model.CreatedAt,
            model.UpdatedAt);
    }

    public static PersistenceModels.Auth.UserSecurityAudit ToPersistence(DomainEntities.Auth.UserSecurityAudit entity)
    {
        return new PersistenceModels.Auth.UserSecurityAudit
        {
            UvaCode = entity.UvaCode,
            ProfileCode = entity.ProfileCode,
            LastIpAddress = entity.LastIpAddress,
            DeviceFingerprint = entity.DeviceFingerprint,
            LastCodeSentAt = entity.LastCodeSentAt,
            IsTrusted = entity.IsTrusted,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt,
        };
    }
}
