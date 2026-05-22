namespace U_VoluntApp_Backend.Src.Domain.States;

using U_VoluntApp_Backend.Src.Domain.Utils.Configuration;

public class TrackingState : ReferenceAdapter
{
    public override void ChangeName(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
        {
            throw new ArgumentException("El nombre del estado del rastreo no puede estar vacío");
        }

        Name = newName;
    }

    internal static TrackingState Rehydrate(string uvaCode, string name)
    {
        return new TrackingState
        {
            UvaCode = uvaCode,
            Name = name
        };
    }
}
