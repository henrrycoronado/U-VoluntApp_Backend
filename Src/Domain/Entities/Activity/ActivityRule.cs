namespace U_VoluntApp_Backend.Src.Domain.Entities.Activity;

using Microsoft.OpenApi.Any;
using U_VoluntApp_Backend.Src.Domain.Utils.Constants;

public class ActivityRule
{
    public string UvaCode { get; private set; } = string.Empty;

    public string ActivityCode { get; private set; } = string.Empty;

    public bool RequiresEnrollment { get; private set; }

    public DateTime? EnrollmentDeadline { get; private set; }

    public bool RequiresApproval { get; private set; }

    public int TotalCapacity { get; private set; }

    public decimal CostAmount { get; private set; }

    public bool CountsVolunteerHours { get; private set; }

    public string? PhotoUrl { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public DateTime? UpdatedAt { get; private set; }

    public static ActivityRule Create(
        string activityCode,
        bool requiresEnrollment,
        bool requiresApproval,
        bool countsVolunteerHours,
        string? photoUrl,
        DateTime? enrollmentDeadline,
        DateTime? startDate,
        int totalCapacity,
        decimal costAmount,
        DateTime nowUtc)
    {
        if (string.IsNullOrWhiteSpace(activityCode))
        {
            throw new InvalidOperationException("Identificador de actividad inválido");
        }

        ValidateInputs(enrollmentDeadline, startDate, totalCapacity, costAmount);

        return new ActivityRule
        {
            UvaCode = Guid.NewGuid().ToString(),
            ActivityCode = activityCode,
            RequiresEnrollment = requiresEnrollment,
            RequiresApproval = requiresApproval,
            CountsVolunteerHours = countsVolunteerHours,
            PhotoUrl = photoUrl ?? ProfilePathConstants.ProfileActivityPath,
            EnrollmentDeadline = enrollmentDeadline,
            TotalCapacity = totalCapacity,
            CostAmount = costAmount,
            CreatedAt = nowUtc,
        };
    }

    public void ApplyUpdate(
        bool requiresEnrollment,
        bool requiresApproval,
        bool countsVolunteerHours,
        string? photoUrl,
        DateTime? enrollmentDeadline,
        DateTime? startDate,
        int totalCapacity,
        decimal costAmount,
        DateTime nowUtc)
    {
        ValidateInputs(enrollmentDeadline, startDate, totalCapacity, costAmount);

        bool updated = false;

        RequiresEnrollment = UpdateIfNotNull(RequiresEnrollment, requiresEnrollment, ref updated);
        RequiresApproval = UpdateIfNotNull(RequiresApproval, requiresApproval, ref updated);
        CountsVolunteerHours = UpdateIfNotNull(CountsVolunteerHours, countsVolunteerHours, ref updated);
        PhotoUrl = UpdateIfNotNull(PhotoUrl, photoUrl, ref updated);
        EnrollmentDeadline = UpdateIfNotNull(EnrollmentDeadline, enrollmentDeadline, ref updated);
        TotalCapacity = UpdateIfNotNull(TotalCapacity, totalCapacity, ref updated);
        CostAmount = UpdateIfNotNull(CostAmount, costAmount, ref updated);

        if (!updated)
        {
            throw new InvalidOperationException("No se han realizado cambios en la regla de la actividad");
        }

        UpdatedAt = nowUtc;
    }

    public bool HasCapacity(int currentEnrollmentCount)
    {
        if (TotalCapacity == 0)
        {
            return true;
        }

        return TotalCapacity > currentEnrollmentCount;
    }

    public bool HasTimeForRegister(DateTime checkDate)
    {
        if (!EnrollmentDeadline.HasValue)
        {
            return false;
        }

        return checkDate < EnrollmentDeadline.Value;
    }

    internal static ActivityRule Rehydrate(
        string uvaCode,
        string activityCode,
        DateTime? enrollmentDeadline,
        bool requiresApproval,
        int totalCapacity,
        decimal costAmount,
        bool countsVolunteerHours,
        bool requiresEnrollment,
        string? photoUrl,
        DateTime createdAt,
        DateTime? updatedAt)
    {
        return new ActivityRule
        {
            UvaCode = uvaCode,
            ActivityCode = activityCode,
            EnrollmentDeadline = enrollmentDeadline,
            RequiresApproval = requiresApproval,
            TotalCapacity = totalCapacity,
            CostAmount = costAmount,
            CountsVolunteerHours = countsVolunteerHours,
            RequiresEnrollment = requiresEnrollment,
            PhotoUrl = photoUrl,
            CreatedAt = createdAt,
            UpdatedAt = updatedAt,
        };
    }

    private static void ValidateInputs(DateTime? enrollmentDeadline, DateTime? startDate, int totalCapacity, decimal costAmount)
    {
        if (enrollmentDeadline.HasValue && !startDate.HasValue)
        {
            throw new InvalidOperationException("La fecha de inicio de la actividad es requerida cuando se especifica la fecha límite de inscripción");
        }

        if (enrollmentDeadline.HasValue && startDate.HasValue && enrollmentDeadline >= startDate)
        {
            throw new InvalidOperationException("La fecha límite de inscripción debe ser anterior a la fecha de inicio de la actividad");
        }

        if (totalCapacity < 0)
        {
            throw new InvalidOperationException("Capacidad total ingresada no valida");
        }

        if (costAmount < 0)
        {
            throw new InvalidOperationException("El monto del costo no es valido");
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
