namespace U_VoluntApp_Backend.Src.Domain.Types;

using U_VoluntApp_Backend.Src.Domain.Utils.Configuration;

public class ScholarshipType : ReferenceAdapter
{
    private bool _isActive;

    public override void ChangeName(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
        {
            throw new ArgumentException("El nombre del tipo de beca no puede estar vacío");
        }

        Name = newName;
    }

    public bool IsScholarshipTypeActive()
    {
        return _isActive;
    }

    public void ActivateScholarshipType()
    {
        _isActive = true;
    }

    public void DeactivateScholarshipType()
    {
        _isActive = false;
    }

    internal static ScholarshipType Rehydrate(string uvaCode, string name, bool isActive)
    {
        return new ScholarshipType
        {
            UvaCode = uvaCode,
            Name = name,
            _isActive = isActive
        };
    }
}
