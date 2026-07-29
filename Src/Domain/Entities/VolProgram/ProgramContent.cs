namespace U_VoluntApp_Core.Src.Domain.Entities.VolProgram;

using U_VoluntApp_Core.Src.Domain.Utils.Constants;

public class ProgramContent
{
    public string UvaCode { get; private set; } = string.Empty;

    public string ProgramCode { get; private set; } = string.Empty;

    public string? Description { get; private set; }

    public string? ActivitiesDescription { get; private set; }

    public string? ScheduleInfo { get; private set; }

    public string? LeadershipInfo { get; private set; }

    public string? ContactInfo { get; private set; }

    public string? MissionStatement { get; private set; }

    public string? ProfilePhotoUrl { get; private set; }

    public string? CoverPhotoUrl { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public DateTime? UpdatedAt { get; private set; }

    public static ProgramContent Create(
        string programCode,
        string? description,
        string? activitiesDescription,
        string? scheduleInfo,
        string? leadershipInfo,
        string? contactInfo,
        string? missionStatement,
        string? profilePhotoUrl,
        string? coverPhotoUrl,
        DateTime nowUtc)
    {
        if (string.IsNullOrWhiteSpace(programCode))
        {
            throw new InvalidOperationException("El ID del programa es invalido");
        }

        return new ProgramContent
        {
            UvaCode = Guid.NewGuid().ToString(),
            ProgramCode = programCode,
            Description = description ?? "Bienvenido a nuestro programa de voluntariado :D",
            ActivitiesDescription = activitiesDescription ?? "Proximamente detallaremos las actividades que realizamos",
            ScheduleInfo = scheduleInfo ?? "Aqui pondremos el horario de nuestras actividades",
            LeadershipInfo = leadershipInfo ?? "Información sobre nuestros lideres proximamente",
            ContactInfo = contactInfo ?? "Información de contacto por definirse :3",
            MissionStatement = missionStatement ?? "Nuestra misión es ser llamados a servir :)",
            ProfilePhotoUrl = profilePhotoUrl ?? ProfilePathConstants.ProfileProgramPath,
            CoverPhotoUrl = coverPhotoUrl ?? BannerPathConstants.BannerWithTextDarkPath,
            CreatedAt = nowUtc
        };
    }

    public void ApplyUpdate(
        string? description,
        string? activitiesDescription,
        string? scheduleInfo,
        string? leadershipInfo,
        string? contactInfo,
        string? missionStatement,
        string? profilePhotoUrl,
        string? coverPhotoUrl,
        DateTime nowUtc)
    {
        bool updated = false;

        Description = UpdateIfNotNull(Description, description, ref updated);
        ActivitiesDescription = UpdateIfNotNull(ActivitiesDescription, activitiesDescription, ref updated);
        ScheduleInfo = UpdateIfNotNull(ScheduleInfo, scheduleInfo, ref updated);
        LeadershipInfo = UpdateIfNotNull(LeadershipInfo, leadershipInfo, ref updated);
        ContactInfo = UpdateIfNotNull(ContactInfo, contactInfo, ref updated);
        MissionStatement = UpdateIfNotNull(MissionStatement, missionStatement, ref updated);
        ProfilePhotoUrl = UpdateIfNotNull(ProfilePhotoUrl, profilePhotoUrl, ref updated);
        CoverPhotoUrl = UpdateIfNotNull(CoverPhotoUrl, coverPhotoUrl, ref updated);

        if (!updated)
        {
            throw new InvalidOperationException("No se han realizado cambios en el contenido del programa");
        }

        UpdatedAt = nowUtc;
    }

    internal static ProgramContent Rehydrate(
        string uvaCode,
        string programCode,
        string? description,
        string? activitiesDescription,
        string? scheduleInfo,
        string? leadershipInfo,
        string? contactInfo,
        string? missionStatement,
        string? profilePhotoUrl,
        string? coverPhotoUrl,
        DateTime createdAt,
        DateTime? updatedAt)
    {
        return new ProgramContent
        {
            UvaCode = uvaCode,
            ProgramCode = programCode,
            Description = description,
            ActivitiesDescription = activitiesDescription,
            ScheduleInfo = scheduleInfo,
            LeadershipInfo = leadershipInfo,
            ContactInfo = contactInfo,
            MissionStatement = missionStatement,
            ProfilePhotoUrl = profilePhotoUrl,
            CoverPhotoUrl = coverPhotoUrl,
            CreatedAt = createdAt,
            UpdatedAt = updatedAt,
        };
    }

    private static string? UpdateIfNotNull(string? currentValue, string? newValue, ref bool updated)
    {
        if (!string.IsNullOrWhiteSpace(newValue) && currentValue != newValue)
        {
            updated = true;
            return newValue;
        }

        return currentValue;
    }
}
