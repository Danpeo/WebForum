using Microsoft.AspNetCore.Identity;
using WebForum_new.Models;

namespace WebForum_new.Data;

public class Seed
{
    public static async Task SeedUsersAndRolesAsync(IApplicationBuilder applicationBuilder)
    {
        using IServiceScope serviceScope = applicationBuilder.ApplicationServices.CreateScope();
        //Roles
        RoleManager<IdentityRole> roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        if (!await roleManager.RoleExistsAsync(IdentityRoles.Admin))
            await roleManager.CreateAsync(new IdentityRole(IdentityRoles.Admin));
        if (!await roleManager.RoleExistsAsync(IdentityRoles.User))
            await roleManager.CreateAsync(new IdentityRole(IdentityRoles.User));

        //Users
        UserManager<AppUser> userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

        string adminEmail = "danilvar4@gmail.com";
        AppUser? adminUser = await userManager.FindByEmailAsync(adminEmail);
        if (adminUser == null)
        {
            var newAdminUser = new AppUser()
            {
                UserName = "Admin",
                Email = adminEmail,
                EmailConfirmed = true,
                UserBio = "Admin of Web Forum",
                RegistrationDate = new DateTime(2023, 08, 03),
                LastLogin = DateTime.Now,
            };
            await userManager.CreateAsync(newAdminUser, "Root_123");
            await userManager.AddToRoleAsync(newAdminUser, IdentityRoles.Admin);
        }

        string userEmail = "user@mailR.com";
        AppUser? appUser = await userManager.FindByEmailAsync(userEmail);
        if (appUser == null)
        {
            var newAppUser = new AppUser()
            {
                UserName = "app-user",
                Email = userEmail,
                EmailConfirmed = true,
                UserBio = "Regular user",
                RegistrationDate = new DateTime(2023, 08, 03),
                LastLogin = DateTime.Now,
            };
            await userManager.CreateAsync(newAppUser, "User_123");
            await userManager.AddToRoleAsync(newAppUser, IdentityRoles.User);
        }
    }
}