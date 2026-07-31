namespace U_VoluntApp_Core.Src.Infrastructure.Storage;

using System;
using System.IO;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using U_VoluntApp_Core.Src.Application.Interfaces;
using U_VoluntApp_Core.Src.Domain.Utils.Constants;

public class StorageService : IStorageService
{
    private readonly IAmazonS3 _s3Client;
    private readonly string _publicBaseUrl;
    private readonly string _uploadBucket;

    public StorageService(IAmazonS3 s3Client, IConfiguration configuration)
    {
        _s3Client = s3Client;

        _publicBaseUrl = configuration["STORAGE_PUBLIC_BASE_URL"]
            ?? StorageConstants.PublicBaseUrl;
        _uploadBucket = configuration["STORAGE_UPLOAD_BUCKET"]
            ?? StorageConstants.UploadBucket;
    }

    public async Task<string> UploadAsync(IFormFile file, string folder)
    {
        if (file == null || file.Length == 0)
        {
            throw new ArgumentException("El archivo es obligatorio y no puede estar vacío");
        }

        const long maxSizeBytes = 5 * 1024 * 1024; // 5 MB
        if (file.Length > maxSizeBytes)
        {
            throw new InvalidOperationException("El archivo supera el tamaño máximo permitido de 5 MB");
        }

        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp", ".gif" };
        if (!allowedExtensions.Contains(extension))
        {
            throw new InvalidOperationException("El formato del archivo no está permitido. Solo se permiten imágenes (.jpg, .jpeg, .png, .webp, .gif)");
        }

        var allowedMimeTypes = new[] { "image/jpeg", "image/png", "image/webp", "image/gif" };
        if (!allowedMimeTypes.Contains(file.ContentType.ToLowerInvariant()))
        {
            throw new InvalidOperationException("El tipo de contenido del archivo no está permitido. Solo se permiten imágenes");
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

        var fileName = $"{normalizedFolder}/{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

        using var memoryStream = new MemoryStream();
        await file.CopyToAsync(memoryStream);
        memoryStream.Position = 0;

        var putRequest = new PutObjectRequest
        {
            BucketName = _uploadBucket,
            Key = fileName,
            InputStream = memoryStream,
            ContentType = file.ContentType,
            DisablePayloadSigning = true,
            DisableDefaultChecksumValidation = true
        };

        await _s3Client.PutObjectAsync(putRequest);

        return $"{_publicBaseUrl.TrimEnd('/')}/{_uploadBucket}/{fileName}";
    }

    public async Task DeleteAsync(string path)
    {
        var prefix = $"{_publicBaseUrl.TrimEnd('/')}/{_uploadBucket}/";
        var relativePath = path.StartsWith(prefix, StringComparison.OrdinalIgnoreCase)
            ? path[prefix.Length..]
            : path;

        var deleteRequest = new DeleteObjectRequest
        {
            BucketName = _uploadBucket,
            Key = relativePath
        };

        await _s3Client.DeleteObjectAsync(deleteRequest);
    }
}
