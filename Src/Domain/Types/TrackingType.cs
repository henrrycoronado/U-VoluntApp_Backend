namespace U_VoluntApp_Backend.Src.Domain.Types;

using U_VoluntApp_Backend.Src.Domain.Utils.Configuration;

public class TrackingType : ReferenceAdapter
{
    private bool _isActive;

    public override void ChangeName(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
        {
            throw new ArgumentException("El nombre del tipo de seguimiento no puede estar vacío");
        }

        Name = newName;
    }

    public bool IsTrackingTypeActive()
    {
        return _isActive;
    }

    public void ActivateTrackingType()
    {
        _isActive = true;
    }

    public void DeactivateTrackingType()
    {
        _isActive = false;
    }

    internal static TrackingType Rehydrate(string uvaCode, string name, bool isActive)
    {
        return new TrackingType
        {
            UvaCode = uvaCode,
            Name = name,
            _isActive = isActive
        };
    }
}
