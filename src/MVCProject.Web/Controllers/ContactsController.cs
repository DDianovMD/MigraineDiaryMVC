using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MigraineDiary.Web.Models;
using MigraineDiary.Web.Services.Contracts;

namespace MigraineDiary.Web.Controllers
{
    [AllowAnonymous]
    public class ContactsController : Controller
    {
        private readonly IMessageService messageService;

        public ContactsController(IMessageService messageService)
        {
            this.messageService = messageService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(MessageAddModel addModel)
        {
            if (!ModelState.IsValid)
            {
                return View(addModel);
            }

            await this.messageService.AddAsync(addModel);

            // Get controller's name and action's name without using magic strings.
            string actionName = nameof(ContactsController.Info);
            string controllerName = nameof(ContactsController).Substring(0, nameof(ContactsController).Length - "Controller".Length);

            return RedirectToAction(actionName, controllerName);
        }

        [HttpGet]
        public IActionResult Info() 
        {
            return View();
        }
    }
}
