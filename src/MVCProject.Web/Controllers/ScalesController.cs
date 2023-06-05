using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MigraineDiary.Services.Contracts;
using MigraineDiary.ViewModels;
using System.Security.Claims;

namespace MigraineDiary.Web.Controllers
{
    [Authorize]
    public class ScalesController : Controller
    {
        private readonly IHIT6ScaleService HIT6ScaleService;
        private readonly IZungScaleForAnxietyService ZungScaleForAnxietyService;

        public ScalesController(IHIT6ScaleService HIT6scaleService,
                      IZungScaleForAnxietyService ZungScaleForAnxietyService)
        {
            this.HIT6ScaleService = HIT6scaleService;
            this.ZungScaleForAnxietyService = ZungScaleForAnxietyService;
        }

        [HttpGet]
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

            try
            {
                await this.HIT6ScaleService.AddAsync(addModel, currentUserId);

                // Get controller's name and action's name without using magic strings.
                string actionName = nameof(ScalesController.MyHIT6Scales);
                string controllerName = nameof(ScalesController).Substring(0, nameof(ScalesController).Length - "Controller".Length);

                return RedirectToAction(actionName, controllerName);
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }

        [HttpGet]
        public async Task<IActionResult> MyHIT6Scales(int pageIndex = 1, int pageSize = 1, string orderByDate = "NewestFirst")
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

            // Get current user's Id.
            string currentUserId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewData["currentUserId"] = currentUserId;

            // Get user's scales.
            PaginatedList<HIT6ScaleViewModel> currentUserScales = await HIT6ScaleService.GetAllAsync(currentUserId, pageIndex, pageSize, orderByDate);

            // Send pagination size and ordering criteria to the view.
            ViewData["pageSize"] = pageSize;
            ViewData["orderByDate"] = orderByDate;

            return View(currentUserScales);
        }

