using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MigraineDiary.Data;
using MigraineDiary.Data.DbModels;
using MigraineDiary.Services;
using MigraineDiary.Services.Contracts;
using MigraineDiary.Tests.Mocks.Database;
using MigraineDiary.Tests.Mocks.Services;
using MigraineDiary.ViewModels;
using NUnit.Framework;

namespace MigraineDiary.Tests.Services
{
    [TestFixture]
    public class AdminServiceTests
    {
        private ApplicationDbContext dbContext;
        private UserManager<ApplicationUser> userManager;
        private RoleManager<IdentityRole> roleManager;

        private IAdminService adminService;

        private string doctorUserId;
        private string patientUserId;
        private string roleId;

        private List<int> rolesCount;
        private List<string> assignedRoles;

        [SetUp]
        public async Task Setup()
        {
            this.rolesCount = new List<int>() { 1 };
            this.assignedRoles = new List<string>();

            this.dbContext = InMemoryDatabase.Instance();
            this.userManager = UserManagerMock.Instance<ApplicationUser>(this.assignedRoles);
            this.roleManager = RoleManagerMock.Instance<IdentityRole>(this.rolesCount);
            this.adminService = new AdminService(this.dbContext, userManager, roleManager);

            this.doctorUserId = Guid.NewGuid().ToString();
            this.patientUserId = Guid.NewGuid().ToString();
            this.roleId = Guid.NewGuid().ToString();

            await this.SeedTestData();
        }

        [Test]
        public async Task AssignRoleAsync_AssignsUserToChosenRole()
        {
            // Arrange - in Setup method.
            ApplicationUser? patient = await this.dbContext.Users.FirstOrDefaultAsync(x => x.Id == patientUserId);
           
            // Act
            Task result = this.adminService.AssignRoleAsync(this.patientUserId, this.roleId);
            await result;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(this.assignedRoles.Any(r => r == "TestPassed"), Is.True);
        }

        [Test]
        public async Task CreateRoleAsync_CreatingRoleWithGivenName()
        {
            // Arrange
            string testName = "TestRole";
            int count = this.rolesCount.Count;

            // Act
            Task result = this.adminService.CreateRoleAsync(testName);
            await result;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(this.rolesCount.Count, Is.EqualTo(++count));
        }

        [Test]
        public async Task DeleteRoleAsync_IsDeletingRole_WhenRoleExists()
        {
            // Arrange
            string? roleId = this.dbContext.Roles.Where(r => r.Name == "Doctor")
                                                 .Select(x => x.Id)
                                                 .FirstOrDefault();

            int count = this.rolesCount.Count;

            // Act
            Task result = this.adminService.DeleteRoleAsync(roleId!);
            await result;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(roleId, Is.Not.Null);
            Assert.That(this.rolesCount.Count, Is.EqualTo(--count));
        }

        [Test]
        public void DeleteRoleAsync_ThrowsArgumentException_WhenRoleDoesNotExist()
        {
            // Arrange
            string roleId = Guid.NewGuid().ToString();

            int count = this.rolesCount.Count;

            // Act and Assert
            Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                Task result = this.adminService.DeleteRoleAsync(roleId!);
                await result;
            }, "Role doesn't exist.");
        }

        [Test]
        public void PopulateUsersAndRoles_ReturnsAllUsersAndRolesInDatabase()
        {
            // Arrange - in Setup method.

            // Act
            SetRoleViewModel result = this.adminService.PopulateUsersAndRoles();

            // Assert
            Assert.That(result.UsersDropdown.Count(), Is.EqualTo(2));
            Assert.That(result.RolesDropdown.Count(), Is.EqualTo(1));
            Assert.That(result.UsersDropdown.All(x => x.Value == this.doctorUserId ||
                                                      x.Value == this.patientUserId), Is.True);

            Assert.That(result.UsersDropdown.All(x => x.Text == "TestDoctor" ||
                                                      x.Text == "TestPatient"), Is.True);
            Assert.That(result.RolesDropdown.All(x => x.Value == this.roleId), Is.True);

            Assert.That(result.RolesDropdown.All(x => x.Text == "Doctor"), Is.True);
        }

        [Test]
        public void GetRoles_ReturnsRegisteredRoles()
        {
            // Arrange - in Setup method.

            // Act
            DeleteRoleViewModel result = this.adminService.GetRoles();

            // Assert
            Assert.That(result.RolesDropdown.All(r => r.Value == this.roleId), Is.True);
            Assert.That(result.RolesDropdown.All(r => r.Text == "Doctor"), Is.True);
        }

        [Test]
        public async Task GetUsersAndRolesAsync_ReturnsAllRegisteredUsersAndTheirRoles()
        {
            // Arrange - in Setup method.

            // Act
            Dictionary<string, List<string>> result = await this.adminService.GetUsersAndRolesAsync();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result["TestDoctor"].All(x => x == "Doctor"), Is.True);
            Assert.That(result["TestPatient"].Count, Is.EqualTo(0));
        }

        [Test]
        public async Task GetUserFullName_ReturnsCorrectName()
        {
            // Arrange
            ApplicationUser? doctor = this.dbContext.Users
                                                    .Where(u => u.Id == this.doctorUserId)
                                                    .FirstOrDefault();
            // Act
            string result = await this.adminService.GetUserFullName(this.doctorUserId);

            // Assert
            Assert.That(doctor, Is.Not.Null);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo($"{doctor!.FirstName} {doctor.LastName}"));
        }

        [TearDown]
        public async Task Teardown()
        {
            await this.dbContext.DisposeAsync();
        }

        private async Task SeedTestData()
        {
            // Create test users.
            ApplicationUser doctorUser = new ApplicationUser
            {
                Id = this.doctorUserId,
                FirstName = "TestDoctor",
                LastName = "TestDoctor",
                UserName = "TestDoctor",
            };

            ApplicationUser patientUser = new ApplicationUser
            {
                Id = this.patientUserId,
                FirstName = "TestPatient",
                LastName = "TestPatient",
                UserName = "TestPatient",
            };

            // Create role.
            IdentityRole role = new IdentityRole
            {
                Id = this.roleId,
                Name = "Doctor",
            };

            IdentityUserRole<string> userRole = new IdentityUserRole<string>
            {
                RoleId = roleId,
                UserId = doctorUserId,
            };

            // Add role and assign it to doctor user.
            await this.dbContext.Roles.AddAsync(role);
            await this.dbContext.UserRoles.AddAsync(userRole);

            // Add users to database.
            await this.dbContext.Users.AddAsync(doctorUser);
            await this.dbContext.Users.AddAsync(patientUser);

            // Save changes in database.
            await this.dbContext.SaveChangesAsync();
        }
    }
}
