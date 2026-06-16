namespace U_VoluntApp_Backend.Src.Domain.Entities.VolProgram;

public class ActivityRecurrencePattern
{
    public string UvaCode { get; private set; } = string.Empty;

    public string ProgramCode { get; private set; } = string.Empty;

    public string Name { get; private set; } = string.Empty;

    public string RecurrenceType { get; private set; } = string.Empty;

    public string StateCode { get; private set; } = string.Empty;

    public DateTime CreatedAt { get; private set; }

    public DateTime? UpdatedAt { get; private set; }

    public DateTime? DeletedAt { get; private set; }

    public static ActivityRecurrencePattern Create(string programCode, string name, string recurrenceType, string stateCode, DateTime nowUtc)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new InvalidOperationException("El nombre del patron de recurrencia es obligatorio");
        }

        if (string.IsNullOrWhiteSpace(recurrenceType))
        {
            throw new InvalidOperationException("El tipo de recurrencia es obligatorio");
        }

        if (string.IsNullOrWhiteSpace(programCode))
        {
            throw new InvalidOperationException("El ID del programa es invalido");
        }

        if (string.IsNullOrWhiteSpace(stateCode) || !stateCode.StartsWith("stage-"))
        {
            throw new InvalidOperationException("El formato del código de estado es inválido");
        }

        return new ActivityRecurrencePattern
        {
            UvaCode = Guid.NewGuid().ToString(),
            ProgramCode = programCode,
            Name = name,
            RecurrenceType = recurrenceType,
            StateCode = stateCode,
            CreatedAt = nowUtc
        };
    }

    public void ApplyUpdate(string name, string recurrenceType, DateTime nowUtc)
    {
        if (DeletedAt.HasValue)
        {
            throw new InvalidOperationException("No se pueden aplicar cambios a un patrón de recurrencia eliminado");
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            throw new InvalidOperationException("El nombre del patrón de recurrencia no puede ser vacío");
        }

        if (string.IsNullOrWhiteSpace(recurrenceType))
        {
            throw new InvalidOperationException("El tipo de recurrencia no puede ser vacío");
        }

        bool updated = false;
        Name = UpdateIfNotNull(Name, name, ref updated) ?? Name;
        RecurrenceType = UpdateIfNotNull(RecurrenceType, recurrenceType, ref updated) ?? RecurrenceType;

        if (!updated)
        {
            throw new InvalidOperationException("No se han proporcionado cambios para aplicar al patrón de recurrencia");
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
            throw new InvalidOperationException("El patrón de recurrencia ya se encuentra en el estado proporcionado");
        }

        StateCode = stateCode;
        UpdatedAt = nowUtc;
        DeletedAt = DeletedAt.HasValue ? null : DeletedAt;
    }

    public void SoftDelete(DateTime nowUtc, string stateCode)
    {
        if (string.IsNullOrWhiteSpace(stateCode) || !stateCode.StartsWith("stage-"))
        {
            throw new InvalidOperationException("El formato del código de estado es inválido");
        }

        if (DeletedAt.HasValue)
        {
            throw new InvalidOperationException("El patrón de recurrencia ya se encuentra eliminado");
        }

        StateCode = stateCode;
        DeletedAt = nowUtc;
    }

    internal static ActivityRecurrencePattern Rehydrate(
        string uvaCode,
        string programCode,
        string name,
        string recurrenceType,
        string stateCode,
        DateTime createdAt,
        DateTime? updatedAt,
        DateTime? deletedAt)
    {
        return new ActivityRecurrencePattern
        {
            UvaCode = uvaCode,
            ProgramCode = programCode,
            Name = name,
            RecurrenceType = recurrenceType,
            StateCode = stateCode,
            CreatedAt = createdAt,
            UpdatedAt = updatedAt,
            DeletedAt = deletedAt,
        };
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
