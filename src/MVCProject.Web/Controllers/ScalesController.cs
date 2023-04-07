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
            HIT6Scale[] currentUserScales = await this.dbContext.HIT6Scales
                                                                .Where(x => x.IsDeleted == false && x.PatientId == (string)ViewData["currentUserId"]!)
                                                                .AsNoTracking()
                                                                .OrderByDescending(x => x.CreatedOn)
                                                                .ToArrayAsync();

            // Initialize collection of view models
            List<HIT6ScaleViewModel> scalesViewModels = new List<HIT6ScaleViewModel>();

            // Fill the collection of view models
            foreach (HIT6Scale scale in currentUserScales)
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

        [HttpGet]
        public async Task<IActionResult> EditHIT6Scale(string id)
        {
            // Get current user's Id
            ViewData["currentUserId"] = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Get HIT6 scale
            HIT6Scale? hit6scale = await this.dbContext.HIT6Scales
                                           .FirstOrDefaultAsync(x => x.Id == id);

            // Check if scale exists and it belongs to the current user.
            // Protection against changing ID values through HTML and trying to get other user's scale.
            if (hit6scale == null || this.dbContext.Users
                                              .Where(x => x.Id == (string)ViewData["currentUserId"]!)
                                              .Include(x => x.HIT6Scales)
                                              .Any(x => x.HIT6Scales.Any(x => x.Id == id) == false))
            {
                return NotFound();
            }

            HIT6ScaleViewModel viewModel = new HIT6ScaleViewModel()
            {
                Id = hit6scale.Id,
                FirstQuestionAnswer = hit6scale.FirstQuestionAnswer,
                SecondQuestionAnswer = hit6scale.SecondQuestionAnswer,
                ThirdQuestionAnswer = hit6scale.ThirdQuestionAnswer,
                FourthQuestionAnswer = hit6scale.FourthQuestionAnswer,
                FifthQuestionAnswer = hit6scale.FifthQuestionAnswer,
                SixthQuestionAnswer = hit6scale.SixthQuestionAnswer,
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditHIT6Scale(HIT6ScaleViewModel viewModel, string currentUserId)
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
            ModelState[nameof(viewModel.FirstQuestionAnswer)]?.Errors.Clear();
            ModelState[nameof(viewModel.SecondQuestionAnswer)]?.Errors.Clear();
            ModelState[nameof(viewModel.ThirdQuestionAnswer)]?.Errors.Clear();
            ModelState[nameof(viewModel.FourthQuestionAnswer)]?.Errors.Clear();
            ModelState[nameof(viewModel.FifthQuestionAnswer)]?.Errors.Clear();
            ModelState[nameof(viewModel.SixthQuestionAnswer)]?.Errors.Clear();

            // Check if all Model properties have values.
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            // Custom model validation for changed radio button's value through page's HTML.
            if (this.HIT6ScaleService.ValidateAnswer(viewModel.FirstQuestionAnswer))
            {
                ModelState.AddModelError(nameof(viewModel.FirstQuestionAnswer), "Необходимо е да отбележите отговор.");
            }

            if (this.HIT6ScaleService.ValidateAnswer(viewModel.SecondQuestionAnswer))
            {
                ModelState.AddModelError(nameof(viewModel.SecondQuestionAnswer), "Необходимо е да отбележите отговор.");
            }

            if (this.HIT6ScaleService.ValidateAnswer(viewModel.ThirdQuestionAnswer))
            {
                ModelState.AddModelError(nameof(viewModel.ThirdQuestionAnswer), "Необходимо е да отбележите отговор.");
            }

            if (this.HIT6ScaleService.ValidateAnswer(viewModel.FourthQuestionAnswer))
            {
                ModelState.AddModelError(nameof(viewModel.FourthQuestionAnswer), "Необходимо е да отбележите отговор.");
            }

            if (this.HIT6ScaleService.ValidateAnswer(viewModel.FifthQuestionAnswer))
            {
                ModelState.AddModelError(nameof(viewModel.FifthQuestionAnswer), "Необходимо е да отбележите отговор.");
            }

            if (this.HIT6ScaleService.ValidateAnswer(viewModel.SixthQuestionAnswer))
            {
                ModelState.AddModelError(nameof(viewModel.SixthQuestionAnswer), "Необходимо е да отбележите отговор.");
            }

            // Check again ModelState if custom Model Errors are added.
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            // Add validated edited answers to string array (given as param to CalculateTotalScore method)
            string[] editedAnswers = new string[]
            {
                viewModel.FirstQuestionAnswer,
                viewModel.SecondQuestionAnswer,
                viewModel.ThirdQuestionAnswer,
                viewModel.FourthQuestionAnswer,
                viewModel.FifthQuestionAnswer,
                viewModel.SixthQuestionAnswer,
            };

            // Get HIT-6 scale that is going to be edited.
            HIT6Scale? HIT6Scale = await this.dbContext.HIT6Scales.FirstOrDefaultAsync(x => x.Id == viewModel.Id);

            // Boolean flag evaluating if logged user has the scale that is going to be edited.
            bool loggedUserHasScale = this.dbContext.Users
                                                    .Where(x => x.Id == currentUserId)
                                                    .Include(x => x.HIT6Scales)
                                                    .Any(x => x.HIT6Scales.Any(x => x.Id == viewModel.Id));

            // Check if hacker has tried to change HIT6Scale's Id through page's HTML.
            // If it has been changed, HIT6Scale eventually is going to be null (returned by FirstOrDefaultAsync method, line 295).

            // If it has been changed and somehow there is HIT6Scale in database with given GUID primary key (it's not null)
            // we check aswell if logged user is owner of the existing scale to reduce even more the chance
            // hacker could edit other user's registered HIT-6 scale. If these two validations fail - HTTP 404 is returned.
            if (HIT6Scale != null && loggedUserHasScale)
            {
                // Assign new values to edit the HIT-6 scale record in the database.
                HIT6Scale.FirstQuestionAnswer = viewModel.FirstQuestionAnswer;
                HIT6Scale.SecondQuestionAnswer = viewModel.SecondQuestionAnswer;
                HIT6Scale.ThirdQuestionAnswer = viewModel.ThirdQuestionAnswer;
                HIT6Scale.FourthQuestionAnswer = viewModel.FourthQuestionAnswer;
                HIT6Scale.FifthQuestionAnswer = viewModel.FifthQuestionAnswer;
                HIT6Scale.SixthQuestionAnswer = viewModel.SixthQuestionAnswer;
                HIT6Scale.PatientId = currentUserId;
                HIT6Scale.TotalScore = this.HIT6ScaleService.CalculateTotalScore(editedAnswers);
                HIT6Scale.CreatedOn = DateTime.UtcNow;

                // Save changes in database.
                await this.dbContext.SaveChangesAsync();
            }
            else
            {
                return NotFound();
            }
            

            // Get controller's name and action's name without using magic strings.
            string actionName = nameof(ScalesController.MyHIT6Scales);
            string controllerName = nameof(ScalesController).Substring(0, nameof(ScalesController).Length - "Controller".Length);

            return RedirectToAction(actionName, controllerName);
        }
    }
}
