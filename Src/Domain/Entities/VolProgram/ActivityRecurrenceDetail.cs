namespace U_VoluntApp_Backend.Src.Domain.Entities.VolProgram;

public class ActivityRecurrenceDetail
{
    public string UvaCode { get; private set; } = string.Empty;

    public string ActivityRecurrencePatternCode { get; private set; } = string.Empty;

    public short? DayOfWeek { get; private set; }

    public short? DayOfMonth { get; private set; }

    public short? WeekOfMonth { get; private set; }

    public TimeOnly? StartHour { get; private set; }

    public TimeOnly? EndHour { get; private set; }

    public string StateCode { get; private set; } = string.Empty;

    public DateTime CreatedAt { get; private set; }

    public DateTime? UpdatedAt { get; private set; }

    public DateTime? DeletedAt { get; private set; }

    public static ActivityRecurrenceDetail Create(
        string patternCode,
        short? dayOfWeek,
        short? dayOfMonth,
        short? weekOfMonth,
        TimeOnly? startHour,
        TimeOnly? endHour,
        string stateCode,
        DateTime nowUtc)
    {
        if (string.IsNullOrWhiteSpace(patternCode))
        {
            throw new InvalidOperationException("El patron de referencia es invalido");
        }

        if (string.IsNullOrWhiteSpace(stateCode) || !stateCode.StartsWith("stage-"))
        {
            throw new InvalidOperationException("El formato del código de estado es inválido");
        }

        ValidateInputs(dayOfWeek, dayOfMonth, weekOfMonth, startHour, endHour);

        return new ActivityRecurrenceDetail
        {
            UvaCode = Guid.NewGuid().ToString(),
            ActivityRecurrencePatternCode = patternCode,
            DayOfWeek = dayOfWeek,
            DayOfMonth = dayOfMonth,
            WeekOfMonth = weekOfMonth,
            StartHour = startHour,
            EndHour = endHour,
            CreatedAt = nowUtc,
            StateCode = stateCode
        };
    }

    public void ApplyUpdate(
        short? dayOfWeek,
        short? dayOfMonth,
        short? weekOfMonth,
        TimeOnly? startHour,
        TimeOnly? endHour,
        DateTime nowUtc)
    {
        if (DeletedAt.HasValue)
        {
            throw new InvalidOperationException("No se puede actualizar un patron de recurrencia eliminado");
        }

        ValidateInputs(dayOfWeek, dayOfMonth, weekOfMonth, startHour, endHour);

        bool updated = false;

        DayOfWeek = UpdateIfNotNull(DayOfWeek, dayOfWeek, ref updated);
        DayOfMonth = UpdateIfNotNull(DayOfMonth, dayOfMonth, ref updated);
        WeekOfMonth = UpdateIfNotNull(WeekOfMonth, weekOfMonth, ref updated);
        StartHour = UpdateIfNotNull(StartHour, startHour, ref updated);
        EndHour = UpdateIfNotNull(EndHour, endHour, ref updated);

        if (!updated)
        {
            throw new InvalidOperationException("No se han proporcionado cambios para aplicar al detalle de recurrencia");
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
            throw new InvalidOperationException("El detalle de recurrencia ya se encuentra en el estado proporcionado");
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
            throw new InvalidOperationException("El detalle de recurrencia ya se encuentra eliminado");
        }

        StateCode = stateCode;
        DeletedAt = nowUtc;
    }

    internal static ActivityRecurrenceDetail Rehydrate(
        string uvaCode,
        string patternCode,
        short? dayOfWeek,
        short? dayOfMonth,
        short? weekOfMonth,
        TimeOnly? startHour,
        TimeOnly? endHour,
        string stateCode,
        DateTime createdAt,
        DateTime? updatedAt,
        DateTime? deletedAt)
    {
        return new ActivityRecurrenceDetail
        {
            UvaCode = uvaCode,
            ActivityRecurrencePatternCode = patternCode,
            DayOfWeek = dayOfWeek,
            DayOfMonth = dayOfMonth,
            WeekOfMonth = weekOfMonth,
            StartHour = startHour,
            EndHour = endHour,
            StateCode = stateCode,
            CreatedAt = createdAt,
            UpdatedAt = updatedAt,
            DeletedAt = deletedAt
        };
    }

    private static void ValidateInputs(
        short? dayOfWeek,
        short? dayOfMonth,
        short? weekOfMonth,
        TimeOnly? startHour,
        TimeOnly? endHour)
    {
        var dayParametersCount = (dayOfWeek.HasValue ? 1 : 0) + (dayOfMonth.HasValue ? 1 : 0) + (weekOfMonth.HasValue ? 1 : 0);

        if (dayParametersCount == 0)
        {
            throw new InvalidOperationException("Debe especificar al menos un parametro de dia");
        }

        if (dayParametersCount > 1)
        {
            throw new InvalidOperationException("Solo se acepta un registro por detalle");
        }

        if (dayOfWeek.HasValue && (dayOfWeek < 0 || dayOfWeek > 6))
        {
            throw new InvalidOperationException("Entrada de dia de la semana no soportado");
        }

        if (dayOfMonth.HasValue && (dayOfMonth < 1 || dayOfMonth > 31))
        {
            throw new InvalidOperationException("Entrada de dia del mes no soportado, debe estar entre 1 y 31");
        }

        if (weekOfMonth.HasValue && (weekOfMonth < 1 || weekOfMonth > 5))
        {
            throw new InvalidOperationException("Entrada de semana del mes no soportado, debe estar entre 1 y 5");
        }

        if (startHour.HasValue && endHour.HasValue && startHour.Value >= endHour.Value)
        {
            throw new InvalidOperationException("La hora de inicio debe ser anterior a la hora de fin");
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
