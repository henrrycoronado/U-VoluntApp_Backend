namespace U_VoluntApp_Backend.Src.Infrastructure.Persistence.Models.Types;

using U_VoluntApp_Backend.Src.Infrastructure.Persistence.Models.Contract;

public partial class ScholarshipType
{
    public int Id { get; set; }

    public string UvaCode { get; set; } = null!;

    public string Name { get; set; } = null!;

    public bool IsActive { get; set; }

    public virtual ICollection<UserScholarship> UserScholarships { get; set; } = new List<UserScholarship>();
}
