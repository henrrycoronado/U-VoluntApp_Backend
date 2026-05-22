namespace U_VoluntApp_Backend.Src.Infrastructure.Persistence.Models.States;

using U_VoluntApp_Backend.Src.Infrastructure.Persistence.Models.Tracking;

public partial class TrackingState
{
    public int Id { get; set; }

    public string UvaCode { get; set; } = null!;

    public string Name { get; set; } = null!;

    public virtual ICollection<TrackingLog> TrackingLogs { get; set; } = new List<TrackingLog>();
}
