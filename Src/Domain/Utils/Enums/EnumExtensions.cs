namespace U_VoluntApp_Core.Src.Domain.Utils.Enums;

using System.Reflection;

public static class EnumExtensions
{
    public static string GetUvaCode(this Enum value)
    {
        var field = value.GetType().GetField(value.ToString());
        var attribute = field?.GetCustomAttribute<UvaAttribute>();
        return attribute?.Code ?? value.ToString();
    }

    public static string GetUvaName(this Enum value)
    {
        var field = value.GetType().GetField(value.ToString());
        var attribute = field?.GetCustomAttribute<UvaAttribute>();
        return attribute?.Name ?? value.ToString();
    }

    public static T? GetByUvaCode<T>(string code)
        where T : Enum
    {
        foreach (var value in Enum.GetValues(typeof(T)))
        {
            if (((Enum)value).GetUvaCode() == code)
            {
                return (T)value;
            }
        }

        return default;
    }
}
