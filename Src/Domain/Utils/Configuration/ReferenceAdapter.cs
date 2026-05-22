namespace U_VoluntApp_Backend.Src.Domain.Utils.Configuration;

public abstract class ReferenceAdapter
{
    public string UvaCode { get; set; } = string.Empty;

    public string Name { get; set; } = null!;

    public virtual void ChangeName(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
        {
            throw new ArgumentException("El nombre del objeto no puede estar vacio");
        }

        Name = newName;
    }
}
