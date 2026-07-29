namespace U_VoluntApp_Core.Src.Domain.Utils.Constants;

public static class StorageConstants
{
    public static string PublicBaseUrl =>
        Environment.GetEnvironmentVariable("STORAGE_PUBLIC_BASE_URL") ?? string.Empty;

    public static string DefaultsBucket =>
        Environment.GetEnvironmentVariable("STORAGE_DEFAULTS_BUCKET") ?? "Defaults";

    public static string UploadBucket =>
        Environment.GetEnvironmentVariable("STORAGE_UPLOAD_BUCKET") ?? "CreatedFiles";

    public static string ProfileFolder =>
        Environment.GetEnvironmentVariable("STORAGE_FOLDER_PROFILES") ?? "profiles";

    public static string EvidenceFolder =>
        Environment.GetEnvironmentVariable("STORAGE_FOLDER_EVIDENCES") ?? "evidences";
}

public static class ProfilePathConstants
{
    public static string ProfileActivityPath => BuildDefaultPath("Profile/ProfileActivity.webp");

    public static string ProfileActivityIconPath => BuildDefaultPath("Profile/ProfileActivity_Icon.webp");

    public static string ProfileActivityLetterPath => BuildDefaultPath("Profile/ProfileActivity_Letra.webp");

    public static string ProfileLogoPath => BuildDefaultPath("Profile/ProfileLogo.webp");

    public static string ProfileProgramPath => BuildDefaultPath("Profile/ProfileProgram.webp");

    public static string ProfileProgramIconPath => BuildDefaultPath("Profile/ProfileProgram_Icon.webp");

    public static string ProfileProgramLetterPath => BuildDefaultPath("Profile/ProfileProgram_letra.webp");

    public static string ProfileVolunteerPath => BuildDefaultPath("Profile/ProfileVolunter.webp");

    private static string BuildDefaultPath(string relativePath)
    {
        return $"{StorageConstants.PublicBaseUrl.TrimEnd('/')}/{StorageConstants.DefaultsBucket}/{relativePath}";
    }
}

public static class BannerPathConstants
{
    public static string BannerDarkPath => BuildDefaultPath("Banner/Banner_Dark.webp");

    public static string BannerLightPath => BuildDefaultPath("Banner/Banner_Light.webp");

    public static string BannerWithTextDarkPath => BuildDefaultPath("Banner/BannerWithText_Dark.webp");

    public static string BannerWithTextLightPath => BuildDefaultPath("Banner/BannerWithText_Light.webp");

    private static string BuildDefaultPath(string relativePath)
    {
        return $"{StorageConstants.PublicBaseUrl.TrimEnd('/')}/{StorageConstants.DefaultsBucket}/{relativePath}";
    }
}
