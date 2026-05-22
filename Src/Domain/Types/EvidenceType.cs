namespace U_VoluntApp_Backend.Src.Domain.Types;

using U_VoluntApp_Backend.Src.Domain.Utils.Configuration;

public class EvidenceType : ReferenceAdapter
{
    private bool _isActive;

    public override void ChangeName(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
        {
            throw new ArgumentException("El nombre del tipo de evidencia no puede estar vacío");
        }

        Name = newName;
    }

    public bool IsEvidenceTypeActive()
    {
        return _isActive;
    }

    public void ActivateEvidenceType()
    {
        _isActive = true;
    }

    public void DeactivateEvidenceType()
    {
        _isActive = false;
    }

    internal static EvidenceType Rehydrate(string uvaCode, string name, bool isActive)
    {
        return new EvidenceType
        {
            UvaCode = uvaCode,
            Name = name,
            _isActive = isActive
        };
    }
}
