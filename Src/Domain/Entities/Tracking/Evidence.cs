namespace U_VoluntApp_Backend.Src.Domain.Entities.Tracking;

public class Evidence
{
    public string UvaCode { get; private set; } = string.Empty;

    public string TrackingLogCode { get; private set; } = string.Empty;

    public string EvidenceTypeCode { get; private set; } = string.Empty;

    public string TypeCode { get; private set; } = string.Empty;

    public string PhotoUrl { get; private set; } = string.Empty;

    public string? Observations { get; private set; }

    public double LocationLatitude { get; private set; }

    public double LocationLongitude { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public DateTime? UpdatedAt { get; private set; }

    public DateTime? DeletedAt { get; private set; }

    public static Evidence Create(
        string trackingLogCode,
        string photoUrl,
        string evidenceTypeCode,
        string typeCode,
        double locationLatitude,
        double locationLongitude,
        DateTime nowUtc,
        string? observations = null)
    {
        if (string.IsNullOrWhiteSpace(trackingLogCode))
        {
            throw new InvalidOperationException("El identificador del registro de seguimiento no es valido");
        }

        if (string.IsNullOrWhiteSpace(photoUrl))
        {
            throw new InvalidOperationException("La URL de la foto es obligatoria");
        }

        if (string.IsNullOrWhiteSpace(evidenceTypeCode) || !evidenceTypeCode.StartsWith("type-"))
        {
            throw new InvalidOperationException("El formato del código de tipo de evidencia es inválido");
        }

        if (string.IsNullOrWhiteSpace(typeCode) || !typeCode.StartsWith("type-"))
        {
            throw new InvalidOperationException("El formato del código de tipo de tracking es inválido");
        }

        return new Evidence
        {
            UvaCode = Guid.NewGuid().ToString(),
            TrackingLogCode = trackingLogCode,
            PhotoUrl = photoUrl,
            EvidenceTypeCode = evidenceTypeCode,
            TypeCode = typeCode,
            Observations = observations,
            LocationLatitude = locationLatitude,
            LocationLongitude = locationLongitude,
            CreatedAt = nowUtc,
            UpdatedAt = nowUtc,
        };
    }

    public void SoftDelete(DateTime nowUtc)
    {
        if (DeletedAt.HasValue)
        {
            throw new InvalidOperationException("La evidencia ya se encuentra eliminada");
        }

        DeletedAt = nowUtc;
    }

    public void UpdateObservations(string? observations, DateTime nowUtc)
    {
        if (DeletedAt.HasValue)
        {
            throw new InvalidOperationException("No se puede actualizar una evidencia eliminada");
        }

        Observations = observations;
        UpdatedAt = nowUtc;
    }

    internal static Evidence Rehydrate(
        string uvaCode,
        string trackingLogCode,
        string evidenceTypeCode,
        string typeCode,
        string photoUrl,
        string? observations,
        double locationLatitude,
        double locationLongitude,
        DateTime createdAt,
        DateTime? updatedAt,
        DateTime? deletedAt)
    {
        return new Evidence
        {
            UvaCode = uvaCode,
            TrackingLogCode = trackingLogCode,
            EvidenceTypeCode = evidenceTypeCode,
            TypeCode = typeCode,
            PhotoUrl = photoUrl,
            Observations = observations,
            LocationLatitude = locationLatitude,
            LocationLongitude = locationLongitude,
            CreatedAt = createdAt,
            UpdatedAt = updatedAt,
            DeletedAt = deletedAt,
        };
    }
}
