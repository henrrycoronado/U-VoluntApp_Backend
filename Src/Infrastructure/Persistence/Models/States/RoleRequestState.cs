namespace U_VoluntApp_Backend.Src.Infrastructure.Persistence.Models.States;

using U_VoluntApp_Backend.Src.Infrastructure.Persistence.Models.Contract;

public partial class RoleRequestState
{
    public int Id { get; set; }

    public string UvaCode { get; set; } = null!;

    public string Name { get; set; } = null!;

    public virtual ICollection<RoleRequest> RoleRequests { get; set; } = new List<RoleRequest>();
}
