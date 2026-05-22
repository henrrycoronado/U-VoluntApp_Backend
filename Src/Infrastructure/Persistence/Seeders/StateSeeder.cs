namespace U_VoluntApp_Backend.Src.Infrastructure.Persistence.Seeders;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using U_VoluntApp_Backend.Src.Infrastructure.Persistence.Models.States;

public static class StateSeeder
{
    public static async Task SeedAsync(AppDbContext db)
    {
        if (!await db.ProfileStates.AnyAsync())
        {
            var states = new[]
            {
                new ProfileState { UvaCode = "stage-1", Name = "inactive" },
                new ProfileState { UvaCode = "stage-2", Name = "active" },
                new ProfileState { UvaCode = "stage-3", Name = "deleted" }
            };
            db.ProfileStates.AddRange(states);
        }

        if (!await db.ProgramStates.AnyAsync())
        {
            var states = new[]
            {
                new ProgramState { UvaCode = "stage-1", Name = "inactive" },
                new ProgramState { UvaCode = "stage-2", Name = "active" },
                new ProgramState { UvaCode = "stage-3", Name = "deleted" }
            };
            db.ProgramStates.AddRange(states);
        }

        if (!await db.ActivityStates.AnyAsync())
        {
            var states = new[]
            {
                new ActivityState { UvaCode = "stage-1", Name = "inactive" },
                new ActivityState { UvaCode = "stage-2", Name = "active" },
                new ActivityState { UvaCode = "stage-3", Name = "deleted" },
                new ActivityState { UvaCode = "stage-4", Name = "canceled" }
            };
            db.ActivityStates.AddRange(states);
        }

        if (!await db.EnrollmentStates.AnyAsync())
        {
            var states = new[]
            {
                new EnrollmentState { UvaCode = "stage-1", Name = "pending" },
                new EnrollmentState { UvaCode = "stage-2", Name = "active" },
                new EnrollmentState { UvaCode = "stage-3", Name = "rejected" },
                new EnrollmentState { UvaCode = "stage-4", Name = "canceled" }
            };
            db.EnrollmentStates.AddRange(states);
        }

        if (!await db.TrackingStates.AnyAsync())
        {
            var states = new[]
            {
                new TrackingState { UvaCode = "stage-1", Name = "pending" },
                new TrackingState { UvaCode = "stage-2", Name = "active" },
                new TrackingState { UvaCode = "stage-3", Name = "deleted" }
            };
            db.TrackingStates.AddRange(states);
        }

        if (!await db.ContractStates.AnyAsync())
        {
            var states = new[]
            {
                new ContractState { UvaCode = "stage-1", Name = "pending" },
                new ContractState { UvaCode = "stage-2", Name = "active" },
                new ContractState { UvaCode = "stage-3", Name = "rejected" },
                new ContractState { UvaCode = "stage-4", Name = "canceled" }
            };
            db.ContractStates.AddRange(states);
        }

        if (!await db.RoleRequestStates.AnyAsync())
        {
            var states = new[]
            {
                new RoleRequestState { UvaCode = "stage-1", Name = "pending" },
                new RoleRequestState { UvaCode = "stage-2", Name = "active" },
                new RoleRequestState { UvaCode = "stage-3", Name = "rejected" },
                new RoleRequestState { UvaCode = "stage-4", Name = "canceled" }
            };
            db.RoleRequestStates.AddRange(states);
        }

        if (db.ChangeTracker.HasChanges())
        {
            await db.SaveChangesAsync();
        }
    }
}
