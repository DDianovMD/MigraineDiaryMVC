using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MigraineDiary.Web.Data;
using MigraineDiary.Web.Data.DbModels;
using MigraineDiary.Web.Models;
using MigraineDiary.Web.Services.Contracts;
using System.Security.Claims;

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
            ViewData["currentUserId"] = User.FindFirstValue(ClaimTypes.NameIdentifier);

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(HeadacheAddFormModel addModel, string currentUserId)
        {
            // Get current user's Id if ModelState is not valid and it's needed to be routed.
            // If not routed on next submission currentUserId is null and ModelState is not valid again.
            ViewData["currentUserId"] = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Reset custom added model errors for next validation cycle.
            // If reset is skipped even with correct data submission on next user input, ModelState continues to not be valid!
            ModelState[nameof(addModel.EndTime)]?.Errors.Clear();

            // Check if all forms submitted have values.
            if (!ModelState.IsValid)
            {
                return View(addModel);
            }

            // Get the headache duration so it can be custom validated not only if data is submitted.
            Dictionary<string, int> headacheDuration = this.headacheService.CalculateDuration(addModel.Onset, addModel.EndTime);

            // Custom headache duration validations.

            // If headache ended before it started scenario.
            if (headacheDuration["Days"] < 0 ||
                headacheDuration["Hours"] < 0 ||
                headacheDuration["Minutes"] < 0)
            {
                ModelState.AddModelError(nameof(addModel.EndTime), "Края на главоболието не може да бъде преди началото му.");

                return View(addModel);
            }
            // Else if headache ended at the moment it just started scenario.
            else if (headacheDuration["Days"] <= 0 &&
                     headacheDuration["Hours"] <= 0 &&
                     headacheDuration["Minutes"] == 0)
            {
                ModelState.AddModelError(nameof(addModel.EndTime), "Началото и краят на главоболието не могат да съвпадат.");
            }

            // Check again for added model errors on custom validation.
            if (!ModelState.IsValid)
            {
                return View(addModel);
            }

            Headache headache = new Headache
            {
                PatientId = currentUserId,
                Aura = addModel.Aura,
                AuraDescriptionNotes = addModel.AuraDescriptionNotes,
                Onset = addModel.Onset,
                EndTime = addModel.EndTime,
                DurationDays = headacheDuration["Days"],
                DurationHours = headacheDuration["Hours"],
                DurationMinutes = headacheDuration["Minutes"],
                Severity = addModel.Severity,
                LocalizationSide = addModel.LocalizationSide,
                PainCharacteristics = addModel.PainCharacteristics,
                Photophoby = addModel.Photophoby,
                Phonophoby = addModel.Phonophoby,
                Nausea = addModel.Nausea,
                Vomiting = addModel.Vomiting,
                Triggers = addModel.Triggers,
            };

            if (addModel.MedicationsTaken.Count > 0)
            {
                foreach (var medication in addModel.MedicationsTaken)
                {
                    Medication currentMedication = new Medication
                    {
                        Name = medication.Name,
                        SinglePillDosage = medication.SinglePillDosage,
                        Units = medication.Units,
                        NumberOfTakenPills = medication.NumberOfTakenPills,
                    };

                    headache.MedicationsTaken.Add(currentMedication);
                }
            }

            await this.dbContext.Headaches.AddAsync(headache);
            await this.dbContext.SaveChangesAsync();

            // Get controller name and action name without using magic strings
            string actionName = nameof(HomeController.Index);
            string controllerName = nameof(HomeController).Substring(0, nameof(HomeController).Length - "Controller".Length);

            return RedirectToAction(actionName, controllerName);
        }

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
    }
}
