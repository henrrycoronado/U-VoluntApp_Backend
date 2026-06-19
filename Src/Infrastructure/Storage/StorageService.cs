namespace U_VoluntApp_Backend.Src.Infrastructure.Storage;

using System;
using System.IO;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using U_VoluntApp_Backend.Src.Application.Interfaces;
using U_VoluntApp_Backend.Src.Domain.Utils.Constants;

public class StorageService : IStorageService
{
    private readonly IAmazonS3 _s3Client;
    private readonly string _publicBaseUrl;
    private readonly string _uploadBucket;
    private readonly string _defaultsBucket;

    public StorageService(IAmazonS3 s3Client, IConfiguration configuration)
    {
        _s3Client = s3Client;

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
