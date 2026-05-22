namespace U_VoluntApp_Backend.Src.Application.DTOs;

public class ApiResponse<T>
{
    public bool Success { get; set; }

    public T? Data { get; set; }

    public string? Message { get; set; }

    public static ApiResponse<T> Ok(T data) =>
        new() { Success = true, Data = data, Message = null };

    public static ApiResponse<T> Fail(string message) =>
        new() { Success = false, Data = default, Message = message };
}

public class PagedResponse<T>
{
    public IEnumerable<T> Items { get; set; } = [];

    public int TotalCount { get; set; }

    public int Page { get; set; }

    public int PageSize { get; set; }

    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
}
