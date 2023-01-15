using Booky.Resources;
using Domain.Constants;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Booky.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Permissions.Accounts.View)]
    public class AccountsController : Controller
    {
        #region Declaration
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly BookyDbContext _dbContext;
        private readonly SignInManager<ApplicationUser> _signInManager;
        #endregion

        #region Constructor
        public AccountsController(RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager,
            BookyDbContext dbContext, SignInManager<ApplicationUser> signInManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _dbContext = dbContext;
            _signInManager = signInManager;
        }

        #endregion

        #region Actions

        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

                if (result.Succeeded)
                    return RedirectToAction("Index", "Home");
                else
                    ViewBag.LoginErr = false;
            }
            return View(model);

        }

        [Authorize(Permissions.Accounts.View)]
        public async Task<IActionResult> Register()
        {
            var roles = await _roleManager.Roles.OrderBy(r => r.Name).ToListAsync();
            var users = await _dbContext.VwUsers.OrderBy(u => u.Role).ToListAsync();

            var model = new RegisterViewModel
            {
                Roles = roles,
                NewRegister = new NewRegister(),
                Users = users

            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Permissions.Accounts.Create)]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("Register", "Accounts");

            model.NewRegister.UserImage = NewImageUrl(model);

            var user = new ApplicationUser
            {
                Id = model.NewRegister.Id,
                Name = model.NewRegister.Name,
                Email = model.NewRegister.Email,
                UserImage = model.NewRegister.UserImage,
                UserName = model.NewRegister.Email,
                IsActive = model.NewRegister.IsActive,
            };

            //Create
            if (user.Id == null)
            {
                user.Id = Guid.NewGuid().ToString();
                var result = await _userManager.CreateAsync(user, model.NewRegister.Password);

                if (result.Succeeded)
                {
                    var roleResult = await _userManager.AddToRoleAsync(user, model.NewRegister.RoleName);
                    if (roleResult.Succeeded)
                        SessionMsgs(Helper.Success, WebResource.Save, WebResource.SaveUserMsg);
                    else
                        SessionMsgs(Helper.Error, WebResource.SaveErr, WebResource.SaveRoleErrMsg);
                }
                else
                    SessionMsgs(Helper.Error, WebResource.RegisterErr, WebResource.RegisterErrMsg);
            }
            //Edit
            else
            {
                var oldUser = await _userManager.FindByIdAsync(user.Id);

                oldUser.Name = model.NewRegister.Name;
                oldUser.Email = model.NewRegister.Email;
                oldUser.UserImage = model.NewRegister.UserImage;
                oldUser.UserName = model.NewRegister.Email;
                oldUser.IsActive = model.NewRegister.IsActive;

                var result = await _userManager.UpdateAsync(oldUser);

                if (result.Succeeded)
                {
                    var oldRole = await _userManager.GetRolesAsync(oldUser);
                    await _userManager.RemoveFromRolesAsync(oldUser, oldRole);
                    var addRoleResult = await _userManager.AddToRoleAsync(oldUser, model.NewRegister.RoleName);

                    if (addRoleResult.Succeeded)
                        SessionMsgs(Helper.Success, WebResource.Edit, WebResource.EditUserMsg);
                    else
                        SessionMsgs(Helper.Error, WebResource.EditErr, WebResource.EditRoleErrMsg);
                }
                else
                    SessionMsgs(Helper.Error, WebResource.EditErr, WebResource.EditUserErrMsg);
            }

            return RedirectToAction("Register", "Accounts");
        }

        private string NewImageUrl(RegisterViewModel model)
        {
            string? oldImageUrl = model.NewRegister.UserImage;
            string? newImageUrl = UploadImage();

            if (newImageUrl != null)
            {
                DeleteImage(oldImageUrl);
            }
            else
            {
                newImageUrl = oldImageUrl ?? Helper.DefaultImage;
            }
            return newImageUrl;
        }
        private string? UploadImage()
        {
            var ImageFile = HttpContext.Request.Form.Files.SingleOrDefault(f => f.Name == "ImageFile");
            if (ImageFile != null)
            {
                var imageName = ImageFile.FileName;

                string uniqueImageName = Guid.NewGuid().ToString() + Path.GetExtension(imageName);

                var imagePath = Path.Combine(@"wwwroot/", Helper.SaveUserImgPath, uniqueImageName);
                ImageFile.CopyTo(new FileStream(imagePath, FileMode.Create));

                return uniqueImageName;
            }
            else
            {
                return null;
            }
        }
        private void DeleteImage(string? OldImageUrl)
        {
            if (OldImageUrl != null && OldImageUrl != Helper.DefaultImage && OldImageUrl != Guid.Empty.ToString())
            {
                var OldImage = Path.Combine(@"wwwroot/", Helper.SaveUserImgPath, OldImageUrl);

                if (System.IO.File.Exists(OldImage))
                    System.IO.File.Delete(OldImage);
            }
        }


        [Authorize(Permissions.Accounts.Delete)]
        public async Task<IActionResult> DeleteUser(string Id)
        {
            var user = await _userManager.Users.SingleOrDefaultAsync(u => u.Id == Id);

            if (user == null)
                return NotFound();

            DeleteImage(user.UserImage);
            var result = await _userManager.DeleteAsync(user);

            //if(!result.Succeeded)

            return RedirectToAction(nameof(Register));
        }

        [Authorize(Permissions.Accounts.Create)]
        public async Task<IActionResult> ChangePassword(RegisterViewModel model)
        {
            if (model.ChangePassword.Id == _userManager.GetUserId(User)
                || User.IsInRole("SuperAdmin") || User.IsInRole("Admin"))
            {
                var user = await _userManager.FindByIdAsync(model.ChangePassword.Id);

                if (user == null)
                    return RedirectToAction(nameof(Register));

                await _userManager.RemovePasswordAsync(user);

                var result = await _userManager.AddPasswordAsync(user, model.ChangePassword.NewPassword);

                if (result.Succeeded)
                    SessionMsgs(Helper.Success, WebResource.Save, WebResource.ChangePasswordSave);
                else
                    SessionMsgs(Helper.Error, WebResource.SaveErr, WebResource.ChangePasswordErr);

                return RedirectToAction(nameof(Register));
            }

            TempData["Access"] = "false";
            return RedirectToAction(nameof(Register));
        }

        [Authorize(Permissions.Roles.View)]
        public async Task<IActionResult> Roles()
        {
            var roles = await _roleManager.Roles.OrderBy(r => r.Name).ToListAsync();
            return View(new RolesViewModel { Roles = roles, NewRole = new NewRole() });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Permissions.Roles.Create)]
        public async Task<IActionResult> Roles(RolesViewModel model)
        {
            if (!ModelState.IsValid)
                return RedirectToAction(nameof(Roles));

            var role = new IdentityRole
            {
                Id = model.NewRole.RoleId,
                Name = model.NewRole.RoleName
            };

            //Create new Role
            if (role.Id == null)
            {
                role.Id = Guid.NewGuid().ToString();
                var result = await _roleManager.CreateAsync(role);

                if (result.Succeeded)
                    SessionMsgs(Helper.Success, WebResource.Save, WebResource.SaveRoleMsg);
                else
                    SessionMsgs(Helper.Error, WebResource.SaveErr, WebResource.SaveRoleErrMsg);

            }
            //Edit an Existing Role
            else
            {
                var oldRole = await _roleManager.FindByIdAsync(role.Id);

                if (oldRole == null)
                    return NotFound();

                //oldRole.Id = role.Id;
                oldRole.Name = role.Name;
                var result = await _roleManager.UpdateAsync(oldRole);

                if (result.Succeeded)
                    SessionMsgs(Helper.Success, WebResource.Edit, WebResource.EditRoleMsg);
                else
                    SessionMsgs(Helper.Error, WebResource.EditErr, WebResource.EditRoleErrMsg);
            }
            return RedirectToAction(nameof(Roles));
        }

        private void SessionMsgs(string msgType, string title, string msg)
        {
            HttpContext.Session.SetString(Helper.MsgType, msgType);
            HttpContext.Session.SetString(Helper.Title, title);
            HttpContext.Session.SetString(Helper.Msg, msg);
        }


        [Authorize(Permissions.Roles.Delete)]
        public async Task<IActionResult> DeleteRole(string Id)
        {
            var role = await _roleManager.Roles.SingleOrDefaultAsync(r => r.Id == Id);

            if (role == null)
                return NotFound();

            var result = await _roleManager.DeleteAsync(role);

            //if(!result.Succeeded)

            return RedirectToAction(nameof(Roles));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }

        #endregion
    }

}
