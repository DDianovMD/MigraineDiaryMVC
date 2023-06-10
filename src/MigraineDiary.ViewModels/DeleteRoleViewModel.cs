using Microsoft.AspNetCore.Mvc.Rendering;

namespace MigraineDiary.ViewModels
{
    public class DeleteRoleViewModel
    {
        public DeleteRoleViewModel()
        {
            this.RolesDropdown = new List<SelectListItem>();
        }

        public List<SelectListItem> RolesDropdown { get; set; }
    }
}
