using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MigraineDiary.Services.Contracts;
using MigraineDiary.ViewModels;
using MigraineDiary.Data;
using MigraineDiary.Data.DbModels;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace MigraineDiary.Web.Controllers
{
    [Authorize]
    public class HeadacheController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ApplicationDbContext dbContext;
        private readonly IHeadacheService headacheService;

        public HeadacheController(UserManager<ApplicationUser> userManager,
                                          ApplicationDbContext dbContext,
                                              IHeadacheService headacheService)
        {
            this.userManager = userManager;
            this.dbContext = dbContext;
            this.headacheService = headacheService;
        }

        [HttpGet]
        public IActionResult Add()
        {
            HeadacheAddFormModel model = new HeadacheAddFormModel();

            ViewData["currentUserId"] = User.FindFirstValue(ClaimTypes.NameIdentifier);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(HeadacheAddFormModel addModel, string currentUserId)
        {
            // Get current user's Id if ModelState is not valid and it's needed to be routed.
            // If not routed on next submission currentUserId is null and ModelState is not valid again.
            ViewData["currentUserId"] = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Calculate headache duration so it can be custom validated.
            Dictionary<string, int> headacheDuration = this.headacheService.CalculateDuration(addModel.Onset, addModel.EndTime);

            // Custom headache validations.

            // Check if form is submitted with default value which is not DateTime (user didn't select value).
            if (ModelState["Onset"]!.Errors.Any(x => x.ErrorMessage == "The value '' is invalid."))
            {
                // Remove default ErrorMessage.
                ModelState.Remove("Onset");

                // Add custom ErrorMessage.
                ModelState.AddModelError(nameof(addModel.Onset), "Необходимо е да въведете начало на главоболието.");
            }

            // Check if form is submitted with default value which is not DateTime (user didn't select value).
            if (ModelState["EndTime"]!.Errors.Any(x => x.ErrorMessage == "The value '' is invalid."))
            {
                // Remove default ErrorMessage .
                ModelState.Remove("EndTime");

                // Add custom ErrorMessage.
                ModelState.AddModelError(nameof(addModel.EndTime), "Необходимо е да въведете край на главоболието.");
            }

            // If headache ended before it started scenario.
            if (headacheDuration["Days"] < 0 ||
                headacheDuration["Hours"] < 0 ||
                headacheDuration["Minutes"] < 0)
            {
                ModelState.AddModelError(nameof(addModel.EndTime), "Края на главоболието не може да бъде преди началото му.");
            }
            // Else if headache ended at the moment it just started scenario.
            else if (headacheDuration["Days"] <= 0 &&
                     headacheDuration["Hours"] <= 0 &&
                     headacheDuration["Minutes"] == 0)
            {
                ModelState.AddModelError(nameof(addModel.EndTime), "Началото и краят на главоболието не могат да съвпадат.");
            }

            // Check if form is submitted without selecting severity.
            if (addModel.Severity == 0)
            {
                ModelState.AddModelError(nameof(addModel.Severity), "Необходимо е да посочите сила на главоболието.");
            }

            // Check if hidden inputs values are submitted (user didn't chose answer).
            if (addModel.Photophoby == "noAnswer")
            {
                ModelState.AddModelError(nameof(addModel.Photophoby), "Необходимо е да посочите отговор.");
            }

            if (addModel.Phonophoby == "noAnswer")
            {
                ModelState.AddModelError(nameof(addModel.Phonophoby), "Необходимо е да посочите отговор.");
            }

            if (addModel.Nausea == "noAnswer")
            {
                ModelState.AddModelError(nameof(addModel.Nausea), "Необходимо е да посочите отговор.");
            }

            if (addModel.Vomiting == "noAnswer")
            {
                ModelState.AddModelError(nameof(addModel.Vomiting), "Необходимо е да посочите отговор.");
            }

            if (addModel.Aura == "noAnswer")
            {
                ModelState.AddModelError(nameof(addModel.Aura), "Необходимо е да посочите отговор.");
            }

            if (!ModelState.IsValid)
            {
                return View(addModel);
            }

            // Add headache.
            await this.headacheService.AddAsync(addModel, headacheDuration, currentUserId);

            // Get controller name and action name without using magic strings.
            string actionName = nameof(HeadacheController.GetAll);
            string controllerName = nameof(HeadacheController).Substring(0, nameof(HeadacheController).Length - "Controller".Length);

            return RedirectToAction(actionName, controllerName);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(int pageIndex = 1, int pageSize = 1, string orderByDate = "NewestFirst")
        {
            // Custom validation against web parameter tampering.
            if ((pageSize != 1 &&
                 pageSize != 5 &&
                 pageSize != 10) ||
                (orderByDate != "NewestFirst" &&
                 orderByDate != "OldestFirst"))
            {
                return BadRequest();
            }

            // Get current user's Id
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewData["currentUserId"] = userId;

            // Get paginated collection of view model according to given criteria.
            PaginatedList<RegisteredHeadacheViewModel> registeredHeadachesViewModel = await headacheService.GetRegisteredHeadachesAsync(userId, pageIndex, pageSize, orderByDate);

            // Send pagination size and ordering criteria to the view.
            ViewData["pageSize"] = pageSize;
            ViewData["orderByDate"] = orderByDate;

            return View(registeredHeadachesViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> SearchDoctor(string name)
        {
            if (name == null || name == string.Empty)
            {
                return BadRequest();
            }

            DoctorViewModel[] doctors = await this.headacheService.GetDoctorUsersByNameAsync(name);

            return Json(doctors);
        }

        [HttpPost]
        public async Task<IActionResult> ShareHeadache([FromBody] SharedHeadacheAddModel headache)
        {
            if (String.IsNullOrEmpty(headache.DoctorID) == false)
            {
                try
                {
                    await this.headacheService.Share(headache.HeadacheID, headache.DoctorID);

                    return Ok("Successfuly shared!");
                }
                catch (ArgumentException ex)
                {
                    // TODO: Log exception.
                    return BadRequest(ex.Message);
                }
            }

            return BadRequest();
        }
    }
}
