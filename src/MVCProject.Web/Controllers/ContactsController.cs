using Microsoft.AspNetCore.Mvc;

namespace MigraineDiary.Web.Controllers
{
    public class ContactsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
