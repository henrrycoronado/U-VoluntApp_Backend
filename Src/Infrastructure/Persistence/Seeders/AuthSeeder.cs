namespace U_VoluntApp_Core.Src.Infrastructure.Persistence.Seeders;

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using U_VoluntApp_Core.Src.Domain.Entities.Profile;
using U_VoluntApp_Core.Src.Domain.Utils.Enums;
using U_VoluntApp_Core.Src.Infrastructure.Persistence.Interfaces.Profile;

public static class AuthSeeder
{
    public static async Task SeedRolesAndSuperUserAsync(
        RoleManager<IdentityRole> roleManager,
        UserManager<IdentityUser> userManager,
        IProfileRepository profileRepository,
        IConfiguration configuration)
    {
        string[] roleNames = { "Volunteer", "Coordinator", "Admin", "SuperUser" };

        foreach (var roleName in roleNames)
        {
            var roleExist = await roleManager.RoleExistsAsync(roleName);
            if (!roleExist)
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }

        var suEmail = configuration["SUPERUSER_EMAIL"];
        var suPassword = configuration["SUPERUSER_PASSWORD"];

        if (string.IsNullOrWhiteSpace(suEmail) || string.IsNullOrWhiteSpace(suPassword))
        {
            return;
        }

        var existingUser = await userManager.FindByEmailAsync(suEmail);

        if (existingUser == null)
        {
            var suId = Guid.NewGuid().ToString();

            var suUser = new IdentityUser
            {
                Id = suId,
                UserName = suEmail,
                Email = suEmail,
                NormalizedEmail = suEmail.ToUpperInvariant(),
                NormalizedUserName = suEmail.ToUpperInvariant(),
                EmailConfirmed = true,
            };

            var createResult = await userManager.CreateAsync(suUser, suPassword);

            if (createResult.Succeeded)
            {
                await userManager.AddToRoleAsync(suUser, "SuperUser");

                var profile = await profileRepository.GetByEmailAsync(suEmail);
                if (profile == null)
                {
                    var nowUtc = DateTime.UtcNow;
                    var suProfile = Profile.Create(
                        suId,
                        suEmail,
                        "Super",
                        "User",
                        ProfileState.Active.GetUvaCode(),
                        nowUtc);

                    await profileRepository.AddAsync(suProfile);
                }
            }
        }
        else
        {
            if (!await userManager.IsInRoleAsync(existingUser, "SuperUser"))
            {
                await userManager.AddToRoleAsync(existingUser, "SuperUser");
            }
        }
    }
}
