namespace U_VoluntApp_Core.Src.Infrastructure.Storage;

using Microsoft.AspNetCore.Http;

public interface IStorageService
{
    Task<string> UploadAsync(IFormFile file, string folder);

    Task DeleteAsync(string path);
}
