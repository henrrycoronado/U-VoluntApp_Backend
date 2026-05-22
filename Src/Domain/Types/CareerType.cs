namespace U_VoluntApp_Backend.Src.Domain.Types;

using U_VoluntApp_Backend.Src.Domain.Utils.Configuration;

public class CareerType : ReferenceAdapter
{
    private bool _isActive;

    public override void ChangeName(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
        {
            throw new ArgumentException("El nombre de la carrera no puede estar vacío");
        }

        Name = newName;
    }

    public bool IsCareerActive()
    {
        return _isActive;
    }

    public void ActivateCareer()
    {
        _isActive = true;
    }

    public void DeactivateCareer()
    {
        _isActive = false;
    }

    internal static CareerType Rehydrate(string uvaCode, string name, bool isActive)
    {
        return new CareerType
        {
            UvaCode = uvaCode,
            Name = name,
            _isActive = isActive
        };
    }
}
