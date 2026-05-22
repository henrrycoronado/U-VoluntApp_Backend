namespace U_VoluntApp_Backend.Src.Domain.States;

using U_VoluntApp_Backend.Src.Domain.Utils.Configuration;

public class RoleRequestState : ReferenceAdapter
{
    public override void ChangeName(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
        {
            throw new ArgumentException("El nombre del estado de la solicitud de rol no puede estar vacío");
        }

        Name = newName;
    }

    internal static RoleRequestState Rehydrate(string uvaCode, string name)
    {
        return new RoleRequestState
        {
            UvaCode = uvaCode,
            Name = name
        };
    }
}
