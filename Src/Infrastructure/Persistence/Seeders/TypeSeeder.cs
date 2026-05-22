namespace U_VoluntApp_Backend.Src.Infrastructure.Persistence.Seeders;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using U_VoluntApp_Backend.Src.Infrastructure.Persistence.Models.Types;

public static class TypeSeeder
{
    public static async Task SeedAsync(AppDbContext db)
    {
        if (!await db.ActivityTypes.AnyAsync())
        {
            var types = new[]
            {
                new ActivityType { UvaCode = "type-1", Name = "taller", IsActive = true },
                new ActivityType { UvaCode = "type-2", Name = "mentoria", IsActive = true },
                new ActivityType { UvaCode = "type-3", Name = "brigada", IsActive = true },
                new ActivityType { UvaCode = "type-4", Name = "evento", IsActive = true },
                new ActivityType { UvaCode = "type-5", Name = "colecta", IsActive = true },
                new ActivityType { UvaCode = "type-6", Name = "customize", IsActive = true }
            };
            db.ActivityTypes.AddRange(types);
        }

        if (!await db.EvidenceTypes.AnyAsync())
        {
            var types = new[]
            {
                new EvidenceType { UvaCode = "type-1", Name = "check_in", IsActive = true },
                new EvidenceType { UvaCode = "type-2", Name = "check_out", IsActive = true }
            };
            db.EvidenceTypes.AddRange(types);
        }

        if (!await db.TrackingTypes.AnyAsync())
        {
            var types = new[]
            {
                new TrackingType { UvaCode = "type-1", Name = "scaning", IsActive = true },
                new TrackingType { UvaCode = "type-2", Name = "manual", IsActive = true }
            };
            db.TrackingTypes.AddRange(types);
        }

        if (!await db.CareerTypes.AnyAsync())
        {
            var types = new[]
            {
                new CareerType { UvaCode = "type-1", Name = "none", IsActive = true },
                new CareerType { UvaCode = "type-2", Name = "ingenieria de software", IsActive = true },
                new CareerType { UvaCode = "type-3", Name = "ingenieria civil", IsActive = true },
                new CareerType { UvaCode = "type-4", Name = "derecho", IsActive = true },
                new CareerType { UvaCode = "type-5", Name = "medicina", IsActive = true },
                new CareerType { UvaCode = "type-6", Name = "administracion de empresas", IsActive = true },
                new CareerType { UvaCode = "type-7", Name = "psicologia", IsActive = true },
                new CareerType { UvaCode = "type-8", Name = "comunicacion social", IsActive = true },
                new CareerType { UvaCode = "type-9", Name = "arquitectura", IsActive = true },
                new CareerType { UvaCode = "type-10", Name = "bioquimica", IsActive = true },
                new CareerType { UvaCode = "type-11", Name = "marketing", IsActive = true }
            };
            db.CareerTypes.AddRange(types);
        }

        if (!await db.ScholarshipTypes.AnyAsync())
        {
            var types = new[]
            {
                new ScholarshipType { UvaCode = "type-1", Name = "ceil", IsActive = true },
                new ScholarshipType { UvaCode = "type-2", Name = "obispo", IsActive = true },
                new ScholarshipType { UvaCode = "type-3", Name = "cre", IsActive = true },
                new ScholarshipType { UvaCode = "type-4", Name = "bachiller", IsActive = true }
            };
            db.ScholarshipTypes.AddRange(types);
        }

        if (db.ChangeTracker.HasChanges())
        {
            await db.SaveChangesAsync();
        }
    }
}
