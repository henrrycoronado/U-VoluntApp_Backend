namespace U_VoluntApp_Backend.Src.Infrastructure.Persistence.Models.States;

using U_VoluntApp_Backend.Src.Infrastructure.Persistence.Models.Contract;
using U_VoluntApp_Backend.Src.Infrastructure.Persistence.Models.VolProgram;

public partial class ContractState
{
    public int Id { get; set; }

    public string UvaCode { get; set; } = null!;

    public string Name { get; set; } = null!;

    public virtual ICollection<ProgramCollaborator> ProgramCollaborators { get; set; } = new List<ProgramCollaborator>();

    public virtual ICollection<UserScholarship> UserScholarships { get; set; } = new List<UserScholarship>();
}
