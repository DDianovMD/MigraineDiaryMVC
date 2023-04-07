using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MigraineDiary.Web.Data;
using MigraineDiary.Web.Data.DbModels;
using MigraineDiary.Web.Models;
using MigraineDiary.Web.Services;
using MigraineDiary.Web.Services.Contracts;
using System.Security.Claims;

namespace MigraineDiary.Web.Controllers
{
    [Authorize]
    public class ScalesController : Controller
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IHIT6ScaleService HIT6ScaleService;

        public ScalesController(ApplicationDbContext dbContext,
            IHIT6ScaleService HIT6scaleService)
        {
            this.dbContext = dbContext;
            this.HIT6ScaleService = HIT6scaleService;
        }

        public IActionResult Index()
        {
            // Get controller's name and action's name without using magic strings.
            string actionName = nameof(ScalesController.AddHIT6Scale);
            string controllerName = nameof(ScalesController).Substring(0, nameof(ScalesController).Length - "Controller".Length);

            return RedirectToAction(actionName, controllerName);
        }

        [HttpGet]
        public IActionResult AddHIT6Scale()
        {
            ViewData["currentUserId"] = User.FindFirstValue(ClaimTypes.NameIdentifier);

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddHIT6Scale(HIT6ScaleAddModel addModel, string currentUserId)
        {
            // Get current user's Id if ModelState is not valid and it's needed to be routed.
            // If not routed on next submission currentUserId is null and ModelState is not valid again.
            ViewData["currentUserId"] = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Check if hacker is trying to change userId through page's HTML with other user's Id.
            // Return NotFound if sended user Id is different.
            if (currentUserId != ViewData["currentUserId"]!.ToString())
            {
                return NotFound();
            }

            // Reset custom added model errors for next validation cycle.
            // If reset is skipped even with correct data submission on next user input, ModelState continues to not be valid!
            ModelState[nameof(addModel.FirstQuestionAnswer)]?.Errors.Clear();
            ModelState[nameof(addModel.SecondQuestionAnswer)]?.Errors.Clear();
            ModelState[nameof(addModel.ThirdQuestionAnswer)]?.Errors.Clear();
            ModelState[nameof(addModel.FourthQuestionAnswer)]?.Errors.Clear();
            ModelState[nameof(addModel.FifthQuestionAnswer)]?.Errors.Clear();
            ModelState[nameof(addModel.SixthQuestionAnswer)]?.Errors.Clear();

            // Check if all Model properties have values.
            // Here default values are evaluated as valid so it's needed custom validation if this one pass.
            if (!ModelState.IsValid)
            {
                return View(addModel);
            }

            // Custom model validation for left questions unanswered and sent with default value
            // or sent with changed radio button's value through page's HTML.
            if (this.HIT6ScaleService.ValidateAnswer(addModel.FirstQuestionAnswer))
            {
                ModelState.AddModelError(nameof(addModel.FirstQuestionAnswer), "Необходимо е да отбележите отговор.");
            }

            if (this.HIT6ScaleService.ValidateAnswer(addModel.SecondQuestionAnswer))
            {
                ModelState.AddModelError(nameof(addModel.SecondQuestionAnswer), "Необходимо е да отбележите отговор.");
            }

            if (this.HIT6ScaleService.ValidateAnswer(addModel.ThirdQuestionAnswer))
            {
                ModelState.AddModelError(nameof(addModel.ThirdQuestionAnswer), "Необходимо е да отбележите отговор.");
            }

            if (this.HIT6ScaleService.ValidateAnswer(addModel.FourthQuestionAnswer))
            {
                ModelState.AddModelError(nameof(addModel.FourthQuestionAnswer), "Необходимо е да отбележите отговор.");
            }

            if (this.HIT6ScaleService.ValidateAnswer(addModel.FifthQuestionAnswer))
            {
                ModelState.AddModelError(nameof(addModel.FifthQuestionAnswer), "Необходимо е да отбележите отговор.");
            }

            if (this.HIT6ScaleService.ValidateAnswer(addModel.SixthQuestionAnswer))
            {
                ModelState.AddModelError(nameof(addModel.SixthQuestionAnswer), "Необходимо е да отбележите отговор.");
            }

            // Check again ModelState if custom Model Errors are added.
            if (!ModelState.IsValid)
            {
                return View(addModel);
            }

            // Assign valid answers to DbModel
            string[] answers = new string[]
            {
                addModel.FirstQuestionAnswer,
                addModel.SecondQuestionAnswer,
                addModel.ThirdQuestionAnswer,
                addModel.FourthQuestionAnswer,
                addModel.FifthQuestionAnswer,
                addModel.SixthQuestionAnswer,
            };

            HIT6Scale HIT6Scale = new HIT6Scale()
            {
                FirstQuestionAnswer = addModel.FirstQuestionAnswer,
                SecondQuestionAnswer = addModel.SecondQuestionAnswer,
                ThirdQuestionAnswer = addModel.ThirdQuestionAnswer,
                FourthQuestionAnswer = addModel.FourthQuestionAnswer,
                FifthQuestionAnswer = addModel.FifthQuestionAnswer,
                SixthQuestionAnswer = addModel.SixthQuestionAnswer,
                PatientId = currentUserId,
                TotalScore = this.HIT6ScaleService.CalculateTotalScore(answers),
            };

            await this.dbContext.HIT6Scales.AddAsync(HIT6Scale);
            await this.dbContext.SaveChangesAsync();

            // Get controller's name and action's name without using magic strings.
            string actionName = nameof(HomeController.Index);
            string controllerName = nameof(HomeController).Substring(0, nameof(HomeController).Length - "Controller".Length);

            return RedirectToAction(actionName, controllerName);
        }

        [HttpGet]
        public async Task<IActionResult> MyHIT6Scales()
        {
            // Get current user's Id
            ViewData["currentUserId"] = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Get user's scales ordered by last added first
            HIT6Scale[] currentUserScales = await dbContext.HIT6Scales
                                                      .Where(x => x.IsDeleted == false && x.PatientId == (string)ViewData["currentUserId"]!)
                                                      .AsNoTracking()
                                                      .OrderByDescending(x => x.CreatedOn)
                                                      .ToArrayAsync();

            // Initialize collection of view models
            List<HIT6ScaleViewModel> scalesViewModels = new List<HIT6ScaleViewModel>();

            // Fill the collection of view models
            foreach (var scale in currentUserScales)
            {
                HIT6ScaleViewModel currentScale = new HIT6ScaleViewModel
                {
                    Id = scale.Id,
                    FirstQuestionAnswer = scale.FirstQuestionAnswer,
                    SecondQuestionAnswer = scale.SecondQuestionAnswer,
                    ThirdQuestionAnswer = scale.ThirdQuestionAnswer,
                    FourthQuestionAnswer = scale.FourthQuestionAnswer,
                    FifthQuestionAnswer = scale.FifthQuestionAnswer,
                    SixthQuestionAnswer = scale.SixthQuestionAnswer,
                    TotalScore = scale.TotalScore,
                    CreatedOn = scale.CreatedOn,
                };

                scalesViewModels.Add(currentScale);
            }

            return View(scalesViewModels);
        }
    }
}
