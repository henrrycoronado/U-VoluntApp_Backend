namespace U_VoluntApp_Backend.Src.Domain.Entities.Activity;

public class ActivityGroup
{
    public string UvaCode { get; private set; } = string.Empty;

    public string ActivityCode { get; private set; } = string.Empty;

    public string Name { get; private set; } = string.Empty;

    public string? Details { get; private set; }

    public int TotalCapacity { get; private set; }

    public DateTime? StartDate { get; private set; }

    public DateTime? EndDate { get; private set; }

    public string StateCode { get; private set; } = string.Empty;

    public DateTime CreatedAt { get; private set; }

    public DateTime? UpdatedAt { get; private set; }

    public DateTime? DeletedAt { get; private set; }

    public static ActivityGroup Create(
        string activityCode,
        string name,
        string stateCode,
        string? details,
        int totalCapacity,
        DateTime? startDate,
        DateTime? endDate,
        DateTime? activityStartDate,
        DateTime? activityEndDate,
        DateTime nowUtc)
    {
        if (string.IsNullOrWhiteSpace(activityCode))
        {
            throw new InvalidOperationException("Identificador de actividad inválido");
        }

        if (string.IsNullOrWhiteSpace(stateCode) || !stateCode.StartsWith("stage-"))
        {
            throw new InvalidOperationException("El formato del código de estado es inválido");
        }

        ValidateInputs(name, totalCapacity, startDate, endDate, activityStartDate, activityEndDate);

        return new ActivityGroup
        {
            UvaCode = Guid.NewGuid().ToString(),
            ActivityCode = activityCode,
            Name = name,
            Details = details,
            TotalCapacity = totalCapacity,
            StartDate = startDate,
            EndDate = endDate,
            StateCode = stateCode,
            CreatedAt = nowUtc
        };
    }

    public void ApplyUpdate(
        string name,
        string? details,
        int totalCapacity,
        DateTime? startDate,
        DateTime? endDate,
        DateTime? activityStartDate,
        DateTime? activityEndDate,
        DateTime nowUtc)
    {
        if (DeletedAt.HasValue)
        {
            throw new InvalidOperationException("No se pueden actualizar grupos eliminados");
        }

        ValidateInputs(name, totalCapacity, startDate, endDate, activityStartDate, activityEndDate);

        bool updated = false;
        Name = UpdateIfNotNull(Name, name, ref updated) ?? Name;
        Details = UpdateIfNotNull(Details, details, ref updated);
        TotalCapacity = UpdateIfNotNull(TotalCapacity, totalCapacity, ref updated);
        StartDate = UpdateIfNotNull(StartDate, startDate, ref updated);
        EndDate = UpdateIfNotNull(EndDate, endDate, ref updated);

        if (!updated)
        {
            throw new InvalidOperationException("No se han realizado cambios en la grupo");
        }

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
            throw new InvalidOperationException("El grupo ya se encuentra en el estado especificado");
        }

        StateCode = stateCode;
        UpdatedAt = nowUtc;
        DeletedAt = DeletedAt.HasValue ? null : DeletedAt;
    }

    public void SoftDelete(string stateCode, DateTime nowUtc)
    {
        if (DeletedAt.HasValue)
        {
            throw new InvalidOperationException("El grupo ya ha sido eliminado");
        }

        if (string.IsNullOrWhiteSpace(stateCode) || !stateCode.StartsWith("stage-"))
        {
            throw new InvalidOperationException("El formato del código de estado es inválido");
        }

        StateCode = stateCode;
        DeletedAt = nowUtc;
    }

    public bool HasCapacityAvailable(int currentEnrollmentCount)
    {
        if (TotalCapacity <= 0)
        {
            return true;
        }

        return currentEnrollmentCount < TotalCapacity;
    }

    internal static ActivityGroup Rehydrate(
        string uvaCode,
        string activityCode,
        string name,
        int totalCapacity,
        string? details,
        DateTime? startDate,
        DateTime? endDate,
        string stateCode,
        DateTime createdAt,
        DateTime? updatedAt,
        DateTime? deletedAt)
    {
        return new ActivityGroup
        {
            UvaCode = uvaCode,
            ActivityCode = activityCode,
            Name = name,
            Details = details,
            TotalCapacity = totalCapacity,
            StartDate = startDate,
            EndDate = endDate,
            StateCode = stateCode,
            CreatedAt = createdAt,
            UpdatedAt = updatedAt,
            DeletedAt = deletedAt,
        };
    }

    private static void ValidateInputs(string name, int totalCapacity, DateTime? startDate, DateTime? endDate, DateTime? startDateActivity, DateTime? endDateActivity)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new InvalidOperationException("El nombre del grupo es obligatorio");
        }

        if (totalCapacity <= 0)
        {
            throw new InvalidOperationException("La capacidad del grupo es invalida");
        }

        if (startDate.HasValue && endDate.HasValue && startDateActivity.HasValue && endDateActivity.HasValue)
        {
            if (startDate > endDate)
            {
                throw new InvalidOperationException("La fecha de inicio del grupo no puede ser posterior a la fecha de fin");
            }

            if (startDate < startDateActivity || endDate > endDateActivity)
            {
                throw new InvalidOperationException("Las fechas del grupo deben estar dentro del rango de fechas de la actividad");
            }
        }
    }

    private static T? UpdateIfNotNull<T>(T? currentValue, T? newValue, ref bool updated)
    {
        if (newValue != null && !newValue.Equals(currentValue))
        {
            updated = true;
            return newValue;
        }

        return currentValue;
    }
}
