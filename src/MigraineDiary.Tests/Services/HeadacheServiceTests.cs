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
    public class HeadacheServiceTests
    {
        // Constants
        private const string patientId = "test-patient";
        private const string doctorId = "test-doctor";

        private ApplicationDbContext dbContext;
        private IHeadacheService headacheService;

        [SetUp]
        public async Task SetUp()
        {
            this.dbContext = InMemoryDatabase.Instance();
            this.headacheService = new HeadacheService(this.dbContext, MedicationServiceMock.Instance());
            await SeedTestData(patientId, doctorId);
        }

        [Test]
        public void CalculateDuration_ReturnsExpectedResult()
        {
            // Arrange
            const int days = 1;
            const int minutes = 7;
            const int hours = 3;

            DateTime onset = DateTime.UtcNow;
            DateTime end = DateTime.UtcNow.AddDays(days).AddMinutes(minutes).AddHours(hours);

            // Act
            Dictionary<string, int> result = this.headacheService.CalculateDuration(onset, end);

            // Assert
            Assert.That(result.ContainsKey("Days"), Is.True);
            Assert.That(result.ContainsKey("Hours"), Is.True);
            Assert.That(result.ContainsKey("Minutes"), Is.True);
            Assert.That(result["Days"], Is.EqualTo(days));
            Assert.That(result["Hours"], Is.EqualTo(hours));
            Assert.That(result["Minutes"], Is.EqualTo(minutes));
        }

        [TestCase(patientId, 1, 2, "NewestFirst")]
        [TestCase(patientId, 1, 5, "NewestFirst")]
        [TestCase(patientId, 1, 2, "OldestFirst")]
        [TestCase(patientId, 1, 5, "OldestFirst")]
        public async Task GetRegisteredHeadachesAsync_ReturnsOrderedByDateResult(string patientId, int pageIndex, int pageSize, string orderByDate)
        {
            // Arrange - in Setup method.

            // Act
            var result = await this.headacheService.GetRegisteredHeadachesAsync(patientId, pageIndex, pageSize, orderByDate);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<PaginatedList<RegisteredHeadacheViewModel>>());
            
            if (orderByDate == "NewestFirst")
            {
                Assert.That(result.ElementAt(0).Onset, Is.GreaterThan(result.ElementAt(1).Onset));
            }
            else
            {
                Assert.That(result.ElementAt(0).Onset, Is.LessThan(result.ElementAt(1).Onset));
            }
        }

        [TestCase(doctorId, patientId, 1, 2, "NewestFirst")]
        [TestCase(doctorId, patientId, 1, 2, "OldestFirst")]
        public async Task GetSharedHeadachesAsync_ReturnsSharedHeadaches(string doctorId, string patientId, int pageIndex, int pageSize, string orderByDate)
        {
            // Arrange
            
            // Get registered headache.
            Headache? firstHeadache = await this.dbContext.Headaches.FindAsync("test-headache-1");
            Headache? secondHeadache = await this.dbContext.Headaches.FindAsync("test-headache-2");
            ApplicationUser? doctor = await this.dbContext.Users.FindAsync(doctorId);
           
            // Share headaches with Doctor user.
            doctor.SharedWithMe.Add(firstHeadache!);
            doctor.SharedWithMe.Add(secondHeadache!);

            // Save data.
            await this.dbContext.SaveChangesAsync();

            // Act
            var result = await this.headacheService.GetSharedHeadachesAsync(doctorId, patientId, pageIndex, pageSize, orderByDate);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(1));

            if (orderByDate == "NewestFirst")
            {
                Assert.That(result.ElementAt(0).Onset, Is.GreaterThan(result.ElementAt(1).Onset));
            }
            else
            {
                Assert.That(result.ElementAt(0).Onset, Is.LessThan(result.ElementAt(1).Onset));
            }
        }

        [Test]
        public async Task GetDoctorUsersByNameAsync_ReturnsNotNull()
        {
            // Arrange - in Setup method.

            // Act
            var result = await this.headacheService.GetDoctorUsersByNameAsync("TestDoctor");

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.All(n => n.FirstName == "TestDoctor"), Is.True);
        }

        [Test]
        public async Task Share_IsAddingHeadacheToDoctorsCollectionOfSharedHeadaches_WhenHeadacheAndDoctorAreExisting()
        {
            // Arrange - in Setup method.
            ApplicationUser doctor = await this.dbContext.Users.FirstAsync(d => d.Id == doctorId);

            // Generate random guid string.
            string roleId = Guid.NewGuid().ToString();

            // Create role.
            IdentityRole role = new IdentityRole
            {
                Id = roleId,
                Name = "Doctor",
            };

            IdentityUserRole<string> userRole = new IdentityUserRole<string>
            {
                RoleId = roleId,
                UserId = doctorId,
            };

            // Add role and assign it to doctor user.
            await this.dbContext.Roles.AddAsync(role);
            await this.dbContext.UserRoles.AddAsync(userRole);

            // Save changes in database.
            await this.dbContext.SaveChangesAsync();

            // Act
            Task result = this.headacheService.Share("test-headache-1", doctorId);
            await result;

            // Assert
            Assert.That(result.IsCompleted, Is.True);
            Assert.That(doctor.SharedWithMe.All(x => x.Id == "test-headache-1"), Is.True);
        }

        [Test]
        public void Share_ThrowsArgumentException_WhenDoctorUserDoesNotExist()
        {
            // Arrange - in Setup method.

            // Act and Assert
            Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                Task result = this.headacheService.Share("test-headache-1", "");
                await result;
            }, "User doesn't exists or isn't in role \"Doctor\"");
        }

        [Test]
        public async Task Share_ThrowsArgumentException_WhenHeadacheDoesNotExist()
        {
            // Arrange
            // Generate random guid string.
            string roleId = Guid.NewGuid().ToString();

            // Create role.
            IdentityRole role = new IdentityRole
            {
                Id = roleId,
                Name = "Doctor",
            };

            IdentityUserRole<string> userRole = new IdentityUserRole<string>
            {
                RoleId = roleId,
                UserId = doctorId,
            };

            // Add role and assign it to doctor user.
            await this.dbContext.Roles.AddAsync(role);
            await this.dbContext.UserRoles.AddAsync(userRole);

            // Save changes in database.
            await this.dbContext.SaveChangesAsync();

            // Act and Assert
            Assert.ThrowsAsync<ArgumentException>(async () => 
            {
                var result = this.headacheService.Share(Guid.NewGuid().ToString(), doctorId);
                await result;
            }, "Headache doesn't exist.");
        }

        [Test]
        public async Task Share_ThrowsArgumentException_WhenHeadacheIsAlreadyShared()
        {
            // Arrange
            // Generate random guid string.
            string roleId = Guid.NewGuid().ToString();

            // Get headache and doctor entities.
            Headache headache = await this.dbContext.Headaches.FirstAsync(x => x.Id == "test-headache-1");
            ApplicationUser doctor = await this.dbContext.Users.FirstAsync(x => x.Id == doctorId);
            
            // Create role.
            IdentityRole role = new IdentityRole
            {
                Id = roleId,
                Name = "Doctor",
            };

            // Assign role to doctor user.
            IdentityUserRole<string> userRole = new IdentityUserRole<string>
            {
                RoleId = roleId,
                UserId = doctorId,
            };

            // Share headache entity with doctor user.
            doctor.SharedWithMe.Add(headache);

            // Add role and assign it to doctor user.
            await this.dbContext.Roles.AddAsync(role);
            await this.dbContext.UserRoles.AddAsync(userRole);

            // Save changes in database.
            await this.dbContext.SaveChangesAsync();

            // Act and Assert
            Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                var result = this.headacheService.Share(headache.Id, doctorId);
                await result;
            }, "Headache is already shared.");
        }

        [TearDown]
        public void TearDown()
        {
            this.dbContext.Dispose();
        }

        private async Task SeedTestData(string patientId, string doctorId)
        {
            // Create headaches.
            List<Headache> headaches = new List<Headache>
            {
                new Headache
                {
                    Id = "test-headache-1",
                    PatientId = patientId,
                    Onset = DateTime.UtcNow,
                    LocalizationSide = "test1",
                    PainCharacteristics = "test1",
                },

                new Headache
                {
                    Id = "test-headache-2",
                    PatientId = patientId,
                    Onset = DateTime.UtcNow,
                    LocalizationSide = "test2",
                    PainCharacteristics = "test2",
                }
            };

            // Create Doctor user.
            ApplicationUser doctor = new ApplicationUser
            {
                Id = doctorId,
                FirstName = "TestDoctor",
            };

            // Save data
            await this.dbContext.Headaches.AddRangeAsync(headaches);
            await this.dbContext.Users.AddAsync(doctor);
            await this.dbContext.SaveChangesAsync();
        }
    }
}
