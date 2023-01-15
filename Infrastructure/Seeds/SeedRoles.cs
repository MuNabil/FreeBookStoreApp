using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Seeds;

public class SeedRoles
{
    public static async Task SeedAsync(RoleManager<IdentityRole> roleManager)
    {
        await roleManager.CreateAsync(new IdentityRole(Helper.Roles.SuperAdmin.ToString()));
        await roleManager.CreateAsync(new IdentityRole(Helper.Roles.Admin.ToString()));
        await roleManager.CreateAsync(new IdentityRole(Helper.Roles.Basic.ToString()));
    }
}
