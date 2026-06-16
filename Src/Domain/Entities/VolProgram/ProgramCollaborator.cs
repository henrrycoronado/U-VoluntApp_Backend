namespace U_VoluntApp_Backend.Src.Domain.Entities.VolProgram;

public class ProgramCollaborator
{
    public string UvaCode { get; private set; } = string.Empty;

    public string ProgramCode { get; private set; } = string.Empty;

    public string ProfileCode { get; private set; } = string.Empty;

    public string? AssignedByProfileCode { get; private set; }

    public string StateCode { get; private set; } = string.Empty;

    public DateTime CreatedAt { get; private set; }

    public DateTime? UpdatedAt { get; private set; }

    public DateTime? DeletedAt { get; private set; }

    public static ProgramCollaborator Create(
        string programCode,
        string profileCode,
        string assignedByProfileCode,
        string stateCode,
        DateTime nowUtc)
    {
        if (string.IsNullOrWhiteSpace(profileCode))
        {
            throw new InvalidOperationException("El perfil es obligatorio");
        }

        if (string.IsNullOrWhiteSpace(programCode))
        {
            throw new InvalidOperationException("El identificador del programa es inválido");
        }

        if (string.IsNullOrWhiteSpace(assignedByProfileCode))
        {
            throw new InvalidOperationException("El identificador de quien asigna es obligatorio");
        }

        if (string.IsNullOrWhiteSpace(stateCode) || !stateCode.StartsWith("stage-"))
        {
            throw new InvalidOperationException("El formato del código de estado es inválido");
        }

        return new ProgramCollaborator
        {
            UvaCode = Guid.NewGuid().ToString(),
            ProgramCode = programCode,
            ProfileCode = profileCode,
            AssignedByProfileCode = assignedByProfileCode,
            StateCode = stateCode,
            CreatedAt = nowUtc,
            UpdatedAt = nowUtc,
        };
    }

    public void ChangeState(string stateCode, DateTime nowUtc)
    {
        if (DeletedAt.HasValue)
        {
            throw new InvalidOperationException("No se puede cambiar el estado de un colaborador eliminado");
        }

        if (string.IsNullOrWhiteSpace(stateCode) || !stateCode.StartsWith("stage-"))
        {
            throw new InvalidOperationException("El formato del código de estado es inválido");
        }

        if (StateCode == stateCode)
        {
            throw new InvalidOperationException("El colaborador ya se encuentra en el estado proporcionado");
        }

        StateCode = stateCode;
        UpdatedAt = nowUtc;
    }

    public void SoftDelete(DateTime nowUtc, string stateCode)
    {
        if (string.IsNullOrWhiteSpace(stateCode) || !stateCode.StartsWith("stage-"))
        {
            throw new InvalidOperationException("El formato del código de estado es inválido");
        }

        if (DeletedAt.HasValue)
        {
            throw new InvalidOperationException("El colaborador ya se encuentra eliminado");
        }

        StateCode = stateCode;
        DeletedAt = nowUtc;
    }

    public void ResolveRequest(DateTime nowUtc, string stateCode, string assignedByProfileCode)
    {
        if (DeletedAt.HasValue)
        {
            throw new InvalidOperationException("No se puede resolver una solicitud de un colaborador eliminado");
        }

        if (string.IsNullOrWhiteSpace(stateCode) || !stateCode.StartsWith("stage-"))
        {
            throw new InvalidOperationException("El formato del código de estado es inválido");
        }

        AssignedByProfileCode = assignedByProfileCode;
        StateCode = stateCode;
        UpdatedAt = nowUtc;
    }

    internal static ProgramCollaborator Rehydrate(
        string uvaCode,
        string programCode,
        string profileCode,
        string? assignedByProfileCode,
        string stateCode,
        DateTime createdAt,
        DateTime? updatedAt,
        DateTime? deletedAt)
    {
        return new ProgramCollaborator
        {
            UvaCode = uvaCode,
            ProgramCode = programCode,
            ProfileCode = profileCode,
            AssignedByProfileCode = assignedByProfileCode,
            StateCode = stateCode,
            CreatedAt = createdAt,
            UpdatedAt = updatedAt,
            DeletedAt = deletedAt,
        };
    }
}
