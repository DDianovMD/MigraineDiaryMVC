using Microsoft.AspNetCore.Mvc;
using MigraineDiary.ViewModels;
using MigraineDiary.Services.Contracts;
using System.Diagnostics;

namespace MigraineDiary.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IArticleService articleService;

        public HomeController(ILogger<HomeController> logger,
                              IArticleService articleService)
        {
            _logger = logger;
            this.articleService = articleService;
        }

        [HttpGet]
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

            PaginatedList<ArticleViewModel> articles = await this.articleService.GetArticles(pageIndex, pageSize, orderByDate);

            ViewData["pageSize"] = pageSize;
            ViewData["orderByDate"] = orderByDate;

            return View(articles);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}