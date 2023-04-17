using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MigraineDiary.Web.Data;
using MigraineDiary.Web.Data.DbModels;
using MigraineDiary.Web.Models;
using MigraineDiary.Web.Services.Contracts;

namespace MigraineDiary.Web.Services
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
            IdentityRole role = await this.roleManager.FindByIdAsync(roleId);

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

        public SetRoleViewModel PopulateUsersAndRoles(SetRoleViewModel model)
        {
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
    }
}
