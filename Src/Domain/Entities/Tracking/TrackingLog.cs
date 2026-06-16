namespace U_VoluntApp_Backend.Src.Domain.Entities.Tracking;

public class TrackingLog
{
    public string UvaCode { get; private set; } = string.Empty;

    public string EnrollmentCode { get; private set; } = string.Empty;

    public string? GroupEnrollmentCode { get; private set; }

    public DateTime? EntryTime { get; private set; }

    public DateTime? ExitTime { get; private set; }

    public decimal CalculatedHours { get; private set; }

    public string StateCode { get; private set; } = string.Empty;

    public DateTime CreatedAt { get; private set; }

    public DateTime? UpdatedAt { get; private set; }

    public DateTime? DeletedAt { get; private set; }

    public string? CheckInRegisteredByCode { get; private set; }

    public string? CheckOutRegisteredByCode { get; private set; }

    public static TrackingLog Create(string enrollmentCode, string? groupEnrollmentCode, string stateCode, DateTime nowUtc)
    {
        if (string.IsNullOrWhiteSpace(enrollmentCode))
        {
            throw new InvalidOperationException("El identificador de inscripción no es valido");
        }

        if (groupEnrollmentCode != null && string.IsNullOrWhiteSpace(groupEnrollmentCode))
        {
            throw new InvalidOperationException("El identificador de inscripción grupal no es valido");
        }

        if (string.IsNullOrWhiteSpace(stateCode) || !stateCode.StartsWith("stage-"))
        {
            throw new InvalidOperationException("El formato del código de estado es inválido");
        }

        return new TrackingLog
        {
            UvaCode = Guid.NewGuid().ToString(),
            EnrollmentCode = enrollmentCode,
            GroupEnrollmentCode = groupEnrollmentCode,
            StateCode = stateCode,
            CreatedAt = nowUtc
        };
    }

    public void CheckIn(DateTime entryTime, DateTime startActivityTime, DateTime endActivityTime, string registerCode, DateTime nowUtc)
    {
        if (DeletedAt.HasValue)
        {
            throw new InvalidOperationException("No se puede registrar en un registro eliminado");
        }

        if (EntryTime.HasValue)
        {
            throw new InvalidOperationException("Ya hay una hora de entrada registrada");
        }

        if (entryTime < startActivityTime.AddMinutes(-15) || entryTime > endActivityTime.AddMinutes(15))
        {
            throw new InvalidOperationException("La hora de entrada debe estar dentro del rango permitido alrededor de la actividad");
        }

        if (string.IsNullOrWhiteSpace(registerCode))
        {
            throw new InvalidOperationException("El identificador del registrante no puede ser nulo");
        }

        entryTime = entryTime < startActivityTime ? startActivityTime : entryTime;
        entryTime = entryTime > endActivityTime ? endActivityTime : entryTime;

        EntryTime = entryTime;
        CheckInRegisteredByCode = registerCode;
        UpdatedAt = nowUtc;
    }

    public void CheckOut(DateTime exitTime, DateTime startActivityTime, DateTime endActivityTime, string registerCode, DateTime nowUtc)
    {
        if (DeletedAt.HasValue)
        {
            throw new InvalidOperationException("No se puede registrar en un registro eliminado");
        }

        if (!EntryTime.HasValue)
        {
            throw new InvalidOperationException("No hay una hora de entrada registrada para este registro");
        }

        if (ExitTime.HasValue)
        {
            throw new InvalidOperationException("Ya hay una hora de salida registrada");
        }

        if (exitTime < EntryTime.Value)
        {
            throw new InvalidOperationException("La hora de salida no puede ser anterior a la hora de entrada");
        }

        if (exitTime < startActivityTime.AddMinutes(-15) || exitTime > endActivityTime.AddMinutes(15))
        {
            throw new InvalidOperationException("La hora de salida debe estar dentro del rango permitido alrededor de la actividad");
        }

        if (string.IsNullOrWhiteSpace(registerCode))
        {
            throw new InvalidOperationException("El identificador del registrante no puede ser nulo");
        }

        exitTime = exitTime < startActivityTime ? startActivityTime : exitTime;
        exitTime = exitTime > endActivityTime ? endActivityTime : exitTime;
        ExitTime = exitTime;
        CheckOutRegisteredByCode = registerCode;
        UpdatedAt = nowUtc;
        CalculatedHours = (decimal)(exitTime - EntryTime.Value).TotalHours;
    }

