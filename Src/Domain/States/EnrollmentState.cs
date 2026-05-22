namespace U_VoluntApp_Backend.Src.Domain.States;

using U_VoluntApp_Backend.Src.Domain.Utils.Configuration;

public class EnrollmentState : ReferenceAdapter
{
    public override void ChangeName(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
        {
            throw new ArgumentException("El nombre del estado de la inscripción no puede estar vacío");
        }

        Name = newName;
    }

    internal static EnrollmentState Rehydrate(string uvaCode, string name)
    {
        return new EnrollmentState
        {
            UvaCode = uvaCode,
            Name = name
        };
    }
}
