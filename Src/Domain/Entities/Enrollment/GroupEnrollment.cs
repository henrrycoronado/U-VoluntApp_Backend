namespace U_VoluntApp_Backend.Src.Domain.Entities.Enrollment;

public class GroupEnrollment
{
    public string UvaCode { get; private set; } = string.Empty;

    public string ActivityGroupCode { get; private set; } = string.Empty;

    public string EnrollmentCode { get; private set; } = string.Empty;

    public string StateCode { get; private set; } = string.Empty;

    public DateTime CreatedAt { get; private set; }

    public DateTime? UpdatedAt { get; private set; }

    public DateTime? DeletedAt { get; private set; }

    public static GroupEnrollment Create(string activityGroupCode, string enrollmentCode, string stateCode, DateTime nowUtc)
    {
        if (string.IsNullOrWhiteSpace(activityGroupCode))
        {
            throw new InvalidOperationException("El identificador del grupo de actividad no es valido");
        }

        if (string.IsNullOrWhiteSpace(enrollmentCode))
        {
            throw new InvalidOperationException("El identificador de la inscripcion no es valido");
        }

        if (string.IsNullOrWhiteSpace(stateCode) || !stateCode.StartsWith("stage-"))
        {
            throw new InvalidOperationException("El formato del código de estado es inválido");
        }

        return new GroupEnrollment
        {
            UvaCode = Guid.NewGuid().ToString(),
            ActivityGroupCode = activityGroupCode,
            EnrollmentCode = enrollmentCode,
            StateCode = stateCode,
            CreatedAt = nowUtc,
            UpdatedAt = nowUtc
        };
    }

    public void ChangeState(string stateCode, DateTime nowUtc)
    {
        if (stateCode == StateCode)
        {
            throw new InvalidOperationException("El estado es el mismo que el actual");
        }

        if (string.IsNullOrWhiteSpace(stateCode) || !stateCode.StartsWith("stage-"))
        {
            throw new InvalidOperationException("El formato del código de estado es inválido");
        }

        StateCode = stateCode;
        UpdatedAt = nowUtc;
        DeletedAt = null;
    }

    public void SoftDelete(string stateCode, DateTime nowUtc)
    {
        if (DeletedAt.HasValue)
        {
            throw new InvalidOperationException("La asociacion ya se encuentra eliminada");
        }

        if (string.IsNullOrWhiteSpace(stateCode) || !stateCode.StartsWith("stage-"))
        {
            throw new InvalidOperationException("El formato del código de estado es inválido");
        }

        StateCode = stateCode;
        DeletedAt = nowUtc;
    }

    internal static GroupEnrollment Rehydrate(
        string uvaCode,
        string activityGroupCode,
        string enrollmentCode,
        string stateCode,
        DateTime createdAt,
        DateTime? updatedAt,
        DateTime? deletedAt)
    {
        return new GroupEnrollment
        {
            UvaCode = uvaCode,
            ActivityGroupCode = activityGroupCode,
            EnrollmentCode = enrollmentCode,
            StateCode = stateCode,
            CreatedAt = createdAt,
            UpdatedAt = updatedAt,
            DeletedAt = deletedAt,
        };
    }
}
