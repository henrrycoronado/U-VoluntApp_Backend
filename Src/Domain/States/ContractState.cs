namespace U_VoluntApp_Backend.Src.Domain.States;

using U_VoluntApp_Backend.Src.Domain.Utils.Configuration;

public class ContractState : ReferenceAdapter
{
    public override void ChangeName(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
        {
            throw new ArgumentException("El nombre del estado del contrato no puede estar vacío");
        }

        Name = newName;
    }

    internal static ContractState Rehydrate(string uvaCode, string name)
    {
        return new ContractState
        {
            UvaCode = uvaCode,
            Name = name
        };
    }
}