    public void UpdateHours(DateTime entryTime, DateTime exitTime, DateTime startActivity, DateTime endActivity, string registerCode, DateTime nowUtc)
    {
        if (DeletedAt.HasValue)
        {
            throw new InvalidOperationException("No se puede actualizar un registro eliminado");
        }

        if (!EntryTime.HasValue || !ExitTime.HasValue)
        {
            throw new InvalidOperationException("No hay horas de entrada y salida registradas para este registro");
        }

        if (entryTime < startActivity.AddMinutes(-15) || entryTime > endActivity.AddMinutes(15))
        {
            throw new InvalidOperationException("La hora de entrada debe estar dentro del rango permitido alrededor de la actividad");
        }

        if (exitTime < startActivity.AddMinutes(-15) || exitTime > endActivity.AddMinutes(15))
        {
            throw new InvalidOperationException("La hora de salida debe estar dentro del rango permitido alrededor de la actividad");
        }

        if (exitTime < entryTime)
        {
            throw new InvalidOperationException("La hora de salida no puede ser anterior a la hora de entrada");
        }

        if (string.IsNullOrWhiteSpace(registerCode))
        {
            throw new InvalidOperationException("El identificador del registrante no puede ser nulo");
        }

        entryTime = entryTime < startActivity ? startActivity : entryTime;
        entryTime = entryTime > endActivity ? endActivity : entryTime;
        exitTime = exitTime < startActivity ? startActivity : exitTime;
        exitTime = exitTime > endActivity ? endActivity : exitTime;

        EntryTime = entryTime;
        ExitTime = exitTime;
        CheckInRegisteredByCode = registerCode;
        CheckOutRegisteredByCode = registerCode;
        UpdatedAt = nowUtc;
        CalculatedHours = (decimal)(exitTime - entryTime).TotalHours;
    }

    public void ChangeState(string stateCode, DateTime nowUtc)
    {
        if (string.IsNullOrWhiteSpace(stateCode) || !stateCode.StartsWith("stage-"))
        {
            throw new InvalidOperationException("El formato del código de estado es inválido");
        }

        if (stateCode == StateCode)
        {
            throw new InvalidOperationException("El estado es el mismo al que se desea cambiar");
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
            throw new InvalidOperationException("El registro ya se encuentra eliminado");
        }

        DeletedAt = nowUtc;
        StateCode = stateCode;
    }

    internal static TrackingLog Rehydrate(
        string uvaCode,
        string enrollmentCode,
        string? groupEnrollmentCode,
        DateTime? entryTime,
        DateTime? exitTime,
        decimal calculatedHours,
        string stateCode,
        DateTime createdAt,
        DateTime? updatedAt,
        DateTime? deletedAt,
        string? checkInRegisteredByCode,
        string? checkOutRegisteredByCode)
    {
        return new TrackingLog
        {
            UvaCode = uvaCode,
            EnrollmentCode = enrollmentCode,
            GroupEnrollmentCode = groupEnrollmentCode,
            EntryTime = entryTime,
            ExitTime = exitTime,
            CalculatedHours = calculatedHours,
            StateCode = stateCode,
            CreatedAt = createdAt,
            UpdatedAt = updatedAt,
            DeletedAt = deletedAt,
            CheckInRegisteredByCode = checkInRegisteredByCode,
            CheckOutRegisteredByCode = checkOutRegisteredByCode,
        };
    }
}
