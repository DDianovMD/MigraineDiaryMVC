using Microsoft.AspNetCore.Mvc.Rendering;
using MigraineDiary.Web.Models;

namespace MigraineDiary.Web.Services.Contracts
{
    public interface IAdminService
    {
        public Task CreateRoleAsync(string roleName);

        public Task AssignRoleAsync(string userId, string roleId);

        public SetRoleViewModel PopulateUsersAndRoles(SetRoleViewModel model);

        public Task<Dictionary<string, List<string>>> GetUsersAndRolesAsync();
    }
}
