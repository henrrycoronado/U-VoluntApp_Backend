namespace U_VoluntApp_Backend.Src.Infrastructure.Persistence.Models.Types;

using U_VoluntApp_Backend.Src.Infrastructure.Persistence.Models.Tracking;

public partial class TrackingType
{
    public int Id { get; set; }

    public string UvaCode { get; set; } = null!;

    public string Name { get; set; } = null!;

    public bool IsActive { get; set; }

    public virtual ICollection<Evidence> Evidences { get; set; } = new List<Evidence>();
}
