using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MigraineDiary.Services.Contracts;
using MigraineDiary.ViewModels;
using MigraineDiary.Web.Controllers;
using System.Security.Claims;

namespace MigraineDiary.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class ArticleController : Controller
    {
        private readonly IArticleService articleService;

        public ArticleController(IArticleService articleService)
        {
            this.articleService = articleService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Add()
        {
            ArticleAddFormModel model = new ArticleAddFormModel();

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(ArticleAddFormModel addModel)
        {
            if (!ModelState.IsValid)
            {
                return View(addModel);
            }

            string currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await articleService.CreateArticle(addModel, currentUserId);

            // Get controller's name and action's name without using magic strings.
            string actionName = nameof(HomeController.Index);
            string controllerName = nameof(HomeController).Substring(0, nameof(HomeController).Length - "Controller".Length);

            return RedirectToAction(actionName, controllerName, new { area = "" });
        }
    }
}
