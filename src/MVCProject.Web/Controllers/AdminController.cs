using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MigraineDiary.Web.Data;
using MigraineDiary.Web.Models;
using MigraineDiary.Web.Services.Contracts;

namespace MigraineDiary.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IAdminService adminService;

        public AdminController(ApplicationDbContext dbContext,
                                      IAdminService adminService)
        {
            this.dbContext = dbContext;
            this.adminService = adminService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateRole(string roleName)
        {
            await this.adminService.CreateRoleAsync(roleName);

            // Get controller's name and action's name without using magic strings.
            string actionName = nameof(AdminController.Index);
            string controllerName = nameof(AdminController).Substring(0, nameof(AdminController).Length - "Controller".Length);

            return RedirectToAction(actionName, controllerName);
        }

        [HttpGet]
        public IActionResult SetRole()
        {
            SetRoleViewModel viewModel = new SetRoleViewModel();
            this.adminService.PopulateUsersAndRoles(viewModel);

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetRole(string userId, string roleId)
        {
            await this.adminService.AssignRoleAsync(userId, roleId);

            // Get controller's name and action's name without using magic strings.
            string actionName = nameof(AdminController.RolesAudit);
            string controllerName = nameof(AdminController).Substring(0, nameof(AdminController).Length - "Controller".Length);

            return RedirectToAction(actionName, controllerName);
        }

        [HttpGet]
        public async Task<IActionResult> RolesAudit() 
        {
            Dictionary<string, List<string>> auditViewModel = await this.adminService.GetUsersAndRolesAsync();

            return View(auditViewModel);
        }
    }
}
