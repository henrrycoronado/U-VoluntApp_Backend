namespace U_VoluntApp_Backend.Src.Infrastructure.Persistence.Models.Tracking;

public partial class Evidence
{
    public long Id { get; set; }

    public string UvaCode { get; set; } = null!;

    public string TrackingLogCode { get; set; } = null!;

    public string PhotoUrl { get; set; } = null!;

    public string EvidenceTypeCode { get; set; } = null!;

    public string TypeCode { get; set; } = null!;

    public string? Observations { get; set; }

    public double LocationLatitude { get; set; }

    public double LocationLongitude { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual TrackingLog TrackingLog { get; set; } = null!;
}
