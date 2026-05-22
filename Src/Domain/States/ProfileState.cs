namespace U_VoluntApp_Backend.Src.Domain.States;

using U_VoluntApp_Backend.Src.Domain.Utils.Configuration;

public class ProfileState : ReferenceAdapter
{
    public override void ChangeName(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
        {
            throw new ArgumentException("El nombre del estado de perfil no puede estar vacío");
        }

        Name = newName;
    }

    internal static ProfileState Rehydrate(string uvaCode, string name)
    {
        return new ProfileState
        {
            UvaCode = uvaCode,
            Name = name
        };
    }
}
