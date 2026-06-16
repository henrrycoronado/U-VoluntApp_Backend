namespace U_VoluntApp_Backend.Src.Domain.Entities.VolProgram;

public class VolProgram
{
    public string UvaCode { get; private set; } = string.Empty;

    public string Name { get; private set; } = string.Empty;

    public string? Acronym { get; private set; }

    public string? ManagerProfileCode { get; private set; }

    public string StateCode { get; private set; } = string.Empty;

    public DateTime CreatedAt { get; private set; }

    public DateTime? UpdatedAt { get; private set; }

    public DateTime? DeletedAt { get; private set; }

    public ProgramContent? Content { get; private set; }

    public static VolProgram Create(
        string name,
        string acronym,
        string managerCode,
        string stateCode,
        DateTime nowUtc)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new InvalidOperationException("El nombre del programa es obligatorio");
        }

        if (string.IsNullOrWhiteSpace(managerCode))
        {
            throw new InvalidOperationException("El ID del gestor del programa es obligatorio");
        }

        if (string.IsNullOrWhiteSpace(stateCode) || !stateCode.StartsWith("stage-"))
        {
            throw new InvalidOperationException("El formato del código de estado es inválido");
        }

        return new VolProgram
        {
            UvaCode = Guid.NewGuid().ToString(),
            Name = name,
            Acronym = acronym,
            ManagerProfileCode = managerCode,
            StateCode = stateCode,
            CreatedAt = nowUtc,
        };
    }

    public void ApplyUpdate(
        string name,
        string? acronym,
        DateTime nowUtc)
    {
        if (DeletedAt.HasValue)
        {
            throw new InvalidOperationException("No se puede actualizar un programa eliminado");
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            throw new InvalidOperationException("El nombre del programa no puede estar vacío");
        }

        bool updated = false;
        Name = UpdateIfNotNull(Name, name, ref updated) ?? Name;
        Acronym = UpdateIfNotNull(Acronym, acronym, ref updated);

        if (!updated)
        {
            throw new InvalidOperationException("No se realizaron cambios en el programa");
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
            throw new InvalidOperationException("El estado del programa es el mismo que el actual");
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
            throw new InvalidOperationException("El programa ya está eliminado");
        }

        StateCode = stateCode;
        DeletedAt = nowUtc;
    }

    public void AddProgramContent(ProgramContent content)
    {
        if (DeletedAt.HasValue)
        {
            throw new InvalidOperationException("No se puede agregar contenido a un programa eliminado");
        }

        Content = content;
    }

    internal static VolProgram Rehydrate(
        string uvaCode,
        string name,
        string? acronym,
        string? managerProfileCode,
        string stateCode,
        DateTime createdAt,
        DateTime? updatedAt,
        DateTime? deletedAt)
    {
        return new VolProgram
        {
            UvaCode = uvaCode,
            Name = name,
            Acronym = acronym,
            ManagerProfileCode = managerProfileCode,
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
