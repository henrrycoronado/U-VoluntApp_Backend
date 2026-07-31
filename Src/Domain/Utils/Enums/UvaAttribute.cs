namespace U_VoluntApp_Core.Src.Domain.Utils.Enums;

[AttributeUsage(AttributeTargets.Field)]
public class UvaAttribute : Attribute
{
    public UvaAttribute(string code, string name)
    {
        Code = code;
        Name = name;
    }

    public string Code { get; }

    public string Name { get; }
}
