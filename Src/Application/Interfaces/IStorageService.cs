namespace U_VoluntApp_Backend.Src.Application.Interfaces;

public interface IStorageService
{
    Task<string> UploadAsync(IFormFile file, string folder);

    Task DeleteAsync(string path);
}
