using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MigraineDiary.Web.Models;
using MigraineDiary.Web.Services.Contracts;
using System.Security.Claims;

namespace MigraineDiary.Web.Controllers
{
    [Authorize(Roles = "Admin,Doctor")]
    public class TrialController : Controller
    {
        private IWebHostEnvironment environment;
        private readonly IClinicalTrialService trialService;

        public TrialController(IWebHostEnvironment environment,
                             IClinicalTrialService trialService)
        {
            this.environment = environment;
            this.trialService = trialService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Add()
        {
            ClinicalTrialAddModel model = new ClinicalTrialAddModel();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(ClinicalTrialAddModel addModel)
        {
            // Reset custom added model errors for next validation cycle.
            // If reset is skipped even with correct data submission on next user input, ModelState continues to not be valid!
            ModelState[nameof(addModel.TrialAgreementDocument)]?.Errors.Clear();
            ModelState[nameof(addModel.TrialAgreementDocument)]?.Errors.Clear();
            ModelState[nameof(addModel.Practicioners)]?.Errors.Clear();

            if (!ModelState.IsValid)
            {
                // Add custom validation if ModelState is not valid.
                if (addModel.TrialAgreementDocument == null)
                {
                    ModelState.AddModelError(nameof(addModel.TrialAgreementDocument), "Необходимо е да приложите файл с описание и информирано съгласие.");
                }

                if (addModel.Practicioners.Any(x => x.FirstName == null))
                {
                    ModelState.AddModelError(nameof(addModel.Practicioners), "Необходимо е да въведете име и фамилия на изследователите.");
                }
                
                return View();
            }

            // Add custom error messages for Practicioner's first name and last name length.

            if (addModel.Practicioners.Any(x => x.FirstName.Length < 3 || x.FirstName.Length > 30) ||
                addModel.Practicioners.Any(x => x.Lastname.Length < 3 || x.Lastname.Length > 30))
            {
                ModelState.AddModelError(nameof(addModel.Practicioners), "Дължината на името трябва да бъде между 3 и 30 символа.");

                return View();
            }

            // Get uploaded file's extension.
            string fileFormatExtension = addModel.TrialAgreementDocument.FileName.Split('.')[1];

            // Check if uploaded file isn't pdf.
            if (fileFormatExtension != "pdf")
            {
                ModelState.AddModelError(nameof(addModel.TrialAgreementDocument), "Не са позволени файлови формати, различни от pdf.");

                return View();
            }

            // Add custom validation for file format extension and file size.

            // Check if file is bigger than 25 MB.
            if (addModel.TrialAgreementDocument.Length > 25000000)
            {
                ModelState.AddModelError(nameof(addModel.TrialAgreementDocument), "Големината на файла не може да надвишава 25 MB.");

                return View();
            }

            // Generate unique file name.
            string fileName = this.trialService.GenerateUniqueFileName(fileFormatExtension);

            // Save file in filesystem.
            string saveDirectory = Path.Combine(environment.WebRootPath, fileName);
            using FileStream fs = new FileStream(saveDirectory, FileMode.Create);
            await addModel.TrialAgreementDocument.CopyToAsync(fs);

            // Get current user ID
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Save clinical trial in database.
            await this.trialService.AddAsync(addModel, fileName, userId);

            // Get controller's name and action's name without using magic strings.
            string actionName = nameof(ScalesController.MyZungScales);
            string controllerName = nameof(ScalesController).Substring(0, nameof(ScalesController).Length - "Controller".Length);

            return RedirectToAction(actionName, controllerName);
        }
    }
}
