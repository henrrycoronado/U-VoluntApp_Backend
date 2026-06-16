namespace U_VoluntApp_Backend.Src.Application.DTOs;

public class CreateEnrollmentDto
{
    public string ActivityCode { get; set; } = null!;

    public string? ActivityGroupCode { get; set; }
}

public class EnrollmentResponseDto
{
    public string UvaCode { get; set; } = null!;

    public string ActivityCode { get; set; } = null!;

    public string ActivityName { get; set; } = null!;

    public string EnrolledProfileCode { get; set; } = null!;

    public string EnrolledProfileName { get; set; } = null!;

    public string? ActivityGroupCode { get; set; }

    public string State { get; set; } = null!;

    public string StateCode { get; set; } = null!;

    public DateTime CreatedAt { get; set; }
}

public class ReviewEnrollmentDto
{
    public bool Approved { get; set; }
}
