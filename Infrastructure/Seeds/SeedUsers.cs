using System.Security.Claims;
using Domain.Constants;
using Domain.Entities;
using Infrastructure.ViewModel;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Seeds;

public static class SeedUsers
{
    public static async Task SeedBasicUsers(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        var users = new List<ApplicationUser>
        {
            new ApplicationUser
            {
                UserName = "Basic@test.com",
                Name = "Basic",
                Email = "Basic@test.com",
                UserImage = Helper.DefaultImage,
                IsActive = true,
                EmailConfirmed = true
            }
        };

        foreach (var user in users)
        {
            var checkUser = await userManager.FindByEmailAsync(user.Email);
            if (checkUser is null)
            {
                await userManager.CreateAsync(user, "Pa$$w0rd");
                await userManager.AddToRoleAsync(user, Helper.Roles.Basic.ToString());
            }
        }
    }

    public static async Task SeedSuperAdmin(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        var superAdmin = new ApplicationUser
        {
            UserName = "SuperAdmin@test.com",
            Name = "SuperAdmin",
            Email = "SuperAdmin@test.com",
            UserImage = Helper.DefaultImage,
            IsActive = true,
            EmailConfirmed = true
        };

        var checkUser = await userManager.FindByEmailAsync(superAdmin.Email);
        if (checkUser is null)
        {
            await userManager.CreateAsync(superAdmin, "Pa$$w0rd");
            await userManager.AddToRoleAsync(superAdmin, Helper.Roles.SuperAdmin.ToString());
        }

        await roleManager.SeedClaimsAsync();
    }



    public static async Task SeedClaimsAsync(this RoleManager<IdentityRole> roleManager)
    {
        var superAdminRole = await roleManager.FindByNameAsync(Helper.Roles.SuperAdmin.ToString());

        var modules = Enum.GetValues(typeof(Helper.PermissionModules));

        foreach (var module in modules)
            await roleManager.AddPermissionClaims(superAdminRole, module.ToString());

    }
    public static async Task AddPermissionClaims(this RoleManager<IdentityRole> roleManager, IdentityRole role,
        string module)
    {
        var allClaims = await roleManager.GetClaimsAsync(role);
        var allPermissions = Permissions.GeneratePermissionsFromMoldule(module);

        foreach (var permission in allPermissions)
        {
            if (!allClaims.Any(c => c.Type == "Permission" && c.Value == permission))
            {
                await roleManager.AddClaimAsync(role, new Claim("Permission", permission));
            }
        }
    }
}

