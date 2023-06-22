using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MigraineDiary.Data;
using MigraineDiary.Data.DbModels;
using MigraineDiary.ViewModels;
using MigraineDiary.Services.Contracts;

namespace MigraineDiary.Services
{
    public class AdminService : IAdminService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public AdminService(ApplicationDbContext dbContext,
                            UserManager<ApplicationUser> userManager,
                            RoleManager<IdentityRole> roleManager)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public async Task AssignRoleAsync(string userId, string roleId)
        {
            ApplicationUser? user = await this.dbContext.Users.FirstOrDefaultAsync(x => x.Id == userId);
            IdentityRole? role = await this.dbContext.Roles.FirstOrDefaultAsync(r => r.Id == roleId);

            if (user != null && role != null)
            {
                await this.userManager.AddToRoleAsync(user, role.Name);
            }

            await this.dbContext.SaveChangesAsync();
        }

        public async Task CreateRoleAsync(string roleName)
        {
            await this.roleManager.CreateAsync(new IdentityRole(roleName));
            await this.dbContext.SaveChangesAsync();
        }

        public async Task DeleteRoleAsync(string roleId)
        {
            IdentityRole? role = await this.dbContext.Roles.FirstOrDefaultAsync(role => role.Id == roleId);

            if (role != null)
            {
                await this.roleManager.DeleteAsync(role);
                await this.dbContext.SaveChangesAsync();
            }
            else
            {
                throw new ArgumentException($"Role doesn't exist.");
            }
        }

        public SetRoleViewModel PopulateUsersAndRoles()
        {
            SetRoleViewModel model = new SetRoleViewModel();

            var users = this.dbContext.Users
                                      .Select(x => new
                                      {
                                          x.Id,
                                          x.UserName
                                      });

            var roles = this.dbContext.Roles
                                      .Select(x => new
                                      {
                                          x.Id,
                                          x.Name
                                      });

            foreach (var user in users)
            {
                SelectListItem item = new SelectListItem();
                item.Text = user.UserName;
                item.Value = user.Id;

                model.UsersDropdown.Add(item);
            }

            foreach (var role in roles)
            {
                SelectListItem item = new SelectListItem();
                item.Text = role.Name;
                item.Value = role.Id;

                model.RolesDropdown.Add(item);
            }

            return model;
        }

        public DeleteRoleViewModel GetRoles()
        {
            var deleteModel = new DeleteRoleViewModel();

            var roles = this.dbContext.Roles
                                      .Select(x => new
                                      {
                                          x.Id,
                                          x.Name
                                      });

            foreach (var role in roles)
            {
                SelectListItem item = new SelectListItem();
                item.Text = role.Name;
                item.Value = role.Id;

                deleteModel.RolesDropdown.Add(item);
            }

            return deleteModel;
        }

        public async Task<Dictionary<string, List<string>>> GetUsersAndRolesAsync()
        {
            var users = await this.dbContext.Users
                                            .OrderBy(x => x.UserName)
                                            .Select(x => new
                                            {
                                                x.Id,
                                                x.UserName
                                            })
                                            .ToArrayAsync();

            var roles = await this.dbContext.Roles
                                            .OrderBy(x => x.Name)
                                            .Select(x => new
                                            {
                                                x.Id,
                                                x.Name
                                            })
                                            .ToArrayAsync();

            var userRoles = await this.dbContext.UserRoles
                                                .Select(x => new
                                                {
                                                    x.RoleId,
                                                    x.UserId
                                                })
                                                .ToArrayAsync();

            Dictionary<string, List<string>> usersRolesNames = new Dictionary<string, List<string>>();

            foreach (var user in users)
            {
                usersRolesNames.Add(user.UserName, new List<string>());

                foreach (var userRole in userRoles.Where(x => x.UserId == user.Id))
                {
                    usersRolesNames[user.UserName].Add(roles.FirstOrDefault(x => x.Id == userRole.RoleId)!.Name);
                }
            }

            return usersRolesNames;
        }

        public async Task<string> GetUserFullName(string id)
        {
            var user = await this.dbContext.Users
                                           .Where(u => u.Id == id)
                                           .Select(u => new
                                           {
                                               FullName = $"{u.FirstName} {u.LastName}"
                                           })
                                           .FirstAsync();

            return user.FullName;
        }
    }
}
