namespace U_VoluntApp_Backend.Src.Domain.Entities.Activity;

public class Activity
{
    public string UvaCode { get; private set; } = string.Empty;

    public string ProgramCode { get; private set; } = string.Empty;

    public string? ResponsibleProfileCode { get; private set; }

    public string ActivityTypeCode { get; private set; } = string.Empty;

    public string? ActivityRecurrencePatternCode { get; private set; }

    public string Name { get; private set; } = string.Empty;

    public string? Description { get; private set; }

    public DateTime StartDate { get; private set; }

    public DateTime EndDate { get; private set; }

    public double LocationLatitude { get; private set; }

    public double LocationLongitude { get; private set; }

    public int RegistrationRadiusMeters { get; private set; }

    public string StateCode { get; private set; } = string.Empty;

    public DateTime CreatedAt { get; private set; }

    public DateTime? UpdatedAt { get; private set; }

    public DateTime? DeletedAt { get; private set; }

    public ActivityRule? Rule { get; private set; }

    public static Activity Create(
        string programCode,
        string? responsibleProfileCode,
        string activityTypeCode,
        string? activityRecurrencePatternCode,
        string name,
        string? description,
        DateTime startDate,
        DateTime endDate,
        int registrationRadiusMeters,
        string stateCode,
        double locationLatitude,
        double locationLongitude,
        DateTime nowUtc)
    {
        if (string.IsNullOrWhiteSpace(programCode))
        {
            throw new InvalidOperationException("Identificador de programa inválido");
        }

        if (string.IsNullOrWhiteSpace(stateCode) || !stateCode.StartsWith("stage-"))
        {
            throw new InvalidOperationException("El formato del código de estado es inválido");
        }

        if (string.IsNullOrWhiteSpace(activityTypeCode) || !activityTypeCode.StartsWith("type-"))
        {
            throw new InvalidOperationException("El formato del código de tipo de actividad es inválido");
        }

        ValidateInputs(activityTypeCode, activityRecurrencePatternCode, name, startDate, endDate, registrationRadiusMeters, nowUtc);

        return new Activity
        {
            UvaCode = Guid.NewGuid().ToString(),
            ProgramCode = programCode,
            ResponsibleProfileCode = responsibleProfileCode,
            ActivityTypeCode = activityTypeCode,
            ActivityRecurrencePatternCode = activityRecurrencePatternCode,
            Name = name,
            Description = description,
            StartDate = startDate,
            EndDate = endDate,
            LocationLatitude = locationLatitude,
            LocationLongitude = locationLongitude,
            RegistrationRadiusMeters = registrationRadiusMeters,
            StateCode = stateCode,
            CreatedAt = nowUtc
        };
    }

    public void ApplyUpdate(
        string? responsibleProfileCode,
        string activityTypeCode,
        string? activityRecurrencePatternCode,
        string name,
        string? description,
        DateTime startDate,
        DateTime endDate,
        double locationLatitude,
        double locationLongitude,
        int registrationRadiusMeters,
        DateTime nowUtc)
    {
        if (DeletedAt.HasValue)
        {
            throw new InvalidOperationException("No se puede actualizar una actividad eliminada");
        }

        if (string.IsNullOrWhiteSpace(activityTypeCode) || !activityTypeCode.StartsWith("type-"))
        {
            throw new InvalidOperationException("El formato del código de tipo de actividad es inválido");
        }

        ValidateInputs(activityTypeCode, activityRecurrencePatternCode, name, startDate, endDate, registrationRadiusMeters, nowUtc);

        bool updated = false;

        ResponsibleProfileCode = UpdateIfNotNull(ResponsibleProfileCode, responsibleProfileCode, ref updated);
        ActivityTypeCode = UpdateIfNotNull(ActivityTypeCode, activityTypeCode, ref updated) ?? ActivityTypeCode;
        ActivityRecurrencePatternCode = UpdateIfNotNull(ActivityRecurrencePatternCode, activityRecurrencePatternCode, ref updated);
        Name = UpdateIfNotNull(Name, name, ref updated) ?? Name;
        Description = UpdateIfNotNull(Description, description, ref updated);
        StartDate = UpdateIfNotNull(StartDate, startDate, ref updated);
        EndDate = UpdateIfNotNull(EndDate, endDate, ref updated);
        LocationLatitude = UpdateIfNotNull(LocationLatitude, locationLatitude, ref updated);
        LocationLongitude = UpdateIfNotNull(LocationLongitude, locationLongitude, ref updated);
        RegistrationRadiusMeters = UpdateIfNotNull(RegistrationRadiusMeters, registrationRadiusMeters, ref updated);

        if (!updated)
        {
            throw new InvalidOperationException("No se han realizado cambios en la actividad");
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
            throw new InvalidOperationException("El estado de la actividad es el mismo al que se desea cambiar");
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
            throw new InvalidOperationException("La actividad ya se encuentra eliminada");
        }

        DeletedAt = nowUtc;
        StateCode = stateCode;
    }

    public void AddRule(ActivityRule rule)
    {
        if (DeletedAt.HasValue)
        {
            throw new InvalidOperationException("No se puede agregar una regla a una actividad eliminada");
        }

        Rule = rule;
    }

    internal static Activity Rehydrate(
        string uvaCode,
        string programCode,
        string? responsibleProfileCode,
        string activityTypeCode,
        string? activityRecurrencePatternCode,
        string name,
        string? description,
        DateTime startDate,
        DateTime endDate,
        double locationLatitude,
        double locationLongitude,
        int registrationRadiusMeters,
        string stateCode,
        DateTime createdAt,
        DateTime? updatedAt,
        DateTime? deletedAt)
    {
        return new Activity
        {
            UvaCode = uvaCode,
            ProgramCode = programCode,
            ResponsibleProfileCode = responsibleProfileCode,
            ActivityTypeCode = activityTypeCode,
            ActivityRecurrencePatternCode = activityRecurrencePatternCode,
            Name = name,
            Description = description,
            StartDate = startDate,
            EndDate = endDate,
            LocationLatitude = locationLatitude,
            LocationLongitude = locationLongitude,
            RegistrationRadiusMeters = registrationRadiusMeters,
            StateCode = stateCode,
            CreatedAt = createdAt,
            UpdatedAt = updatedAt,
            DeletedAt = deletedAt,
        };
    }

    private static void ValidateInputs(
        string activityTypeCode,
        string? activityRecurrencePatternCode,
        string name,
        DateTime startDate,
        DateTime endDate,
        int registrationRadiusMeters,
        DateTime nowUtc)
    {
        if (string.IsNullOrWhiteSpace(activityTypeCode))
        {
            throw new InvalidOperationException("Tipo de actividad inválido");
        }

        if (activityRecurrencePatternCode != null && string.IsNullOrWhiteSpace(activityRecurrencePatternCode))
        {
            throw new InvalidOperationException("Identificador de patrón de recurrencia inválido");
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            throw new InvalidOperationException("El nombre es obligatorio");
        }

        if (startDate >= endDate || startDate < nowUtc)
        {
            throw new InvalidOperationException("La fecha adjuntada no es válida");
        }

        if (registrationRadiusMeters <= 5 || registrationRadiusMeters > 1000)
        {
            throw new InvalidOperationException("El radio de registro no es valido");
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
