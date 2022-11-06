using Booky.Resources;
using Domain.Entities;
using Infrastructure.IRepositories;
using Infrastructure.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Booky.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoriesController : Controller
    {
        private readonly IServicesRepository<Category> _servicesCategory;
        private readonly IServicesRepositoryLog<LogCategory> _logCategory;
        private readonly UserManager<ApplicationUser> _userManager;

        public CategoriesController(IServicesRepository<Category> servicesCategory,
            IServicesRepositoryLog<LogCategory> logCategory, UserManager<ApplicationUser> userManager)
        {
            _servicesCategory = servicesCategory;
            _logCategory = logCategory;
            _userManager = userManager;
        }
        public IActionResult Categories()
        {
            var categories = _servicesCategory.GetAll();
            var logCategories = _logCategory.GetAll();
            return View(new CategoryViewModel { Categories = categories, LogCategories = logCategories, NewCategory = new Category() });
        }

        public IActionResult DeleteLog(Guid Id)
        {
            if (_logCategory.DeleteLog(Id))
                return RedirectToAction(nameof(Categories));

            return RedirectToAction(nameof(Categories));
        }

        public IActionResult Delete(Guid Id)
        {
            var userId = _userManager.GetUserId(User);
            if (_servicesCategory.Delete(Id) && _logCategory.Delete(Id, Guid.Parse(userId)))
                return RedirectToAction(nameof(Categories));

            return RedirectToAction(nameof(Categories));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Save(CategoryViewModel model)
        {
            if (!ModelState.IsValid)
                return RedirectToAction(nameof(Categories));


            var userId = _userManager.GetUserId(User);
            //Create
            if (_servicesCategory.FindById(model.NewCategory.Id) == null)
            {
                if (_servicesCategory.FindByName(model.NewCategory.Name) != null)
                    SessionMsgs(Helper.Error, WebResource.SaveErr, WebResource.NameAlreadyExists);
                else
                {
                    if (_servicesCategory.Save(model.NewCategory)
                        && _logCategory.Save(model.NewCategory.Id, Guid.Parse(userId)))
                        SessionMsgs(Helper.Success, WebResource.Save, WebResource.SaveCategoryMsg);
                    else
                        SessionMsgs(Helper.Error, WebResource.SaveErr, WebResource.SaveCategoryErrMsg);
                }
            }
            //Edit
            else
            {
                if (_servicesCategory.Save(model.NewCategory)
                    && _logCategory.Update(model.NewCategory.Id, Guid.Parse(userId)))
                    SessionMsgs(Helper.Success, WebResource.Edit, WebResource.EditCategoryMsg);
                else
                    SessionMsgs(Helper.Error, WebResource.EditErr, WebResource.EditCategoryErrMsg);
            }

            return RedirectToAction(nameof(Categories));
        }

        private void SessionMsgs(string msgType, string title, string msg)
        {
            HttpContext.Session.SetString(Helper.MsgType, msgType);
            HttpContext.Session.SetString(Helper.Title, title);
            HttpContext.Session.SetString(Helper.Msg, msg);
        }

    }
}
