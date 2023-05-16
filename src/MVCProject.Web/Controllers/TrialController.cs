using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MigraineDiary.Services.Contracts;
using MigraineDiary.ViewModels;
using System.Security.Claims;
using System.Net.Mime;

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
        [AllowAnonymous]
        public async Task<IActionResult> Index(int pageIndex = 1, int pageSize = 1, string orderByDate = "NewestFirst")
        {
            // Custom validation against web parameter tampering
            if ((pageSize != 1 &&
                 pageSize != 5 &&
                 pageSize != 10) ||
                (orderByDate != "NewestFirst" &&
                 orderByDate != "OldestFirst"))
            {
                return BadRequest();
            }

            // Get all trials
            PaginatedList<ClinicalTrialViewModel> trials = await this.trialService.GetAllTrialsAsync(pageIndex, pageSize, orderByDate);

            // Send pagination size and ordering criteria to the view.
            ViewData["pageSize"] = pageSize;
            ViewData["orderByDate"] = orderByDate;

            return View(trials);
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
            
            // Add custom validation for file format extension and file size.

            // Get uploaded file's extension.
            string fileFormatExtension = addModel.TrialAgreementDocument.FileName.Split('.')[1];

            // Check if uploaded file isn't pdf.
            if (fileFormatExtension != "pdf")
            {
                ModelState.AddModelError(nameof(addModel.TrialAgreementDocument), "Не са позволени файлови формати, различни от pdf.");

                return View();
            }

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
            string actionName = nameof(TrialController.Index);
            string controllerName = nameof(TrialController).Substring(0, nameof(TrialController).Length - "Controller".Length);

            return RedirectToAction(actionName, controllerName);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Download(string documentName)
        {
            string filePath = Path.Combine(environment.WebRootPath, documentName);

            if (System.IO.File.Exists(filePath))
            {
                byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);

                return File(fileBytes, MediaTypeNames.Application.Pdf, "Информирано съгласие.pdf");
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet]
        public async Task<IActionResult> MyTrials(int pageIndex = 1, int pageSize = 1, string orderByDate = "NewestFirst")
        {
            // Custom validation against web parameter tampering
            if ((pageSize != 1 &&
                 pageSize != 5 &&
                 pageSize != 10) ||
                (orderByDate != "NewestFirst" &&
                 orderByDate != "OldestFirst"))
            {
                return BadRequest();
            }

            // Get logged user's ID
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Get all trials registered by this specific user.
            PaginatedList<ClinicalTrialViewModel> trials = await this.trialService.GetAllTrialsAsync(pageIndex, pageSize, orderByDate, userId);

            // Send pagination size and ordering criteria to the view.
            ViewData["pageSize"] = pageSize;
            ViewData["orderByDate"] = orderByDate;

            return View(trials);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string trialId, string creatorId)
        {
            // Get logged user Id.
            string userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Check for parameter tampering.
            if (!ModelState.IsValid || creatorId != userId)
            {
                return BadRequest();
            }

            // Get trial which is going to be edited.
            ClinicalTrialEditModel trialModel = await this.trialService.GetByIdAsync(trialId, userId);

            return View(trialModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ClinicalTrialEditModel editModel)
        {
            // Reset custom added model errors for next validation cycle.
            // If reset is skipped even with correct data submission on next user input, ModelState continues to not be valid!
            ModelState[nameof(editModel.TrialAgreementDocument)]?.Errors.Clear();
            ModelState[nameof(editModel.Practicioners)]?.Errors.Clear();

            if (!ModelState.IsValid)
            {
                // Add custom validation if ModelState is not valid.
                if (editModel.Practicioners.Any(x => x.FirstName == null) ||
                    editModel.Practicioners.Any(x => x.Lastname == null))
                {
                    ModelState.AddModelError(nameof(editModel.Practicioners), "Необходимо е да въведете име и фамилия на изследователите.");
                }

                return View(editModel);
            }

            // Add custom error messages for Practicioner's first name and last name length.

            if (editModel.Practicioners.Any(x => x.FirstName.Length < 3 || x.FirstName.Length > 30) ||
                editModel.Practicioners.Any(x => x.Lastname.Length < 3 || x.Lastname.Length > 30))
            {
                ModelState.AddModelError(nameof(editModel.Practicioners), "Дължината на името трябва да бъде между 3 и 30 символа.");

                return View(editModel);
            }

            // Add custom validation for file format extension and file size.

            // Check if user has uploaded new file.
            if (editModel.TrialAgreementDocument != null)
            {
                // Get uploaded file's extension.
                string fileFormatExtension = editModel.TrialAgreementDocument.FileName.Split('.')[1];

                // Check if uploaded file isn't pdf.
                if (fileFormatExtension != "pdf")
                {
                    ModelState.AddModelError(nameof(editModel.TrialAgreementDocument), "Не са позволени файлови формати, различни от pdf.");

                    return View(editModel);
                }

                // Check if file is bigger than 25 MB.
                if (editModel.TrialAgreementDocument.Length > 25000000)
                {
                    ModelState.AddModelError(nameof(editModel.TrialAgreementDocument), "Големината на файла не може да надвишава 25 MB.");

                    return View(editModel);
                }

                // Create delete path.
                string deleteDirectory = Path.Combine(environment.WebRootPath, editModel.AgreementDocumentName);

                // Delete previously uploaded pdf file.
                System.IO.File.Delete(deleteDirectory);

                // Generate unique file name.
                editModel.AgreementDocumentName = this.trialService.GenerateUniqueFileName(fileFormatExtension);

                // Save file in filesystem.
                string saveDirectory = Path.Combine(environment.WebRootPath, editModel.AgreementDocumentName);
                using FileStream fs = new FileStream(saveDirectory, FileMode.Create);
                await editModel.TrialAgreementDocument.CopyToAsync(fs);
            }

            // Save edited settings
            await this.trialService.EditTrialAsync(editModel);

            // Get controller's name and action's name without using magic strings.
            string actionName = nameof(TrialController.MyTrials);
            string controllerName = nameof(TrialController).Substring(0, nameof(TrialController).Length - "Controller".Length);

            return RedirectToAction(actionName, controllerName);
        }
    }
}
