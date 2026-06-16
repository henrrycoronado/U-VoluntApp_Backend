namespace U_VoluntApp_Backend.Src.Infrastructure.Persistence.Mappers;

using DomainEntities = U_VoluntApp_Backend.Src.Domain.Entities;
using PersistenceModels = U_VoluntApp_Backend.Src.Infrastructure.Persistence.Models;

public static partial class DomainPersistenceMapper
{
    public static DomainEntities.Tracking.TrackingLog ToDomain(PersistenceModels.Tracking.TrackingLog model)
    {
        return DomainEntities.Tracking.TrackingLog.Rehydrate(
            model.UvaCode,
            model.EnrollmentCode,
            model.GroupEnrollmentCode,
            model.EntryTime,
            model.ExitTime,
            model.CalculatedHours,
            model.StateCode,
            model.CreatedAt,
            model.UpdatedAt,
            model.DeletedAt,
            model.CheckInRegisteredByCode,
            model.CheckOutRegisteredByCode);
    }

    public static DomainEntities.Tracking.Evidence ToDomain(PersistenceModels.Tracking.Evidence model)
    {
        return DomainEntities.Tracking.Evidence.Rehydrate(
            model.UvaCode,
            model.TrackingLogCode,
            model.EvidenceTypeCode,
            model.TypeCode,
            model.PhotoUrl,
            model.Observations,
            model.LocationLatitude,
            model.LocationLongitude,
            model.CreatedAt,
            model.UpdatedAt,
            model.DeletedAt);
    }

    public static PersistenceModels.Tracking.TrackingLog ToPersistence(DomainEntities.Tracking.TrackingLog entity)
    {
        return new PersistenceModels.Tracking.TrackingLog
        {
            UvaCode = entity.UvaCode,
            EnrollmentCode = entity.EnrollmentCode,
            GroupEnrollmentCode = entity.GroupEnrollmentCode,
            EntryTime = entity.EntryTime,
            ExitTime = entity.ExitTime,
            CalculatedHours = entity.CalculatedHours,
            StateCode = entity.StateCode,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt,
            DeletedAt = entity.DeletedAt,
            CheckInRegisteredByCode = entity.CheckInRegisteredByCode,
            CheckOutRegisteredByCode = entity.CheckOutRegisteredByCode,
        };
    }

    public static PersistenceModels.Tracking.Evidence ToPersistence(DomainEntities.Tracking.Evidence entity)
    {
        return new PersistenceModels.Tracking.Evidence
        {
            UvaCode = entity.UvaCode,
            TrackingLogCode = entity.TrackingLogCode,
            PhotoUrl = entity.PhotoUrl,
            EvidenceTypeCode = entity.EvidenceTypeCode,
            TypeCode = entity.TypeCode,
            Observations = entity.Observations,
            LocationLatitude = entity.LocationLatitude,
            LocationLongitude = entity.LocationLongitude,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt,
            DeletedAt = entity.DeletedAt,
        };
    }
}
