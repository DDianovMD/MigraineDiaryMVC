using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MigraineDiary.Data;
using MigraineDiary.Data.DbModels;
using MigraineDiary.Services;
using MigraineDiary.Services.Contracts;
using MigraineDiary.Tests.Mocks.Database;
using MigraineDiary.ViewModels;
using NUnit.Framework;

namespace MigraineDiary.Tests.Services
{
    [TestFixture]
    public class ClinicalTrialServiceTests
    {
        private string doctorUserId;
        private string patientUserId;
        private ApplicationDbContext dbContext;
        private IClinicalTrialService clinicalTrialService;

        [SetUp]
        public async Task SetUp()
        {
            this.dbContext = InMemoryDatabase.Instance();
            this.clinicalTrialService = new ClinicalTrialService(this.dbContext);

            this.doctorUserId = Guid.NewGuid().ToString();
            this.patientUserId = Guid.NewGuid().ToString();

            await this.SeedTestData();
        }

        [Test]
        public async Task AddAsync_IsAddingClinicalTrialAndPracticionersInDatabase()
        {
            // Arrange
            var trial = new ClinicalTrialAddModel
            {
                City = "Test",
                Heading = "Test",
                Hospital = "Test",
                Practicioners = new List<PracticionerAddModel>
                {
                    new PracticionerAddModel
                    {
                        FirstName = "Test",
                        Lastname = "Test",
                        Rank = "Acad.",
                        ScienceDegree = "PhD",
                    },

                    new PracticionerAddModel
                    {
                        FirstName = "Test2",
                        Lastname = "Test2",
                        Rank = "Doc.",
                        ScienceDegree = "NoDegree",
                    },
                }
            };

            // Act
            Task result = this.clinicalTrialService.AddAsync(trial, "testName", doctorUserId);
            await result;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(this.dbContext.ClinicalTrials.Count(), Is.EqualTo(1));
            Assert.That(this.dbContext.Practicioners.Count(), Is.EqualTo(2));
        }

        [TestCase("txt")]
        [TestCase("pdf")]
        [TestCase("doc")]
        [TestCase("docx")]
        public void GenerateUniqueFileName_GeneratesFileNameWithExpectedExtension(string fileExtension)
        {
            // Act
            string resultName = this.clinicalTrialService.GenerateUniqueFileName(fileExtension);

            // Assert
            Assert.That(resultName.Split('.').ToList().Last(), Is.EqualTo(fileExtension));
        }

        [TestCase(1, 1, "NewestFirst")]
        [TestCase(1, 5, "NewestFirst")]
        [TestCase(1, int.MaxValue, "NewestFirst")]
        [TestCase(1, 1, "OldestFirst")]
        [TestCase(1, 5, "OldestFirst")]
        [TestCase(1, int.MaxValue, "OldestFirst")]
        public async Task GetAllTrialsAsync_ReturnsRegisteredTrials(int pageIndex, int pageSize, string orderByDate)
        {
            // Arrange
            var trial = new ClinicalTrial
            {
                CreatedOn = DateTime.UtcNow,
                IsDeleted = false,
                DeletedOn = null,
                AgreementDocumentName = "Test",
                City = "Test",
                Hospital = "Test",
                Heading = "Test",
                CreatorId = doctorUserId,
                Practicioners = new List<Practicioner>
                {
                    new Practicioner
                    {
                        CreatedOn = DateTime.UtcNow,
                        IsDeleted = false,
                        DeletedOn = null,
                        FirstName = "Test",
                        Lastname = "Test",
                        Rank = "Acad.",
                        ScienceDegree = "PhD",
                    },

                    new Practicioner
                    {
                        CreatedOn = DateTime.UtcNow,
                        IsDeleted = false,
                        DeletedOn = null,
                        FirstName = "Test2",
                        Lastname = "Test2",
                        Rank = "Doc.",
                        ScienceDegree = "NoDegree",
                    },
                }
            };

            this.dbContext.ClinicalTrials.Add(trial);
            await this.dbContext.SaveChangesAsync();

            // Act
            PaginatedList<ClinicalTrialViewModel> result = await this.clinicalTrialService.GetAllTrialsAsync(pageIndex, pageSize, orderByDate);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<PaginatedList<ClinicalTrialViewModel>>());
            if (result.HasNextPage == true)
            {
                Assert.That(result.Count(), Is.EqualTo(pageSize));
            }
            else
            {
                Assert.That(result.Count(), Is.LessThanOrEqualTo(pageSize));
            }
        }

