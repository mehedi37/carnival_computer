using carnival_computer.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;

public static class SeedData
{
    public static async Task Initialize(IServiceProvider serviceProvider, UserManager<carnival_computerUser> userManager)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        string[] roleNames = { "Admin", "User" };
        IdentityResult roleResult;

        foreach (var roleName in roleNames)
        {
            var roleExist = await roleManager.RoleExistsAsync(roleName);
            if (!roleExist)
            {
                roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }

        var adminUser = new carnival_computerUser
        {
            UserName = "admin@admin.com",
            Email = "admin@admin.com",
            Name = "Admin User",
            Role = "Admin"
        };

        string adminPassword = "Admin@NPHard1";
        var user = await userManager.FindByEmailAsync("admin@admin.com");

        if (user == null)
        {
            var createAdminUser = await userManager.CreateAsync(adminUser, adminPassword);
            if (createAdminUser.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }
        else
        {
            user.Name = adminUser.Name;
            user.Role = adminUser.Role;
            user.ProfilePic = adminUser.ProfilePic;
            await userManager.UpdateAsync(user);
        }
    }
}
