using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MigraineDiary.Data.DbModels;
using MigraineDiary.Services.Contracts;
using MigraineDiary.ViewModels;
using System.Security.Claims;

namespace MigraineDiary.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IAdminService adminService;
        private readonly IMessageService messageService;
        private readonly UserManager<ApplicationUser> userManager;

        public AdminController(UserManager<ApplicationUser> userManager,
                                              IAdminService adminService,
                                            IMessageService messageService)
        {
            this.adminService = adminService;
            this.messageService = messageService;
            this.userManager = userManager;
        }

        public IActionResult Index()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

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
            await adminService.CreateRoleAsync(roleName);

            // Get controller's name and action's name without using magic strings.
            string actionName = nameof(AdminController.Index);
            string controllerName = nameof(AdminController).Substring(0, nameof(AdminController).Length - "Controller".Length);

            return RedirectToAction(actionName, controllerName);
        }

        [HttpGet]
        public IActionResult AssignRole()
        {
            SetRoleViewModel viewModel = this.adminService.PopulateUsersAndRoles();

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignRole(string userId, string roleId)
        {
            await adminService.AssignRoleAsync(userId, roleId);

            // Get controller's name and action's name without using magic strings.
            string actionName = nameof(AdminController.UsersAudit);
            string controllerName = nameof(AdminController).Substring(0, nameof(AdminController).Length - "Controller".Length);

            return RedirectToAction(actionName, controllerName);
        }

        [HttpGet]
        public IActionResult RemoveRole()
        {
            SetRoleViewModel viewModel = this.adminService.PopulateUsersAndRoles();

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveRole(string userId, string roleId)
        {
            await adminService.RemoveFromRoleAsync(userId, roleId);

            // Get controller's name and action's name without using magic strings.
            string actionName = nameof(AdminController.UsersAudit);
            string controllerName = nameof(AdminController).Substring(0, nameof(AdminController).Length - "Controller".Length);

            return RedirectToAction(actionName, controllerName);
        }

        [HttpGet]
        public async Task<IActionResult> UsersAudit()
        {
            Dictionary<string, List<string>> auditViewModel = await adminService.GetUsersAndRolesAsync();

            return View(auditViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Inbox(int pageIndex = 1, int pageSize = 1, string orderByDate = "NewestFirst")
        {
            // Custom validation against web parameter tampering
            if (pageSize != 1 &&
                 pageSize != 5 &&
                 pageSize != 10 ||
                orderByDate != "NewestFirst" &&
                 orderByDate != "OldestFirst")
            {
                return BadRequest();
            }

            PaginatedList<MessageViewModel> messages = await messageService.GetAllAsync(pageIndex, pageSize, orderByDate);

            // Send pagination size and ordering criteria to the view.
            ViewData["pageSize"] = pageSize;
            ViewData["orderByDate"] = orderByDate;

            return View(messages);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteMessage(string messageId)
        {
            try
            {
                await messageService.DeleteAsync(messageId);

                // Get controller name and action name without using magic strings
                string actionName = nameof(AdminController.Inbox);
                string controllerName = nameof(AdminController).Substring(0, nameof(AdminController).Length - "Controller".Length);

                return RedirectToAction(actionName, controllerName);
            }
            catch (Exception ex)
            {
                // Log exception to logger.
                // Get controller name and action name without using magic strings
                string actionName = nameof(AdminController.Inbox);
                string controllerName = nameof(AdminController).Substring(0, nameof(AdminController).Length - "Controller".Length);

                return RedirectToAction(actionName, controllerName);
            }
        }

        [HttpGet]
        public async Task<IActionResult> SoftDeleteMessage(string messageId)
        {
            try
            {
                await messageService.SoftDeleteAsync(messageId);

                // Get controller name and action name without using magic strings
                string actionName = nameof(AdminController.Inbox);
                string controllerName = nameof(AdminController).Substring(0, nameof(AdminController).Length - "Controller".Length);

                return RedirectToAction(actionName, controllerName);
            }
            catch (Exception ex)
            {
                // Log exception to logger.
                // Get controller name and action name without using magic strings
                string actionName = nameof(AdminController.Inbox);
                string controllerName = nameof(AdminController).Substring(0, nameof(AdminController).Length - "Controller".Length);

                return RedirectToAction(actionName, controllerName);
            }
        }

        [HttpGet]
        [Route("/messageNotifications")]
        public async Task<JsonResult> GetMessagesCount()
        {
            // Get messages count.
            int msgCount = await messageService.GetMessagesCountAsync();

            // Return count as JSON.
            return Json(new { Count = msgCount });
        }

        [HttpGet]
        public IActionResult DeleteRole()
        {
            // Get all roles.
            DeleteRoleViewModel viewModel = this.adminService.GetRoles();

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteRole(string roleId)
        {
            try
            {
                await this.adminService.DeleteRoleAsync(roleId);

            }
            catch (ArgumentException ex)
            {
                // TODO: Log exception
                return NotFound(ex.Message);
            }

            // Get controller's name and action's name without using magic strings.
            string actionName = nameof(AdminController.UsersAudit);
            string controllerName = nameof(AdminController).Substring(0, nameof(AdminController).Length - "Controller".Length);

            return RedirectToAction(actionName, controllerName);
        }
    }
}
