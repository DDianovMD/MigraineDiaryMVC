using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MigraineDiary.Web.Data;
using MigraineDiary.Web.Data.DbModels;
using MigraineDiary.Web.Models;
using System.Security.Claims;

namespace MigraineDiary.Web.Controllers
{
    [Authorize]
    public class ScalesController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public ScalesController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
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
            if (addModel.FifthQuestionAnswer == "NoAnswer" ||
                (addModel.FirstQuestionAnswer != "Never" &&
                addModel.FirstQuestionAnswer != "Rarely" &&
                addModel.FirstQuestionAnswer != "Sometimes" &&
                addModel.FirstQuestionAnswer != "Very often" &&
                addModel.FirstQuestionAnswer != "Always"))
            {
                ModelState.AddModelError(nameof(addModel.FirstQuestionAnswer), "Необходимо е да отбележите отговор.");
            }
            
            if (addModel.SecondQuestionAnswer == "NoAnswer" ||
                (addModel.SecondQuestionAnswer != "Never" &&
                addModel.SecondQuestionAnswer != "Rarely" &&
                addModel.SecondQuestionAnswer != "Sometimes" &&
                addModel.SecondQuestionAnswer != "Very often" &&
                addModel.SecondQuestionAnswer != "Always"))
            {
                ModelState.AddModelError(nameof(addModel.SecondQuestionAnswer), "Необходимо е да отбележите отговор.");
            }
            
            if (addModel.ThirdQuestionAnswer == "NoAnswer" ||
                (addModel.ThirdQuestionAnswer != "Never" &&
                addModel.ThirdQuestionAnswer != "Rarely" &&
                addModel.ThirdQuestionAnswer != "Sometimes" &&
                addModel.ThirdQuestionAnswer != "Very often" &&
                addModel.ThirdQuestionAnswer != "Always"))
            {
                ModelState.AddModelError(nameof(addModel.ThirdQuestionAnswer), "Необходимо е да отбележите отговор.");
            }
            
            if (addModel.FourthQuestionAnswer == "NoAnswer" ||
                (addModel.FourthQuestionAnswer != "Never" &&
                addModel.FourthQuestionAnswer != "Rarely" &&
                addModel.FourthQuestionAnswer != "Sometimes" &&
                addModel.FourthQuestionAnswer != "Very often" &&
                addModel.FourthQuestionAnswer != "Always"))
            {
                ModelState.AddModelError(nameof(addModel.FourthQuestionAnswer), "Необходимо е да отбележите отговор.");
            }
            
            if (addModel.FifthQuestionAnswer == "NoAnswer" ||
                (addModel.FifthQuestionAnswer != "Never" &&
                addModel.FifthQuestionAnswer != "Rarely" &&
                addModel.FifthQuestionAnswer != "Sometimes" &&
                addModel.FifthQuestionAnswer != "Very often" &&
                addModel.FifthQuestionAnswer != "Always"))
            {
                ModelState.AddModelError(nameof(addModel.FifthQuestionAnswer), "Необходимо е да отбележите отговор.");
            }
            
            if (addModel.SixthQuestionAnswer == "NoAnswer" ||
                (addModel.SixthQuestionAnswer != "Never" &&
                addModel.SixthQuestionAnswer != "Rarely" &&
                addModel.SixthQuestionAnswer != "Sometimes" &&
                addModel.SixthQuestionAnswer != "Very often" &&
                addModel.SixthQuestionAnswer != "Always"))
            {
                ModelState.AddModelError(nameof(addModel.SixthQuestionAnswer), "Необходимо е да отбележите отговор.");
            }

            // Check again ModelState if custom Model Errors are added.
            if (!ModelState.IsValid)
            {
                return View(addModel);
            }

            // Assign valid answers to DbModel
            HIT6Scale HIT6Scale = new HIT6Scale()
            {
                FirstQuestionAnswer = addModel.FirstQuestionAnswer,
                SecondQuestionAnswer = addModel.SecondQuestionAnswer,
                ThirdQuestionAnswer = addModel.ThirdQuestionAnswer,
                FourthQuestionAnswer = addModel.FourthQuestionAnswer,
                FifthQuestionAnswer = addModel.FifthQuestionAnswer,
                SixthQuestionAnswer = addModel.SixthQuestionAnswer,
                PatientId = currentUserId,
                // TODO: Add HIT6ScaleService and implement method to calculate total score and assign it.
                
                // TotalScore = HIT6ScaleService.CalculateTotalScore(); - must be implemented.
            };

            await this.dbContext.HIT6Scales.AddAsync(HIT6Scale);
            await this.dbContext.SaveChangesAsync();

            // Get controller's name and action's name without using magic strings.
            string actionName = nameof(HomeController.Index);
            string controllerName = nameof(HomeController).Substring(0, nameof(HomeController).Length - "Controller".Length);

            return RedirectToAction(actionName, controllerName);
        }
    }
}
