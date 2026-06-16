namespace U_VoluntApp_Backend.Src.Infrastructure.Persistence.Seeders;

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using U_VoluntApp_Backend.Src.Domain.Utils.Enums;
using U_VoluntApp_Backend.Src.Infrastructure.Persistence.Models.Activity;
using U_VoluntApp_Backend.Src.Infrastructure.Persistence.Models.VolProgram;

public static class DataSeeder
{
    public static async Task SeedInitialDataAsync(AppDbContext context, IConfiguration configuration)
    {
        var suEmail = configuration["SUPERUSER_EMAIL"];
        if (string.IsNullOrWhiteSpace(suEmail))
        {
            return;
        }

        var suProfile = await context.Profiles.FirstOrDefaultAsync(p => p.Email == suEmail);
        if (suProfile == null)
        {
            return;
        }

        var suCode = suProfile.UvaCode;

        // 1. Volunteer Programs
        var programs = new (string UvaCode, string Name, string? Acronym)[]
        {
            ("6ca9df57-19d2-430b-9c4c-3ba868114f24", "Ministerio de Monaguillos", null),
            ("711e5a59-9f79-43a9-a9a3-5c3b94b05a61", "Ministerio de Musica", null),
            ("a8b27f4d-4c12-4217-91f1-ef0dbaf9e7a8", "Catedra Cardenal Julio Terrazas", null),
            ("bf9f0951-b062-42df-b371-55bb40026e68", "Alpha", null),
            ("d30f6a2d-b0a3-4886-9a25-ccceb4d7c0f1", "Programa del Adulto Mayor - PAM", "PAM"),
            ("f1b9b1d9-81a1-432a-bc91-23d91eb69735", "Formacion de Lideres", null),
            ("47c1a84f-e25c-4fdb-9e79-22a969f688e5", "Mision Basilio", null),
            ("28d57579-251c-4395-9b24-9b0d3b6f27dc", "Vina del Senor Plan 3000", null),
            ("9cfb3e2b-272e-4071-8bc6-eb528f89c748", "Jovins", null),
            ("1842bc3c-f4df-41bb-98f5-4cf55097de69", "Camino a/en la Universidad", null),
            ("ea667232-2775-47e9-a477-d6b38c234a5d", "Alas de Esperanza", null),
            ("5180f98e-49b0-466d-a111-e7370788ab63", "Recicla y Ayuda", null),
            ("349eb5be-968b-4b3c-b26a-9b1d1f05a9de", "Promocion y difusion de la Pastoral", null),
            ("a2d21226-9f17-4d7a-8f35-6548ebc43f79", "Atencion a emergencias", null)
        };

        foreach (var p in programs)
        {
            if (!await context.VolPrograms.AnyAsync(vp => vp.UvaCode == p.UvaCode))
            {
                var program = new VolProgram
                {
                    UvaCode = p.UvaCode,
                    Name = p.Name,
                    Acronym = p.Acronym,
                    ManagerProfileCode = suCode,
                    StateCode = ProgramState.Active.GetUvaCode(),
                    CreatedAt = DateTime.UtcNow
                };
                context.VolPrograms.Add(program);

                // Add content for the first two as examples
                if (p.UvaCode == "6ca9df57-19d2-430b-9c4c-3ba868114f24")
                {
                    context.ProgramContents.Add(new ProgramContent
                    {
                        UvaCode = "pc-monaguillos",
                        ProgramCode = p.UvaCode,
                        Description = "Fortalece la identidad catolica de universitarios desde los servicios religiosos.",
                        ActivitiesDescription = "Servicio liturgico. Formacion espiritual.",
                        MissionStatement = "Senor, hazme un siervo leal.",
                        CreatedAt = DateTime.UtcNow
                    });
                }
                else if (p.UvaCode == "711e5a59-9f79-43a9-a9a3-5c3b94b05a61")
                {
                    context.ProgramContents.Add(new ProgramContent
                    {
                        UvaCode = "pc-musica",
                        ProgramCode = p.UvaCode,
                        Description = "Servicio y evangelizacion a traves del canto.",
                        ActivitiesDescription = "Ensayos de coro. Misas.",
                        MissionStatement = "El que canta, ora dos veces.",
                        CreatedAt = DateTime.UtcNow
                    });
                }
            }
        }

        await context.SaveChangesAsync();

        // 2. Activities
        var activities = new[]
        {
            new { UvaCode = "act-monaguillos-1", ProgramCode = "6ca9df57-19d2-430b-9c4c-3ba868114f24", Name = "Misa de Apertura", Description = "Misa comunitaria de inicio de semestre", Type = ActivityType.Event.GetUvaCode() },
            new { UvaCode = "act-musica-1", ProgramCode = "711e5a59-9f79-43a9-a9a3-5c3b94b05a61", Name = "Ensayo Semanal", Description = "Ensayo del ministerio de musica", Type = ActivityType.Workshop.GetUvaCode() }
        };

        foreach (var a in activities)
        {
            if (!await context.Activities.AnyAsync(act => act.UvaCode == a.UvaCode))
            {
                var now = DateTime.UtcNow;
                var activity = new Activity
                {
                    UvaCode = a.UvaCode,
                    ProgramCode = a.ProgramCode,
                    ResponsibleProfileCode = suCode,
                    ActivityTypeCode = a.Type,
                    Name = a.Name,
                    Description = a.Description,
                    StartDate = now.AddDays(1),
                    EndDate = now.AddDays(1).AddHours(2),
                    LocationLatitude = -17.7833,
                    LocationLongitude = -63.1821,
                    RegistrationRadiusMeters = 100,
                    StateCode = ActivityState.Active.GetUvaCode(),
                    CreatedAt = now
                };
                context.Activities.Add(activity);

                context.ActivityRules.Add(new ActivityRule
                {
                    UvaCode = "rule-" + a.UvaCode,
                    ActivityCode = a.UvaCode,
                    TotalCapacity = 50,
                    CountsVolunteerHours = true,
                    RequiresEnrollment = true,
                    RequiresApproval = false,
                    CreatedAt = now
                });
            }
        }

        await context.SaveChangesAsync();
    }
}
