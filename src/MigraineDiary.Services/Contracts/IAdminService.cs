using MigraineDiary.ViewModels;

namespace MigraineDiary.Services.Contracts
{
    public interface IAdminService
    {
        public Task CreateRoleAsync(string roleName);

        public Task AssignRoleAsync(string userId, string roleId);

        public Task DeleteRoleAsync(string roleId);

        public DeleteRoleViewModel GetRoles();

        public SetRoleViewModel PopulateUsersAndRoles();

        public Task<Dictionary<string, List<string>>> GetUsersAndRolesAsync();

        public Task<string> GetUserFullName(string id);
    }
}
