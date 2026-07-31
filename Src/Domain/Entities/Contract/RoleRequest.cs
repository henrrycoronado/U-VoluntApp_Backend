namespace U_VoluntApp_Core.Src.Domain.Entities.Contract;

public class RoleRequest
{
    public string UvaCode { get; private set; } = string.Empty;

    public string RequesterProfileCode { get; private set; } = string.Empty;

    public string RequestedRoleCode { get; private set; } = string.Empty;

    public string Reason { get; private set; } = string.Empty;

    public int? DurationInMonths { get; private set; }

    public string StateCode { get; private set; } = string.Empty;

    public string? ResolvedByProfileCode { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public DateTime? UpdatedAt { get; private set; }

    public DateTime? DeletedAt { get; private set; }

    public DateTime? ResolvedAt { get; private set; }

    public static RoleRequest Create(
        string requesterProfileCode,
        string requestedRoleCode,
        string reason,
        int? durationInMonths,
        string pendingStateCode,
        DateTime nowUtc)
    {
        if (string.IsNullOrWhiteSpace(requesterProfileCode))
        {
            throw new InvalidOperationException("El identificador del perfil solicitante es requerido");
        }

        if (string.IsNullOrWhiteSpace(requestedRoleCode))
        {
            throw new InvalidOperationException("El identificador del rol solicitado es requerido");
        }

        if (string.IsNullOrWhiteSpace(reason))
        {
            throw new InvalidOperationException("El motivo es requerido");
        }

        if (string.IsNullOrWhiteSpace(pendingStateCode) || !pendingStateCode.StartsWith("stage-"))
        {
            throw new InvalidOperationException("El formato del código de estado es inválido");
        }

        if (durationInMonths.HasValue && durationInMonths < 0)
        {
            throw new InvalidOperationException("La duración en meses no es valida");
        }

        return new RoleRequest
        {
            UvaCode = Guid.NewGuid().ToString(),
            RequesterProfileCode = requesterProfileCode,
            RequestedRoleCode = requestedRoleCode,
            Reason = reason,
            DurationInMonths = durationInMonths,
            StateCode = pendingStateCode,
            CreatedAt = nowUtc,
            UpdatedAt = nowUtc,
        };
    }

    public void Approve(string resolvedByProfileCode, string approvedStateCode, DateTime nowUtc)
    {
        if (DeletedAt.HasValue)
        {
            throw new InvalidOperationException("No se puede responder a una solicitud eliminada");
        }

        if (string.IsNullOrWhiteSpace(approvedStateCode) || !approvedStateCode.StartsWith("stage-"))
        {
            throw new InvalidOperationException("El formato del código de estado es inválido");
        }

        if (string.IsNullOrWhiteSpace(resolvedByProfileCode))
        {
            throw new InvalidOperationException("El identificador del perfil que resuelve la solicitud es requerido para realizar la respuesta");
        }

        StateCode = approvedStateCode;
        ResolvedByProfileCode = resolvedByProfileCode;
        ResolvedAt = nowUtc;
        UpdatedAt = nowUtc;
    }

    public void Reject(string resolvedByProfileCode, string rejectedStateCode, DateTime nowUtc)
    {
        if (DeletedAt.HasValue)
        {
            throw new InvalidOperationException("No se puede responder a una solicitud eliminada");
        }

        if (string.IsNullOrWhiteSpace(rejectedStateCode) || !rejectedStateCode.StartsWith("stage-"))
        {
            throw new InvalidOperationException("El formato del código de estado es inválido");
        }

        if (string.IsNullOrWhiteSpace(resolvedByProfileCode))
        {
            throw new InvalidOperationException("El identificador del perfil que resuelve la solicitud es requerido para realizar la respuesta");
        }

        StateCode = rejectedStateCode;
        ResolvedByProfileCode = resolvedByProfileCode;
        ResolvedAt = nowUtc;
        UpdatedAt = nowUtc;
    }

    public void SoftDelete(string stateCode, DateTime nowUtc)
    {
        if (string.IsNullOrWhiteSpace(stateCode) || !stateCode.StartsWith("stage-"))
        {
            throw new InvalidOperationException("El formato del código de estado es inválido");
        }

        if (DeletedAt.HasValue)
        {
            throw new InvalidOperationException("La solicitud ya se encuentra eliminada");
        }

        DeletedAt = nowUtc;
        StateCode = stateCode;
    }

    internal static RoleRequest Rehydrate(
        string uvaCode,
        string requesterProfileCode,
        string requestedRoleCode,
        string reason,
        int? durationInMonths,
        string stateCode,
        string? resolvedByProfileCode,
        DateTime createdAt,
        DateTime? updatedAt,
        DateTime? deletedAt,
        DateTime? resolvedAt)
    {
        return new RoleRequest
        {
            UvaCode = uvaCode,
            RequesterProfileCode = requesterProfileCode,
            RequestedRoleCode = requestedRoleCode,
            Reason = reason,
            DurationInMonths = durationInMonths,
            StateCode = stateCode,
            ResolvedByProfileCode = resolvedByProfileCode,
            CreatedAt = createdAt,
            UpdatedAt = updatedAt,
            DeletedAt = deletedAt,
            ResolvedAt = resolvedAt,
        };
    }
}
