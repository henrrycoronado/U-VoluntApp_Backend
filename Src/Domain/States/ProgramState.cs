namespace U_VoluntApp_Backend.Src.Domain.States;

using U_VoluntApp_Backend.Src.Domain.Utils.Configuration;

public class ProgramState : ReferenceAdapter
{
    public override void ChangeName(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
        {
            throw new ArgumentException("El nombre del estado del programa no puede estar vacío");
        }

        Name = newName;
    }

    internal static ProgramState Rehydrate(string uvaCode, string name)
    {
        return new ProgramState
        {
            UvaCode = uvaCode,
            Name = name
        };
    }
}
