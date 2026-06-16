namespace U_VoluntApp_Backend.Src.Application.DTOs;

public class ReferenceStateDto
{
    public string UvaCode { get; set; } = null!;

    public string Name { get; set; } = null!;
}

public class ReferenceTypeDto
{
    public string UvaCode { get; set; } = null!;

    public string Name { get; set; } = null!;

    public bool IsActive { get; set; }
}

public class UpdateReferenceStateNameDto
{
    public string Name { get; set; } = null!;
}

public class CreateReferenceTypeDto
{
    public string Name { get; set; } = null!;

    public bool IsActive { get; set; } = true;
}

public class UpdateReferenceTypeDto
{
    public string? Name { get; set; }

    public bool? IsActive { get; set; }
}
