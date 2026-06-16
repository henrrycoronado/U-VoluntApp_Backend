namespace U_VoluntApp_Backend.Src.Infrastructure.Reports;

using Microsoft.Extensions.Configuration;
using Supabase;
using U_VoluntApp_Backend.Src.Application.Interfaces;
using U_VoluntApp_Backend.Src.Domain.Utils.Constants;

public class SupabaseStorageService : IStorageService
{
    private readonly Client _supabase;
    private readonly string _publicBaseUrl;
    private readonly string _uploadBucket;
    private readonly string _defaultsBucket;

    public SupabaseStorageService(Client supabase, IConfiguration configuration)
    {
        _supabase = supabase;
        _publicBaseUrl = configuration["STORAGE_PUBLIC_BASE_URL"]
            ?? StorageConstants.PublicBaseUrl;
        _uploadBucket = configuration["STORAGE_UPLOAD_BUCKET"]
            ?? StorageConstants.UploadBucket;
        _defaultsBucket = configuration["STORAGE_DEFAULTS_BUCKET"]
            ?? StorageConstants.DefaultsBucket;
    }

    public async Task<string> UploadAsync(IFormFile file, string folder)
    {
        if (string.Equals(_uploadBucket, _defaultsBucket, StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException("La configuracion de STORAGE_UPLOAD_BUCKET no puede apuntar al bucket de defaults");
        }

        if (string.IsNullOrWhiteSpace(folder))
        {
            throw new InvalidOperationException("La carpeta de destino es obligatoria");
        }

        var normalizedFolder = folder.Trim().Trim('/').Trim('\\');
        if (normalizedFolder.Contains("..", StringComparison.Ordinal))
        {
            throw new InvalidOperationException("La carpeta de destino no es valida");
        }

        using var stream = file.OpenReadStream();
        var buffer = new byte[file.Length];
        await stream.ReadExactlyAsync(buffer);

        var fileName = $"{normalizedFolder}/{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

        await _supabase.Storage
            .From(_uploadBucket)
            .Upload(buffer, fileName, new Supabase.Storage.FileOptions
            {
                ContentType = file.ContentType,
                Upsert = false,
            });

        return $"{_publicBaseUrl.TrimEnd('/')}/{_uploadBucket}/{fileName}";
    }

    public async Task DeleteAsync(string path)
    {
        var prefix = $"{_publicBaseUrl.TrimEnd('/')}/{_uploadBucket}/";
        var relativePath = path.StartsWith(prefix, StringComparison.OrdinalIgnoreCase)
            ? path[prefix.Length..]
            : path;

        await _supabase.Storage.From(_uploadBucket).Remove([relativePath]);
    }
}
