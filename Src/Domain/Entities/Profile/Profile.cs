namespace U_VoluntApp_Core.Src.Domain.Entities.Profile;

using U_VoluntApp_Core.Src.Domain.Utils.Constants;

public class Profile
{
    public string UvaCode { get; private set; } = string.Empty;

    public string IdentityUserId { get; private set; } = string.Empty;

    public string FirstName { get; private set; } = string.Empty;

    public string LastName { get; private set; } = string.Empty;

    public string Email { get; private set; } = string.Empty;

    public string? PhotoUrl { get; private set; }

    public string CareerCode { get; private set; } = string.Empty;

    public string? AddressLocation { get; private set; }

    public string? Phone { get; private set; }

    public decimal PersonalGoalHours { get; private set; }

    public string StateCode { get; private set; } = string.Empty;

    public DateTime CreatedAt { get; private set; }

    public DateTime? UpdatedAt { get; private set; }

    public DateTime? DeletedAt { get; private set; }

    public static Profile Create(
        string identityUserId,
        string email,
        string firstName,
        string lastName,
        string stateCode,
        DateTime nowUtc)
    {
        if (string.IsNullOrWhiteSpace(identityUserId))
        {
            throw new InvalidOperationException("No se pudo obtener el identificador del usuario");
        }

        if (string.IsNullOrWhiteSpace(email) || !email.EndsWith("@ucb.edu.bo"))
        {
            throw new InvalidOperationException("El correo es obligatorio o la extensión del correo no es válida");
        }

        if (string.IsNullOrWhiteSpace(firstName))
        {
            throw new InvalidOperationException("El nombre es obligatorio");
        }

        if (string.IsNullOrWhiteSpace(lastName))
        {
            throw new InvalidOperationException("El apellido es obligatorio");
        }

        if (string.IsNullOrWhiteSpace(stateCode) || !stateCode.StartsWith("stage-"))
        {
            throw new InvalidOperationException("El formato del código de estado es inválido");
        }

        return new Profile
        {
            UvaCode = Guid.NewGuid().ToString(),
            IdentityUserId = identityUserId,
            Email = email,
            FirstName = firstName,
            LastName = lastName,
            PhotoUrl = ProfilePathConstants.ProfileLogoPath,
            CareerCode = "type-1",
            StateCode = stateCode,
            CreatedAt = nowUtc
        };
    }

    public void ApplyUpdate(
        string firstName,
        string lastName,
        string? phone,
        string? addressLocation,
        string? careerCode,
        decimal? personalGoalHours,
        DateTime nowUtc)
    {
        if (DeletedAt.HasValue)
        {
            throw new InvalidOperationException("No se puede actualizar un perfil eliminado");
        }

        if (string.IsNullOrWhiteSpace(firstName))
        {
            throw new InvalidOperationException("El nombre no puede estar vacío");
        }

        if (string.IsNullOrWhiteSpace(lastName))
        {
            throw new InvalidOperationException("El apellido no puede estar vacío");
        }

        if (!string.IsNullOrWhiteSpace(phone) && (phone.Length < 8 || phone.Length > 15))
        {
            throw new InvalidOperationException("El teléfono no es valido");
        }

        if (careerCode != null && (string.IsNullOrWhiteSpace(careerCode) || !careerCode.StartsWith("type-")))
        {
            throw new InvalidOperationException("El formato del código de la carrera es inválido");
        }

        if (personalGoalHours.HasValue && personalGoalHours.Value < 0)
        {
            throw new InvalidOperationException("Las horas de meta personal no pueden ser negativas");
        }

        bool updated = false;

        FirstName = UpdateIfNotNull(FirstName, firstName, ref updated) ?? FirstName;
        LastName = UpdateIfNotNull(LastName, lastName, ref updated) ?? LastName;
        Phone = UpdateIfNotNull(Phone, phone, ref updated);
        AddressLocation = UpdateIfNotNull(AddressLocation, addressLocation, ref updated);
        CareerCode = UpdateIfNotNull(CareerCode, careerCode ?? CareerCode, ref updated) ?? CareerCode;
        PersonalGoalHours = UpdateIfNotNull(PersonalGoalHours, personalGoalHours ?? PersonalGoalHours, ref updated);

        if (!updated)
        {
            throw new InvalidOperationException("No se realizaron cambios en el perfil");
        }

        UpdatedAt = nowUtc;
    }

    public void UpdatePhoto(string photoUrl, DateTime nowUtc)
    {
        if (DeletedAt.HasValue)
        {
            throw new InvalidOperationException("No se puede actualizar la foto de un perfil eliminado");
        }

        if (string.IsNullOrWhiteSpace(photoUrl))
        {
            throw new InvalidOperationException("La URL de la foto es obligatoria");
        }

        if (photoUrl == PhotoUrl)
        {
            throw new InvalidOperationException("La foto es la misma que la actual");
        }

        PhotoUrl = photoUrl;
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
            throw new InvalidOperationException("El estado del perfil es el mismo que el actual");
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
            throw new InvalidOperationException("El perfil ya está eliminado");
        }

        StateCode = stateCode;
        DeletedAt = nowUtc;
    }

    internal static Profile Rehydrate(
        string uvaCode,
        string identityUserId,
        string firstName,
        string lastName,
        string email,
        string? photoUrl,
        string careerCode,
        string? addressLocation,
        string? phone,
        decimal personalGoalHours,
        string stateCode,
        DateTime createdAt,
        DateTime? updatedAt,
        DateTime? deletedAt)
    {
        return new Profile
        {
            UvaCode = uvaCode,
            IdentityUserId = identityUserId,
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            PhotoUrl = photoUrl,
            CareerCode = careerCode,
            AddressLocation = addressLocation,
            Phone = phone,
            PersonalGoalHours = personalGoalHours,
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
