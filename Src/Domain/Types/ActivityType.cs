namespace U_VoluntApp_Backend.Src.Domain.Types;

using U_VoluntApp_Backend.Src.Domain.Utils.Configuration;

public class ActivityType : ReferenceAdapter
{
    private bool _isActive;

    public override void ChangeName(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
        {
            throw new ArgumentException("El nombre del tipo de actividad no puede estar vacío");
        }

        Name = newName;
    }

    public bool IsActivityTypeActive()
    {
        return _isActive;
    }

    public void ActivateActivityType()
    {
        _isActive = true;
    }

    public void DeactivateActivityType()
    {
        _isActive = false;
    }

    internal static ActivityType Rehydrate(string uvaCode, string name, bool isActive)
    {
        return new ActivityType
        {
            UvaCode = uvaCode,
            Name = name,
            _isActive = isActive
        };
    }
}
