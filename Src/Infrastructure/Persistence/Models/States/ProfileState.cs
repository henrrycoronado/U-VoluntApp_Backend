namespace U_VoluntApp_Backend.Src.Infrastructure.Persistence.Models.States;

using U_VoluntApp_Backend.Src.Infrastructure.Persistence.Models.Profile;

public partial class ProfileState
{
    public int Id { get; set; }

    public string UvaCode { get; set; } = null!;

    public string Name { get; set; } = null!;

    public virtual ICollection<Profile> Profiles { get; set; } = new List<Profile>();
}
