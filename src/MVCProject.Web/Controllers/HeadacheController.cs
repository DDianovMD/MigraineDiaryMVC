using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
                HeadacheDuration = this.headacheService.CalculateDuration(addModel.EndTime, addModel.Onset),
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
    }
}
