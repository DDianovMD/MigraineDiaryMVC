using Microsoft.AspNetCore.Mvc.Rendering;

namespace MigraineDiary.Web.Models
{
    public class SetRoleViewModel
    {
        public List<SelectListItem> UsersDropdown { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> RolesDropdown { get; set; } = new List<SelectListItem>();
    }
}
