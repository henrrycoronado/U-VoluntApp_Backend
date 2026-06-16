namespace U_VoluntApp_Backend.Src.Domain.Utils.Configuration;

public class RequestFilter
{
    public int State { get; set; }

    public string? StateName { get; set; }

    public int Page { get; set; } = 1;

    public int PageSize { get; set; } = 10;

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }
}
