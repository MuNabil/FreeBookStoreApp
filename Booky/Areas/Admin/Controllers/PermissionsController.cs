using System.Security.Claims;
using Domain.Constants;
using Infrastructure.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Booky.Areas.Admin.Controllers;

[Area("Admin")]
public class PermissionsController : Controller
{
    private readonly RoleManager<IdentityRole> _roleManager;

    public PermissionsController(RoleManager<IdentityRole> roleManager)
    {
        _roleManager = roleManager;
    }

    public async Task<IActionResult> ManagePermissions(string roleId)
    {
        var role = await _roleManager.FindByIdAsync(roleId);

        if (role == null) return NotFound();

        var roleClaims = _roleManager.GetClaimsAsync(role).Result.Select(c => c.Value).ToList();

        var allClaims = Permissions.GenerateAllPermissions();

        var allPermissions = allClaims.Select(p => new CheckBoxViewModel { Value = p }).ToList();

        foreach (var permission in allPermissions)
        {
            if (roleClaims.Any(c => c == permission.Value))
                permission.IsSelected = true;
        }

        var viewModel = new PermissionsViewModel
        {
            RoleId = role.Id,
            RoleName = role.Name,
            RoleClaims = allPermissions
        };

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ManagePermissions(PermissionsViewModel model)
    {
        var role = await _roleManager.FindByIdAsync(model.RoleId);

        if (role == null) return NotFound();

        //Get all claims which already assigned to this role
        var roleClaims = await _roleManager.GetClaimsAsync(role);

        //Remover all claims from this role
        foreach (var claim in roleClaims)
        {
            await _roleManager.RemoveClaimAsync(role, claim);
        }

        //Get all selcted claims returned from view in model parameter 
        var selectedClaims = model.RoleClaims.Where(c => c.IsSelected).ToList();

        //Add all selected Claims to this role
        foreach (var claim in selectedClaims)
        {
            await _roleManager.AddClaimAsync(role, new Claim("Permission", claim.Value));
        }

        return RedirectToAction("Roles", "Accounts");
    }
}
