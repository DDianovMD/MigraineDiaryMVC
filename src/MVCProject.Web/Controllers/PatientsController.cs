using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MigraineDiary.Services.Contracts;
using MigraineDiary.ViewModels;
using System.Security.Claims;

namespace MigraineDiary.Web.Controllers
{
    [Authorize(Roles = "Doctor")]
    public class PatientsController : Controller
    {
        private readonly IPatientService patientService;
        private readonly IHeadacheService headacheService;
        private readonly IHIT6ScaleService hit6ScaleService;
        private readonly IZungScaleForAnxietyService zungScaleForAnxietyService;

        public PatientsController(IPatientService patientService, 
                                 IHeadacheService headacheService, 
                                IHIT6ScaleService hit6ScaleService,
                      IZungScaleForAnxietyService zungScaleForAnxietyService)
        {
            this.patientService = patientService;
            this.headacheService = headacheService;
            this.hit6ScaleService = hit6ScaleService;
            this.zungScaleForAnxietyService = zungScaleForAnxietyService;

        }

        [HttpGet]
        public async Task<IActionResult> MyPatients()
        {
            // Get logged user's ID
            string currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Get all users who shared headache or scale with logged user in role "Doctor".
            PatientViewModel[] patients = await this.patientService.GetAllPatientsAsync(currentUserId);

            return View(patients);
        }

        [HttpGet]
        public async Task<IActionResult> Headaches(string patientId, int pageIndex = 1, int pageSize = 1, string orderByDate = "NewestFirst")
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

            // Get logged user's ID.
            string currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Get shared headaches with logged user in role "Doctor".
            PaginatedList<SharedHeadacheViewModel> sharedHeadachesViewModel = await this.headacheService.GetSharedHeadachesAsync(currentUserId, patientId, pageIndex, pageSize, orderByDate);

            // Send pagination size and ordering criteria to the view.
            ViewData["pageSize"] = pageSize;
            ViewData["orderByDate"] = orderByDate;

            return View(sharedHeadachesViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> HIT6Scales(string patientId, int pageIndex = 1, int pageSize = 1, string orderByDate = "NewestFirst")
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

            // Get logged user's ID.
            string currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Get shared headaches with logged user in role "Doctor".
            PaginatedList<SharedHIT6ScaleViewModel> sharedHIT6ScalesViewModel = await this.hit6ScaleService.GetSharedScalesAsync(currentUserId, patientId, pageIndex, pageSize, orderByDate);

            // Send pagination size and ordering criteria to the view.
            ViewData["pageSize"] = pageSize;
            ViewData["orderByDate"] = orderByDate;

            return View(sharedHIT6ScalesViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> ZungScales(string patientId, int pageIndex = 1, int pageSize = 1, string orderByDate = "NewestFirst")
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

            // Get logged user's ID.
            string currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Get shared headaches with logged user in role "Doctor".
            PaginatedList<SharedZungScaleForAnxietyViewModel> sharedZungScalesViewModel = await this.zungScaleForAnxietyService.GetSharedScalesAsync(currentUserId, patientId, pageIndex, pageSize, orderByDate);

            // Send pagination size and ordering criteria to the view.
            ViewData["pageSize"] = pageSize;
            ViewData["orderByDate"] = orderByDate;

            return View(sharedZungScalesViewModel);
        }
    }
}