        [TestCase(1, 1, "NewestFirst")]
        [TestCase(1, 1, "OldestFirst")]
        public async Task GetAllTrialsAsync_ReturnsAllRegisteredTrialsByUser(int pageIndex, int pageSize, string orderByDate)
        {
            // Arrange
            ClinicalTrial trial = new ClinicalTrial
            {
                CreatedOn = DateTime.UtcNow,
                IsDeleted = false,
                DeletedOn = null,
                AgreementDocumentName = "Test",
                City = "Test",
                Hospital = "Test",
                Heading = "Test",
                CreatorId = doctorUserId,
                Practicioners = new List<Practicioner>
                {
                    new Practicioner
                    {
                        CreatedOn = DateTime.UtcNow,
                        IsDeleted = false,
                        DeletedOn = null,
                        FirstName = "Test",
                        Lastname = "Test",
                        Rank = "Acad.",
                        ScienceDegree = "PhD",
                    },

                    new Practicioner
                    {
                        CreatedOn = DateTime.UtcNow,
                        IsDeleted = false,
                        DeletedOn = null,
                        FirstName = "Test2",
                        Lastname = "Test2",
                        Rank = "Doc.",
                        ScienceDegree = "NoDegree",
                    },
                }
            };

            ClinicalTrial secondTrial = new ClinicalTrial
            {
                CreatedOn = DateTime.UtcNow,
                IsDeleted = false,
                DeletedOn = null,
                AgreementDocumentName = "Test",
                City = "Test",
                Hospital = "Test",
                Heading = "Test",
                CreatorId = Guid.NewGuid().ToString(),
                Practicioners = new List<Practicioner>
                {
                    new Practicioner
                    {
                        CreatedOn = DateTime.UtcNow,
                        IsDeleted = false,
                        DeletedOn = null,
                        FirstName = "Test",
                        Lastname = "Test",
                        Rank = "Acad.",
                        ScienceDegree = "PhD",
                    },

                    new Practicioner
                    {
                        CreatedOn = DateTime.UtcNow,
                        IsDeleted = false,
                        DeletedOn = null,
                        FirstName = "Test2",
                        Lastname = "Test2",
                        Rank = "Doc.",
                        ScienceDegree = "NoDegree",
                    },
                }
            };

            // Add trials to database.
            this.dbContext.ClinicalTrials.Add(trial);
            this.dbContext.ClinicalTrials.Add(secondTrial);

            // Save changes in database.
            await this.dbContext.SaveChangesAsync();

            // Act
            PaginatedList<ClinicalTrialViewModel> result = await this.clinicalTrialService.GetAllTrialsAsync(pageIndex, pageSize, orderByDate, doctorUserId);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<PaginatedList<ClinicalTrialViewModel>>());
            Assert.That(result.Count(), Is.EqualTo(1));
        }

        [Test]
        public async Task GetByIdAsync_ReturnsClinicalTrial_WhenTrialExists()
        {
            // Arrange

            // Generate random guid Id.
            string trialId = Guid.NewGuid().ToString();

            // Create trial
            ClinicalTrial trial = new ClinicalTrial
            {
                Id = trialId,
                CreatedOn = DateTime.UtcNow,
                IsDeleted = false,
                DeletedOn = null,
                AgreementDocumentName = "Test",
                City = "Test",
                Hospital = "Test",
                Heading = "Test",
                CreatorId = doctorUserId,
                Practicioners = new List<Practicioner>
                {
                    new Practicioner
                    {
                        CreatedOn = DateTime.UtcNow,
                        IsDeleted = false,
                        DeletedOn = null,
                        FirstName = "Test",
                        Lastname = "Test",
                        Rank = "Acad.",
                        ScienceDegree = "PhD",
                    },

                    new Practicioner
                    {
                        CreatedOn = DateTime.UtcNow,
                        IsDeleted = false,
                        DeletedOn = null,
                        FirstName = "Test2",
                        Lastname = "Test2",
                        Rank = "Doc.",
                        ScienceDegree = "NoDegree",
                    },
                }
            };

            // Add trial to dabatase.
            this.dbContext.ClinicalTrials.Add(trial);

            // Save changes in database.
            await this.dbContext.SaveChangesAsync();

            // Act
            ClinicalTrialEditModel result = await this.clinicalTrialService.GetByIdAsync(trialId, doctorUserId);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<ClinicalTrialEditModel>());
            Assert.That(result.Id, Is.EqualTo(trialId));
            Assert.That(result.Practicioners.Count, Is.EqualTo(2));
        }

        [Test]
        public void GetByIdAsync_ThrowsArgumentException_WhenTrialDoesNotExist()
        {
            // Act and Assert
            Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                var result = this.clinicalTrialService.GetByIdAsync(Guid.NewGuid().ToString(), doctorUserId);
                await result;
            }, "Невалидно Id на клинично проучване");
        }

        [Test]
        public async Task EditTrialAsync_SetsNewValuesToTrialProperties_WhenTrialExist()
        {
            // Arrange

            // Generate random guid Id.
            string trialId = Guid.NewGuid().ToString();

            // Create trial
            ClinicalTrial trial = new ClinicalTrial
            {
                Id = trialId,
                CreatedOn = DateTime.UtcNow,
                IsDeleted = false,
                DeletedOn = null,
                AgreementDocumentName = "Test",
                City = "Test",
                Hospital = "Test",
                Heading = "Test",
                CreatorId = doctorUserId,
                Practicioners = new List<Practicioner>
                {
                    new Practicioner
                    {
                        Id = "test-practicioner-1",
                        CreatedOn = DateTime.UtcNow,
                        IsDeleted = false,
                        DeletedOn = null,
                        FirstName = "Test",
                        Lastname = "Test",
                        Rank = "Acad.",
                        ScienceDegree = "PhD",
                    },

                    new Practicioner
                    {
                        Id = "test-practicioner-2",
                        CreatedOn = DateTime.UtcNow,
                        IsDeleted = false,
                        DeletedOn = null,
                        FirstName = "Test2",
                        Lastname = "Test2",
                        Rank = "Doc.",
                        ScienceDegree = "NoDegree",
                    },
                }
            };

            ClinicalTrialEditModel editedTrial = new ClinicalTrialEditModel
            {
                Id = trialId,
                City = "Edited",
                Hospital = "Edited",
                Heading = "Edited",
                AgreementDocumentName = "Edited",
                Practicioners = new List<PracticionerEditModel>
                {
                     new PracticionerEditModel
                    {
                        Id = "test-practicioner-1",
                        IsDeleted = true,
                        FirstName = "Test",
                        Lastname = "Test",
                        Rank = "Acad.",
                        ScienceDegree = "PhD",
                    },

                    new PracticionerEditModel
                    {
                        Id = "test-practicioner-2",
                        IsDeleted = false,
                        FirstName = "Edited",
                        Lastname = "Edited",
                        Rank = "Edited",
                        ScienceDegree = "Edited",
                    },

                    new PracticionerEditModel
                    {
                        Id = Guid.NewGuid().ToString(),
                        IsDeleted = false,
                        FirstName = "NewlyAdded",
                        Lastname = "NewlyAdded",
                        Rank = "NewlyAdded",
                        ScienceDegree = "NewlyAdded",
                    },
                }
            };

            // Add trial in database.
            await this.dbContext.ClinicalTrials.AddAsync(trial);

            // Save changes in database.
            await this.dbContext.SaveChangesAsync();

            // Act
            Task result = this.clinicalTrialService.EditTrialAsync(editedTrial);
            await result;

            // Get trial
            ClinicalTrial? newValuesTrial = await this.dbContext.ClinicalTrials.FirstOrDefaultAsync(t => t.Id == trialId);

            // Get soft deleted practicioner.
            Practicioner? deletedPracticioner = await this.dbContext.Practicioners.FirstOrDefaultAsync(p => p.Id == "test-practicioner-1");
            
            // Assert
            Assert.That(newValuesTrial, Is.Not.Null);
            Assert.That(newValuesTrial.Heading, Is.EqualTo("Edited"));
            Assert.That(newValuesTrial.City, Is.EqualTo("Edited"));
            Assert.That(newValuesTrial.Hospital, Is.EqualTo("Edited"));
            Assert.That(newValuesTrial.Practicioners.Count, Is.EqualTo(3));
            Assert.That(newValuesTrial.Practicioners.ElementAt(1).FirstName, Is.EqualTo("Edited"));
            Assert.That(newValuesTrial.Practicioners.ElementAt(1).Lastname, Is.EqualTo("Edited"));
            Assert.That(newValuesTrial.Practicioners.ElementAt(1).Rank, Is.EqualTo("Edited"));
            Assert.That(newValuesTrial.Practicioners.ElementAt(1).ScienceDegree, Is.EqualTo("Edited"));
            Assert.That(deletedPracticioner, Is.Not.Null);
            Assert.That(deletedPracticioner.IsDeleted, Is.True);
            Assert.That(deletedPracticioner.DeletedOn, Is.Not.Null);
        }

        [Test]
        public async Task SoftDeleteAsync_IsSettingIsDeletedAndDeletedOnProperties_WhenTrialExists()
        {
            // Arrange
            // Generate random guid Id.
            string trialId = Guid.NewGuid().ToString();

            // Create trial
            ClinicalTrial trial = new ClinicalTrial
            {
                Id = trialId,
                CreatedOn = DateTime.UtcNow,
                IsDeleted = false,
                DeletedOn = null,
                AgreementDocumentName = "Test",
                City = "Test",
                Hospital = "Test",
                Heading = "Test",
                CreatorId = doctorUserId,
                Practicioners = new List<Practicioner>
                {
                    new Practicioner
                    {
                        Id = "test-practicioner-1",
                        CreatedOn = DateTime.UtcNow,
                        IsDeleted = false,
                        DeletedOn = null,
                        FirstName = "Test",
                        Lastname = "Test",
                        Rank = "Acad.",
                        ScienceDegree = "PhD",
                    },

                    new Practicioner
                    {
                        Id = "test-practicioner-2",
                        CreatedOn = DateTime.UtcNow,
                        IsDeleted = false,
                        DeletedOn = null,
                        FirstName = "Test2",
                        Lastname = "Test2",
                        Rank = "Doc.",
                        ScienceDegree = "NoDegree",
                    },
                }
            };

            // Add trial to database.
            await this.dbContext.ClinicalTrials.AddAsync(trial);

            // Save changes in database.
            await this.dbContext.SaveChangesAsync();

            // Act
            Task result = this.clinicalTrialService.SoftDeleteAsync(trialId, doctorUserId);
            await result;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(trial.IsDeleted, Is.True);
            Assert.That(trial.DeletedOn, Is.Not.Null);
        }

        [Test]
        public void SoftDeleteAsync_ThrowsArgumentException_WhenScaleDoesNotExist()
        {
            // Act and Assert
            Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                Task result = this.clinicalTrialService.SoftDeleteAsync(Guid.NewGuid().ToString(), doctorUserId);
                await result;
            }, "trialId parameter tampering detected.");
        }

        [TearDown]
        public async Task TearDown()
        {
            await this.dbContext.DisposeAsync();
        }

        public async Task SeedTestData()
        {
            // Create test users.
            ApplicationUser doctorUser = new ApplicationUser
            {
                Id = doctorUserId,
                FirstName = "TestDoctor",
            };

            ApplicationUser patientUser = new ApplicationUser
            {
                Id = patientUserId,
                FirstName = "TestPatient",
            };

            // Generate random roleId guid.
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
