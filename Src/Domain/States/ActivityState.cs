namespace U_VoluntApp_Backend.Src.Domain.States;

using U_VoluntApp_Backend.Src.Domain.Utils.Configuration;

public class ActivityState : ReferenceAdapter
{
    public override void ChangeName(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
        {
            throw new ArgumentException("El nombre del estado de la actividad no puede estar vacío");
        }

        Name = newName;
    }

    internal static ActivityState Rehydrate(string uvaCode, string name)
    {
        return new ActivityState
        {
            UvaCode = uvaCode,
            Name = name
        };
    }
}
