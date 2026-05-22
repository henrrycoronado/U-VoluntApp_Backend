namespace U_VoluntApp_Backend.Src.Infrastructure.Persistence.Models.States;

using U_VoluntApp_Backend.Src.Infrastructure.Persistence.Models.VolProgram;

public partial class ProgramState
{
    public int Id { get; set; }

    public string UvaCode { get; set; } = null!;

    public string Name { get; set; } = null!;

    public virtual ICollection<VolProgram> VolPrograms { get; set; } = new List<VolProgram>();
}
