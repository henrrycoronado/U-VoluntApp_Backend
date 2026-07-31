namespace U_VoluntApp_Core.Src.Domain.Entities.Enrollment;

public class Enrollment
{
    public string UvaCode { get; private set; } = string.Empty;

    public string ActivityCode { get; private set; } = string.Empty;

    public string EnrolledProfileCode { get; private set; } = string.Empty;

    public string StateCode { get; private set; } = string.Empty;

    public DateTime CreatedAt { get; private set; }

    public DateTime? UpdatedAt { get; private set; }

    public DateTime? DeletedAt { get; private set; }

    public static Enrollment Create(string activityCode, string enrolledProfileCode, string stateCode, DateTime nowUtc)
    {
        if (string.IsNullOrWhiteSpace(enrolledProfileCode))
        {
            throw new InvalidOperationException("El perfil no fue reconocido para realizar la inscripción");
        }

        if (string.IsNullOrWhiteSpace(activityCode))
        {
            throw new InvalidOperationException("El identificador de la actividad no es válido para realizar la inscripción");
        }

        if (string.IsNullOrWhiteSpace(stateCode) || !stateCode.StartsWith("stage-"))
        {
            throw new InvalidOperationException("El formato del código de estado es inválido");
        }

        return new Enrollment
        {
            UvaCode = Guid.NewGuid().ToString(),
            ActivityCode = activityCode,
            EnrolledProfileCode = enrolledProfileCode,
            StateCode = stateCode,
            CreatedAt = nowUtc
        };
    }

    public void ChangeState(string stateCode, DateTime nowUtc)
    {
        if (string.IsNullOrWhiteSpace(stateCode) || !stateCode.StartsWith("stage-"))
        {
            throw new InvalidOperationException("El formato del código de estado es inválido");
        }

        if (StateCode == stateCode)
        {
            throw new InvalidOperationException("El estado de la inscripción es el mismo que se desea asignar");
        }

        StateCode = stateCode;
        UpdatedAt = nowUtc;
        DeletedAt = DeletedAt.HasValue ? null : DeletedAt;
    }

    public void SoftDelete(string stateCode, DateTime nowUtc)
    {
        if (string.IsNullOrWhiteSpace(stateCode) || !stateCode.StartsWith("stage-"))
        {
            throw new InvalidOperationException("El formato del código de estado es inválido");
        }

        if (DeletedAt.HasValue)
        {
            throw new InvalidOperationException("La inscripción ya se encuentra eliminada");
        }

        StateCode = stateCode;
        DeletedAt = nowUtc;
    }

    internal static Enrollment Rehydrate(
        string uvaCode,
        string activityCode,
        string enrolledProfileCode,
        string stateCode,
        DateTime createdAt,
        DateTime? updatedAt,
        DateTime? deletedAt)
    {
        return new Enrollment
        {
            UvaCode = uvaCode,
            ActivityCode = activityCode,
            EnrolledProfileCode = enrolledProfileCode,
            StateCode = stateCode,
            CreatedAt = createdAt,
            UpdatedAt = updatedAt,
            DeletedAt = deletedAt,
        };
    }
}
