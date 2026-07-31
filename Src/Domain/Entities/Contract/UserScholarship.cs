namespace U_VoluntApp_Core.Src.Domain.Entities.Contract;

public class UserScholarship
{
    public string UvaCode { get; private set; } = string.Empty;

    public string AssignedProfileCode { get; private set; } = string.Empty;

    public string? EvaluatorProfileCode { get; private set; }

    public string ScholarshipTypeCode { get; private set; } = string.Empty;

    public string Reason { get; private set; } = string.Empty;

    public decimal RequiredHours { get; private set; }

    public DateTime StartDate { get; private set; }

    public DateTime? EndDate { get; private set; }

    public string StateCode { get; private set; } = string.Empty;

    public DateTime CreatedAt { get; private set; }

    public DateTime? UpdatedAt { get; private set; }

    public DateTime? DeletedAt { get; private set; }

    public static UserScholarship CreatePending(
        string assignedProfileCode,
        DateTime startDate,
        DateTime endDate,
        string scholarshipTypeCode,
        decimal requiredHours,
        string reason,
        string stateCode,
        DateTime nowUtc)
    {
        if (endDate < startDate)
        {
            throw new InvalidOperationException("La fecha de fin no puede ser anterior a la fecha de inicio");
        }

        if (string.IsNullOrWhiteSpace(reason))
        {
            throw new InvalidOperationException("La razón es obligatoria");
        }

        if (string.IsNullOrWhiteSpace(assignedProfileCode))
        {
            throw new InvalidOperationException("El identificador de perfil es obligatorio");
        }

        if (string.IsNullOrWhiteSpace(stateCode) || !stateCode.StartsWith("stage-"))
        {
            throw new InvalidOperationException("El formato del código de estado es inválido");
        }

        if (string.IsNullOrWhiteSpace(scholarshipTypeCode) || !scholarshipTypeCode.StartsWith("type-"))
        {
            throw new InvalidOperationException("El formato del código de tipo de beca es inválido");
        }

        if ((requiredHours < 50 || requiredHours > 100) && requiredHours != 0)
        {
            throw new InvalidOperationException("Las horas requeridas deben estar entre 50 y 100");
        }

        return new UserScholarship
        {
            UvaCode = Guid.NewGuid().ToString(),
            AssignedProfileCode = assignedProfileCode,
            Reason = reason,
            StartDate = startDate,
            EndDate = endDate,
            RequiredHours = requiredHours,
            ScholarshipTypeCode = scholarshipTypeCode,
            StateCode = stateCode,
            CreatedAt = nowUtc,
            UpdatedAt = nowUtc,
        };
    }

    public static UserScholarship CreateApproved(
        string assignedProfileCode,
        string evaluatorProfileCode,
        DateTime startDate,
        DateTime endDate,
        string scholarshipTypeCode,
        decimal requiredHours,
        string reason,
        string stateCode,
        DateTime nowUtc)
    {
        if (endDate < startDate)
        {
            throw new InvalidOperationException("La fecha de fin no puede ser anterior a la fecha de inicio");
        }

        if (string.IsNullOrWhiteSpace(reason))
        {
            throw new InvalidOperationException("La razón es obligatoria");
        }

        if (string.IsNullOrWhiteSpace(assignedProfileCode))
        {
            throw new InvalidOperationException("El identificador de perfil es obligatorio");
        }

        if (string.IsNullOrWhiteSpace(evaluatorProfileCode))
        {
            throw new InvalidOperationException("El identificador del evaluador es obligatorio");
        }

        if (string.IsNullOrWhiteSpace(scholarshipTypeCode) || !scholarshipTypeCode.StartsWith("type-"))
        {
            throw new InvalidOperationException("El formato del código de tipo de beca es inválido");
        }

        if (string.IsNullOrWhiteSpace(stateCode) || !stateCode.StartsWith("stage-"))
        {
            throw new InvalidOperationException("El formato del código de estado es inválido");
        }

        if ((requiredHours < 50 || requiredHours > 100) && requiredHours != 0)
        {
            throw new InvalidOperationException("Las horas requeridas deben estar entre 50 y 100");
        }

        return new UserScholarship
        {
            UvaCode = Guid.NewGuid().ToString(),
            AssignedProfileCode = assignedProfileCode,
            Reason = reason,
            StartDate = startDate,
            EndDate = endDate,
            RequiredHours = requiredHours,
            EvaluatorProfileCode = evaluatorProfileCode,
            ScholarshipTypeCode = scholarshipTypeCode,
            StateCode = stateCode,
            CreatedAt = nowUtc,
            UpdatedAt = nowUtc,
        };
    }

    public void Review(string stateCode, string evaluatorProfileCode, decimal requiredHours, DateTime nowUtc)
    {
        if (DeletedAt.HasValue)
        {
            throw new InvalidOperationException("No se puede revisar una beca eliminada");
        }

        if (string.IsNullOrWhiteSpace(stateCode) || !stateCode.StartsWith("stage-"))
        {
            throw new InvalidOperationException("El formato del código de estado es inválido");
        }

        if (string.IsNullOrWhiteSpace(evaluatorProfileCode))
        {
            throw new InvalidOperationException("El identificador del evaluador es obligatorio");
        }

        if ((requiredHours < 50 || requiredHours > 100) && requiredHours != 0)
        {
            throw new InvalidOperationException("Las horas requeridas deben estar entre 50 y 100");
        }

        StateCode = stateCode;
        EvaluatorProfileCode = evaluatorProfileCode;
        RequiredHours = requiredHours;
        UpdatedAt = nowUtc;
    }

    public void Complete(DateTime endDate, string stateCode, DateTime nowUtc)
    {
        if (DeletedAt.HasValue)
        {
            throw new InvalidOperationException("No se puede completar una beca eliminada");
        }

        if (string.IsNullOrWhiteSpace(stateCode) || !stateCode.StartsWith("stage-"))
        {
            throw new InvalidOperationException("El formato del código de estado es inválido");
        }

        EndDate = endDate;
        StateCode = stateCode;
        UpdatedAt = nowUtc;
    }

    public void ChangeState(string stateCode, DateTime nowUtc)
    {
        if (string.IsNullOrWhiteSpace(stateCode) || !stateCode.StartsWith("stage-"))
        {
            throw new InvalidOperationException("El formato del código de estado es inválido");
        }

        if (stateCode == StateCode)
        {
            throw new InvalidOperationException("El estado de la beca es el mismo al que se desea cambiar");
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
            throw new InvalidOperationException("La beca ya se encuentra eliminada");
        }

        DeletedAt = nowUtc;
        StateCode = stateCode;
    }

    internal static UserScholarship Rehydrate(
        string uvaCode,
        string assignedProfileCode,
        string? evaluatorProfileCode,
        string scholarshipTypeCode,
        string reason,
        decimal requiredHours,
        DateTime startDate,
        DateTime? endDate,
        string stateCode,
        DateTime createdAt,
        DateTime? updatedAt,
        DateTime? deletedAt)
    {
        return new UserScholarship
        {
            UvaCode = uvaCode,
            AssignedProfileCode = assignedProfileCode,
            EvaluatorProfileCode = evaluatorProfileCode,
            ScholarshipTypeCode = scholarshipTypeCode,
            Reason = reason,
            RequiredHours = requiredHours,
            StartDate = startDate,
            EndDate = endDate,
            StateCode = stateCode,
            CreatedAt = createdAt,
            UpdatedAt = updatedAt,
            DeletedAt = deletedAt
        };
    }
}