        [HttpGet]
        public async Task<IActionResult> EditHIT6Scale(string id)
        {
            // Get current user's Id.
            string currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewData["currentUserId"] = currentUserId;

            // Get HIT6 scale
            HIT6ScaleViewModel hit6scaleViewModel = await HIT6ScaleService.GetByIdAsync(id, currentUserId);

            // Check if scale exists and it belongs to the current user.
            if (hit6scaleViewModel == null)
            {
                return NotFound();
            }

            return View(hit6scaleViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditHIT6Scale(HIT6ScaleViewModel viewModel, string userId)
        {
            // Get current user's Id if ModelState is not valid and it's needed to be routed.
            // If not routed on next submission currentUserId is null and ModelState is not valid again.
            string currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewData["currentUserId"] = currentUserId;

            // Check if hacker is trying to change userId through page's HTML with other user's Id.
            // Return NotFound if sended user Id is different.
            if (userId != currentUserId)
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

            // Add validated edited answers to string array (given as param to EditAsync method).
            string[] editedAnswers = new string[]
            {
                viewModel.FirstQuestionAnswer,
                viewModel.SecondQuestionAnswer,
                viewModel.ThirdQuestionAnswer,
                viewModel.FourthQuestionAnswer,
                viewModel.FifthQuestionAnswer,
                viewModel.SixthQuestionAnswer,
            };

            try
            {
                await this.HIT6ScaleService.EditAsync(viewModel.Id, currentUserId, editedAnswers);

                // Get controller's name and action's name without using magic strings.
                string actionName = nameof(ScalesController.MyHIT6Scales);
                string controllerName = nameof(ScalesController).Substring(0, nameof(ScalesController).Length - "Controller".Length);

                return RedirectToAction(actionName, controllerName);
            }
            catch (ArgumentException ae)
            {
                return NotFound();
            }
        }

        [HttpGet]
        public async Task<IActionResult> DeleteHIT6Scale(string hit6scaleId, string currentUserId)
        {
            // Get current user's Id.
            currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewData["currentUserId"] = currentUserId;

            // Check if hacker is trying to change userId through page's HTML with other user's Id.
            // Return NotFound if sent user Id is different.
            if (currentUserId != ViewData["currentUserId"]!.ToString())
            {
                return NotFound();
            }

            try
            {
                await this.HIT6ScaleService.SoftDeleteAsync(hit6scaleId, currentUserId);

                // Get controller's name and action's name without using magic strings.
                string actionName = nameof(ScalesController.MyHIT6Scales);
                string controllerName = nameof(ScalesController).Substring(0, nameof(ScalesController).Length - "Controller".Length);

                return RedirectToAction(actionName, controllerName);
            }
            catch (ArgumentException ex)
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> ShareHIT6Scale([FromBody] SharedHIT6ScaleAddModel scale)
        {
            if (String.IsNullOrEmpty(scale.DoctorID) == false)
            {
                try
                {
                    await this.HIT6ScaleService.Share(scale.ScaleID, scale.DoctorID);

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

        [HttpGet]
        public IActionResult AddZungScale()
        {
            ViewData["currentUserId"] = User.FindFirstValue(ClaimTypes.NameIdentifier);

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddZungScale(ZungScaleAddModel addModel, string currentUserId)
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
            ModelState[nameof(addModel.SeventhQuestionAnswer)]?.Errors.Clear();
            ModelState[nameof(addModel.EighthQuestionAnswer)]?.Errors.Clear();
            ModelState[nameof(addModel.NinthQuestionAnswer)]?.Errors.Clear();
            ModelState[nameof(addModel.TenthQuestionAnswer)]?.Errors.Clear();
            ModelState[nameof(addModel.EleventhQuestionAnswer)]?.Errors.Clear();
            ModelState[nameof(addModel.TwelfthQuestionAnswer)]?.Errors.Clear();
            ModelState[nameof(addModel.ThirteenthQuestionAnswer)]?.Errors.Clear();
            ModelState[nameof(addModel.FourteenthQuestionAnswer)]?.Errors.Clear();
            ModelState[nameof(addModel.FifteenthQuestionAnswer)]?.Errors.Clear();
            ModelState[nameof(addModel.SixteenthQuestionAnswer)]?.Errors.Clear();
            ModelState[nameof(addModel.SeventeenthQuestionAnswer)]?.Errors.Clear();
            ModelState[nameof(addModel.EighteenthQuestionAnswer)]?.Errors.Clear();
            ModelState[nameof(addModel.NineteenthQuestionAnswer)]?.Errors.Clear();
            ModelState[nameof(addModel.TwentiethQuestionAnswer)]?.Errors.Clear();

            // Check if all Model properties have values.
            // Here default values are evaluated as valid so it's needed custom validation if this one pass.
            if (!ModelState.IsValid)
            {
                return View(addModel);
            }

            // Custom model validation for changed radio button's value through page's HTML.
            if (this.ZungScaleForAnxietyService.ValidateAnswer(addModel.FirstQuestionAnswer))
            {
                ModelState.AddModelError(nameof(addModel.FirstQuestionAnswer), "Необходимо е да отбележите отговор.");
            }

            if (this.ZungScaleForAnxietyService.ValidateAnswer(addModel.SecondQuestionAnswer))
            {
                ModelState.AddModelError(nameof(addModel.SecondQuestionAnswer), "Необходимо е да отбележите отговор.");
            }

            if (this.ZungScaleForAnxietyService.ValidateAnswer(addModel.ThirdQuestionAnswer))
            {
                ModelState.AddModelError(nameof(addModel.ThirdQuestionAnswer), "Необходимо е да отбележите отговор.");
            }

            if (this.ZungScaleForAnxietyService.ValidateAnswer(addModel.FourthQuestionAnswer))
            {
                ModelState.AddModelError(nameof(addModel.FourthQuestionAnswer), "Необходимо е да отбележите отговор.");
            }

            if (this.ZungScaleForAnxietyService.ValidateAnswer(addModel.FifthQuestionAnswer))
            {
                ModelState.AddModelError(nameof(addModel.FifthQuestionAnswer), "Необходимо е да отбележите отговор.");
            }

            if (this.ZungScaleForAnxietyService.ValidateAnswer(addModel.SixthQuestionAnswer))
            {
                ModelState.AddModelError(nameof(addModel.SixthQuestionAnswer), "Необходимо е да отбележите отговор.");
            }

            if (this.ZungScaleForAnxietyService.ValidateAnswer(addModel.SeventhQuestionAnswer))
            {
                ModelState.AddModelError(nameof(addModel.SeventhQuestionAnswer), "Необходимо е да отбележите отговор.");
            }

            if (this.ZungScaleForAnxietyService.ValidateAnswer(addModel.EighthQuestionAnswer))
            {
                ModelState.AddModelError(nameof(addModel.EighthQuestionAnswer), "Необходимо е да отбележите отговор.");
            }

            if (this.ZungScaleForAnxietyService.ValidateAnswer(addModel.NinthQuestionAnswer))
            {
                ModelState.AddModelError(nameof(addModel.NinthQuestionAnswer), "Необходимо е да отбележите отговор.");
            }

            if (this.ZungScaleForAnxietyService.ValidateAnswer(addModel.TenthQuestionAnswer))
            {
                ModelState.AddModelError(nameof(addModel.TenthQuestionAnswer), "Необходимо е да отбележите отговор.");
            }

            if (this.ZungScaleForAnxietyService.ValidateAnswer(addModel.EleventhQuestionAnswer))
            {
                ModelState.AddModelError(nameof(addModel.EleventhQuestionAnswer), "Необходимо е да отбележите отговор.");
            }

            if (this.ZungScaleForAnxietyService.ValidateAnswer(addModel.TwelfthQuestionAnswer))
            {
                ModelState.AddModelError(nameof(addModel.TwelfthQuestionAnswer), "Необходимо е да отбележите отговор.");
            }

            if (this.ZungScaleForAnxietyService.ValidateAnswer(addModel.ThirteenthQuestionAnswer))
            {
                ModelState.AddModelError(nameof(addModel.ThirteenthQuestionAnswer), "Необходимо е да отбележите отговор.");
            }

            if (this.ZungScaleForAnxietyService.ValidateAnswer(addModel.FourteenthQuestionAnswer))
            {
                ModelState.AddModelError(nameof(addModel.FourteenthQuestionAnswer), "Необходимо е да отбележите отговор.");
            }

            if (this.ZungScaleForAnxietyService.ValidateAnswer(addModel.FifteenthQuestionAnswer))
            {
                ModelState.AddModelError(nameof(addModel.FifteenthQuestionAnswer), "Необходимо е да отбележите отговор.");
            }

            if (this.ZungScaleForAnxietyService.ValidateAnswer(addModel.SixteenthQuestionAnswer))
            {
                ModelState.AddModelError(nameof(addModel.SixteenthQuestionAnswer), "Необходимо е да отбележите отговор.");
            }

            if (this.ZungScaleForAnxietyService.ValidateAnswer(addModel.SeventeenthQuestionAnswer))
            {
                ModelState.AddModelError(nameof(addModel.SeventeenthQuestionAnswer), "Необходимо е да отбележите отговор.");
            }

            if (this.ZungScaleForAnxietyService.ValidateAnswer(addModel.EighteenthQuestionAnswer))
            {
                ModelState.AddModelError(nameof(addModel.EighteenthQuestionAnswer), "Необходимо е да отбележите отговор.");
            }

            if (this.ZungScaleForAnxietyService.ValidateAnswer(addModel.NineteenthQuestionAnswer))
            {
                ModelState.AddModelError(nameof(addModel.NineteenthQuestionAnswer), "Необходимо е да отбележите отговор.");
            }

            if (this.ZungScaleForAnxietyService.ValidateAnswer(addModel.TwentiethQuestionAnswer))
            {
                ModelState.AddModelError(nameof(addModel.TwentiethQuestionAnswer), "Необходимо е да отбележите отговор.");
            }

            // Check again ModelState if custom Model Errors are added.
            if (!ModelState.IsValid)
            {
                return View(addModel);
            }

            try
            {
                await this.ZungScaleForAnxietyService.AddAsync(addModel, currentUserId);

                // Get controller's name and action's name without using magic strings.
                string actionName = nameof(ScalesController.MyZungScales);
                string controllerName = nameof(ScalesController).Substring(0, nameof(ScalesController).Length - "Controller".Length);

                return RedirectToAction(actionName, controllerName);
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }

        [HttpGet]
        public async Task<IActionResult> MyZungScales(int pageIndex = 1, int pageSize = 1, string orderByDate = "NewestFirst")
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

            // Get current user's Id.
            string currentUserId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewData["currentUserId"] = currentUserId;

            // Get user's scales.
            PaginatedList<ZungScaleViewModel> currentUserScales = await ZungScaleForAnxietyService.GetAllAsync(currentUserId, pageIndex, pageSize, orderByDate);

            // Send pagination size and ordering criteria to the view.
            ViewData["pageSize"] = pageSize;
            ViewData["orderByDate"] = orderByDate;

            return View(currentUserScales);
        }

        [HttpGet]
        public async Task<IActionResult> EditZungScale(string id)
        {
            // Get current user's Id.
            string currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewData["currentUserId"] = currentUserId;

            // Get Zung scale.
            ZungScaleViewModel zungScaleViewModel = await ZungScaleForAnxietyService.GetByIdAsync(id, currentUserId);

            // Check if scale exists and it belongs to the current user.
            if (zungScaleViewModel == null)
            {
                return NotFound();
            }

            return View(zungScaleViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditZungScale(ZungScaleViewModel viewModel, string userId)
        {
            // Get current user's Id if ModelState is not valid and it's needed to be routed.
            // If not routed on next submission currentUserId is null and ModelState is not valid again.
            string currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewData["currentUserId"] = currentUserId;

            // Check if hacker is trying to change userId through page's HTML with other user's Id.
            // Return NotFound if sended user Id is different.
            if (userId != currentUserId)
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
            ModelState[nameof(viewModel.SeventhQuestionAnswer)]?.Errors.Clear();
            ModelState[nameof(viewModel.EighthQuestionAnswer)]?.Errors.Clear();
            ModelState[nameof(viewModel.NinthQuestionAnswer)]?.Errors.Clear();
            ModelState[nameof(viewModel.TenthQuestionAnswer)]?.Errors.Clear();
            ModelState[nameof(viewModel.EleventhQuestionAnswer)]?.Errors.Clear();
            ModelState[nameof(viewModel.TwelfthQuestionAnswer)]?.Errors.Clear();
            ModelState[nameof(viewModel.ThirteenthQuestionAnswer)]?.Errors.Clear();
            ModelState[nameof(viewModel.FourteenthQuestionAnswer)]?.Errors.Clear();
            ModelState[nameof(viewModel.FifteenthQuestionAnswer)]?.Errors.Clear();
            ModelState[nameof(viewModel.SixteenthQuestionAnswer)]?.Errors.Clear();
            ModelState[nameof(viewModel.SeventeenthQuestionAnswer)]?.Errors.Clear();
            ModelState[nameof(viewModel.EighteenthQuestionAnswer)]?.Errors.Clear();
            ModelState[nameof(viewModel.NineteenthQuestionAnswer)]?.Errors.Clear();
            ModelState[nameof(viewModel.TwentiethQuestionAnswer)]?.Errors.Clear();

            // Check if all Model properties have values.
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            // Custom model validation for changed radio button's value through page's HTML.
            if (this.ZungScaleForAnxietyService.ValidateAnswer(viewModel.FirstQuestionAnswer))
            {
                ModelState.AddModelError(nameof(viewModel.FirstQuestionAnswer), "Необходимо е да отбележите отговор.");
            }

            if (this.ZungScaleForAnxietyService.ValidateAnswer(viewModel.SecondQuestionAnswer))
            {
                ModelState.AddModelError(nameof(viewModel.SecondQuestionAnswer), "Необходимо е да отбележите отговор.");
            }

            if (this.ZungScaleForAnxietyService.ValidateAnswer(viewModel.ThirdQuestionAnswer))
            {
                ModelState.AddModelError(nameof(viewModel.ThirdQuestionAnswer), "Необходимо е да отбележите отговор.");
            }

            if (this.ZungScaleForAnxietyService.ValidateAnswer(viewModel.FourthQuestionAnswer))
            {
                ModelState.AddModelError(nameof(viewModel.FourthQuestionAnswer), "Необходимо е да отбележите отговор.");
            }

            if (this.ZungScaleForAnxietyService.ValidateAnswer(viewModel.FifthQuestionAnswer))
            {
                ModelState.AddModelError(nameof(viewModel.FifthQuestionAnswer), "Необходимо е да отбележите отговор.");
            }

            if (this.ZungScaleForAnxietyService.ValidateAnswer(viewModel.SixthQuestionAnswer))
            {
                ModelState.AddModelError(nameof(viewModel.SixthQuestionAnswer), "Необходимо е да отбележите отговор.");
            }

            if (this.ZungScaleForAnxietyService.ValidateAnswer(viewModel.SeventhQuestionAnswer))
            {
                ModelState.AddModelError(nameof(viewModel.SeventhQuestionAnswer), "Необходимо е да отбележите отговор.");
            }

            if (this.ZungScaleForAnxietyService.ValidateAnswer(viewModel.EighthQuestionAnswer))
            {
                ModelState.AddModelError(nameof(viewModel.EighthQuestionAnswer), "Необходимо е да отбележите отговор.");
            }

            if (this.ZungScaleForAnxietyService.ValidateAnswer(viewModel.NinthQuestionAnswer))
            {
                ModelState.AddModelError(nameof(viewModel.NinthQuestionAnswer), "Необходимо е да отбележите отговор.");
            }

            if (this.ZungScaleForAnxietyService.ValidateAnswer(viewModel.TenthQuestionAnswer))
            {
                ModelState.AddModelError(nameof(viewModel.TenthQuestionAnswer), "Необходимо е да отбележите отговор.");
            }

            if (this.ZungScaleForAnxietyService.ValidateAnswer(viewModel.EleventhQuestionAnswer))
            {
                ModelState.AddModelError(nameof(viewModel.EleventhQuestionAnswer), "Необходимо е да отбележите отговор.");
            }

            if (this.ZungScaleForAnxietyService.ValidateAnswer(viewModel.TwelfthQuestionAnswer))
            {
                ModelState.AddModelError(nameof(viewModel.TwelfthQuestionAnswer), "Необходимо е да отбележите отговор.");
            }

            if (this.ZungScaleForAnxietyService.ValidateAnswer(viewModel.ThirteenthQuestionAnswer))
            {
                ModelState.AddModelError(nameof(viewModel.ThirteenthQuestionAnswer), "Необходимо е да отбележите отговор.");
            }

            if (this.ZungScaleForAnxietyService.ValidateAnswer(viewModel.FourteenthQuestionAnswer))
            {
                ModelState.AddModelError(nameof(viewModel.FourteenthQuestionAnswer), "Необходимо е да отбележите отговор.");
            }

            if (this.ZungScaleForAnxietyService.ValidateAnswer(viewModel.FifteenthQuestionAnswer))
            {
                ModelState.AddModelError(nameof(viewModel.FifteenthQuestionAnswer), "Необходимо е да отбележите отговор.");
            }

            if (this.ZungScaleForAnxietyService.ValidateAnswer(viewModel.SixteenthQuestionAnswer))
            {
                ModelState.AddModelError(nameof(viewModel.SixteenthQuestionAnswer), "Необходимо е да отбележите отговор.");
            }

            if (this.ZungScaleForAnxietyService.ValidateAnswer(viewModel.SeventeenthQuestionAnswer))
            {
                ModelState.AddModelError(nameof(viewModel.SeventeenthQuestionAnswer), "Необходимо е да отбележите отговор.");
            }

            if (this.ZungScaleForAnxietyService.ValidateAnswer(viewModel.EighteenthQuestionAnswer))
            {
                ModelState.AddModelError(nameof(viewModel.EighteenthQuestionAnswer), "Необходимо е да отбележите отговор.");
            }

            if (this.ZungScaleForAnxietyService.ValidateAnswer(viewModel.NineteenthQuestionAnswer))
            {
                ModelState.AddModelError(nameof(viewModel.NineteenthQuestionAnswer), "Необходимо е да отбележите отговор.");
            }

            if (this.ZungScaleForAnxietyService.ValidateAnswer(viewModel.TwentiethQuestionAnswer))
            {
                ModelState.AddModelError(nameof(viewModel.TwentiethQuestionAnswer), "Необходимо е да отбележите отговор.");
            }

            // Check again ModelState if custom Model Errors are added.
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            // Add validated edited answers to string array (given as param to EditAsync method).
            string[] editedAnswers = new string[]
            {
                viewModel.FirstQuestionAnswer,
                viewModel.SecondQuestionAnswer,
                viewModel.ThirdQuestionAnswer,
                viewModel.FourthQuestionAnswer,
                viewModel.FifthQuestionAnswer,
                viewModel.SixthQuestionAnswer,
                viewModel.SeventhQuestionAnswer,
                viewModel.EighthQuestionAnswer,
                viewModel.NinthQuestionAnswer,
                viewModel.TenthQuestionAnswer,
                viewModel.EleventhQuestionAnswer,
                viewModel.TwelfthQuestionAnswer,
                viewModel.ThirteenthQuestionAnswer,
                viewModel.FourteenthQuestionAnswer,
                viewModel.FifteenthQuestionAnswer,
                viewModel.SixteenthQuestionAnswer,
                viewModel.SeventeenthQuestionAnswer,
                viewModel.EighteenthQuestionAnswer,
                viewModel.NineteenthQuestionAnswer,
                viewModel.TwentiethQuestionAnswer,
            };

            try
            {
                await this.ZungScaleForAnxietyService.EditAsync(viewModel.Id, currentUserId, editedAnswers);

                // Get controller's name and action's name without using magic strings.
                string actionName = nameof(ScalesController.MyZungScales);
                string controllerName = nameof(ScalesController).Substring(0, nameof(ScalesController).Length - "Controller".Length);

                return RedirectToAction(actionName, controllerName);
            }
            catch (ArgumentException ae)
            {
                return NotFound();
            }
        }

        [HttpGet]
        public async Task<IActionResult> DeleteZungScale(string zungScaleId, string currentUserId)
        {
            // Get current user's Id.
            currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewData["currentUserId"] = currentUserId;

            // Check if hacker is trying to change userId through page's HTML with other user's Id.
            // Return NotFound if sent user Id is different.
            if (currentUserId != ViewData["currentUserId"]!.ToString())
            {
                return NotFound();
            }

            try
            {
                await this.ZungScaleForAnxietyService.SoftDeleteAsync(zungScaleId, currentUserId);

                // Get controller's name and action's name without using magic strings.
                string actionName = nameof(ScalesController.MyZungScales);
                string controllerName = nameof(ScalesController).Substring(0, nameof(ScalesController).Length - "Controller".Length);

                return RedirectToAction(actionName, controllerName);
            }
            catch (ArgumentException ex)
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> ShareZungScale([FromBody] SharedZungScaleForAnxietyAddModel scale)
        {
            if (String.IsNullOrEmpty(scale.DoctorID) == false)
            {
                try
                {
                    await this.ZungScaleForAnxietyService.Share(scale.ScaleID, scale.DoctorID);

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
