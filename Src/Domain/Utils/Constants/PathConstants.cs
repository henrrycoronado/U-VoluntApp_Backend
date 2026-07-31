namespace U_VoluntApp_Core.Src.Domain.Utils.Constants;

public static class StorageConstants
{
    public static string PublicBaseUrl =>
        Environment.GetEnvironmentVariable("STORAGE_PUBLIC_BASE_URL") ?? string.Empty;

    public static string UploadBucket =>
        Environment.GetEnvironmentVariable("STORAGE_UPLOAD_BUCKET") ?? "CreatedFiles";

    public static string ProfileFolder =>
        Environment.GetEnvironmentVariable("STORAGE_FOLDER_PROFILES") ?? "profiles";

    public static string EvidenceFolder =>
        Environment.GetEnvironmentVariable("STORAGE_FOLDER_EVIDENCES") ?? "evidences";
}
