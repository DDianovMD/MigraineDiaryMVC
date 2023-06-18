using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
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
    public class ZungScaleForAnxietyServiceTests
    {
        private string doctorUserId;
        private string patientUserId;
        private string testScaleId;

        private ZungScaleForAnxietyService zungScaleForAnxietyService;
        private ApplicationDbContext dbContext;

        [SetUp]
        public async Task SetUp()
        {
            doctorUserId = Guid.NewGuid().ToString();
            patientUserId = Guid.NewGuid().ToString();
            testScaleId = Guid.NewGuid().ToString();

            dbContext = InMemoryDatabase.Instance();
            this.zungScaleForAnxietyService = new ZungScaleForAnxietyService(this.dbContext);
            await this.SeedTestData(doctorUserId, patientUserId, testScaleId);
        }

        [Test]
        public async Task AddAsync_IsAddingZungScaleForAnxietyToDatabase()
        {
            // Arrange
            ApplicationUser patient = await this.dbContext.Users.FirstAsync(u => u.FirstName == "TestPatient");

            ZungScaleAddModel zungScaleForAnxiety = new ZungScaleAddModel
            {
                FirstQuestionAnswer = "Test",
                SecondQuestionAnswer = "Test",
                ThirdQuestionAnswer = "Test",
                FourthQuestionAnswer = "Test",
                FifthQuestionAnswer = "Test",
                SixthQuestionAnswer = "Test",
                SeventhQuestionAnswer = "Test",
                EighthQuestionAnswer = "Test",
                NinthQuestionAnswer = "Test",
                TenthQuestionAnswer = "Test",
                EleventhQuestionAnswer = "Test",
                TwelfthQuestionAnswer = "Test",
                ThirteenthQuestionAnswer = "Test",
                FourteenthQuestionAnswer = "Test",
                FifteenthQuestionAnswer = "Test",
                SixteenthQuestionAnswer = "Test",
                SeventeenthQuestionAnswer = "Test",
                EighteenthQuestionAnswer = "Test",
                NineteenthQuestionAnswer = "Test",
                TwentiethQuestionAnswer = "Test",
            };

            // Act
            Task result = this.zungScaleForAnxietyService.AddAsync(zungScaleForAnxiety, patient.Id);
            await result;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(patient.ZungScalesForAnxiety.Count(), Is.EqualTo(2)); // One scale is seeded in Setup method.
            Assert.That(this.dbContext.ZungScalesForAnxiety.Count(), Is.EqualTo(2));
            Assert.That(this.dbContext.ZungScalesForAnxiety.All(x => x.PatientId == patient.Id), Is.True);
        }

        [TestCase("Never or rarely")]
        [TestCase("Sometimes")]
        [TestCase("Often")]
        [TestCase("Very often or always")]
        public void CalculateResult_IsCalculatingAsExpected(string answer)
        {
            // Arrange.
            string[] answers = new string[20];

            // Fill array with identical answers.
            for (int i = 0; i < answers.Length; i++)
            {
                answers[i] = answer;
            }

            // Define expectedResult variable.
            int expectedResult = 0;

            // Set expectedResult value.
            switch (answer)
            {
                case "Never or rarely":
                    expectedResult = 20 * 1;
                    break;
                case "Sometimes":
                    expectedResult = 20 * 2;
                    break;
                case "Often":
                    expectedResult = 20 * 3;
                    break;
                case "Very often or always":
                    expectedResult = 20 * 4;
                    break;
                default:
                    break;
            }

            // Act
            int result = this.zungScaleForAnxietyService.CalculateTotalScore(answers);

            // Assert
            Assert.That(result, Is.Not.Zero);
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [TestCase("Never or rarely")]
        [TestCase("Sometimes")]
        [TestCase("Often")]
        [TestCase("Very often or always")]
        public async Task EditAsync_IsSettingCorrectAnswers_WhenScaleExists(string answer)
        {
            // Arrange
            // User and scale are seeded in Setup method.
            string[] newAnswers = new string[20];

            // Fill the array with identical answers.
            for (int i = 0; i < newAnswers.Length; i++)
            {
                newAnswers[i] = answer;
            }

            // Act
            Task result = this.zungScaleForAnxietyService.EditAsync(testScaleId, patientUserId, newAnswers);
            await result;

            // Get edited scale.
            ZungScaleForAnxiety editedScale = await this.dbContext.ZungScalesForAnxiety.FirstAsync(s => s.Id == testScaleId);

            // Assert
            Assert.That(editedScale.FirstQuestionAnswer, Is.EqualTo(answer));
            Assert.That(editedScale.SecondQuestionAnswer, Is.EqualTo(answer));
            Assert.That(editedScale.ThirdQuestionAnswer, Is.EqualTo(answer));
            Assert.That(editedScale.FourthQuestionAnswer, Is.EqualTo(answer));
            Assert.That(editedScale.FifthQuestionAnswer, Is.EqualTo(answer));
            Assert.That(editedScale.SixthQuestionAnswer, Is.EqualTo(answer));
            Assert.That(editedScale.SeventhQuestionAnswer, Is.EqualTo(answer));
            Assert.That(editedScale.EighthQuestionAnswer, Is.EqualTo(answer));
            Assert.That(editedScale.NinthQuestionAnswer, Is.EqualTo(answer));
            Assert.That(editedScale.TenthQuestionAnswer, Is.EqualTo(answer));
            Assert.That(editedScale.EleventhQuestionAnswer, Is.EqualTo(answer));
            Assert.That(editedScale.TwelfthQuestionAnswer, Is.EqualTo(answer));
            Assert.That(editedScale.ThirteenthQuestionAnswer, Is.EqualTo(answer));
            Assert.That(editedScale.FourteenthQuestionAnswer, Is.EqualTo(answer));
            Assert.That(editedScale.FifteenthQuestionAnswer, Is.EqualTo(answer));
            Assert.That(editedScale.SixteenthQuestionAnswer, Is.EqualTo(answer));
            Assert.That(editedScale.SeventeenthQuestionAnswer, Is.EqualTo(answer));
            Assert.That(editedScale.EighteenthQuestionAnswer, Is.EqualTo(answer));
            Assert.That(editedScale.NineteenthQuestionAnswer, Is.EqualTo(answer));
            Assert.That(editedScale.TwelfthQuestionAnswer, Is.EqualTo(answer));
        }

        [Test]
        public void EditAsync_ThrowsArgumentException_WhenScaleDoesNotExist()
        {
            // Arrange - in Setup method.

            // Act and Assert
            Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                var result = this.zungScaleForAnxietyService.EditAsync(Guid.NewGuid().ToString(), patientUserId, new string[6]);
                await result;
            }, "Скалата на Zung не съществува или не принадлежи на потребителя.");
        }

        [Test]
        public void EditAsync_ThrowsArgumentException_WhenScaleDoesNotBelongToLoggedUser()
        {
            // Arrange - in Setup method.

            // Act and Assert
            Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                var result = this.zungScaleForAnxietyService.EditAsync(testScaleId, doctorUserId, new string[6]);
                await result;
            }, "Скалата на Zung не съществува или не принадлежи на потребителя.");
        }

        [TestCase(1, 1, "NewestFirst")]
        [TestCase(1, 2, "NewestFirst")]
        [TestCase(1, 5, "NewestFirst")]
        [TestCase(1, 1, "OldestFirst")]
        [TestCase(1, 2, "OldestFirst")]
        [TestCase(1, 5, "OldestFirst")]
        public async Task GetAllAsync_ReturnsAllRegisteredZungScalesForAnxiety(int pageIndex, int pageSize, string orderByDate)
        {
            // Arrange
            // Add second scale (first is seeded in SeedTestData method called in Setup method).
            ZungScaleForAnxiety testScale = new ZungScaleForAnxiety
            {
                Id = Guid.NewGuid().ToString(),
                CreatedOn = DateTime.Now,
                IsDeleted = false,
                DeletedOn = null,
                FirstQuestionAnswer = "Test",
                SecondQuestionAnswer = "Test",
                ThirdQuestionAnswer = "Test",
                FourthQuestionAnswer = "Test",
                FifthQuestionAnswer = "Test",
                SixthQuestionAnswer = "Test",
                SeventhQuestionAnswer = "Test",
                EighthQuestionAnswer = "Test",
                NinthQuestionAnswer = "Test",
                TenthQuestionAnswer = "Test",
                EleventhQuestionAnswer = "Test",
                TwelfthQuestionAnswer = "Test",
                ThirteenthQuestionAnswer = "Test",
                FourteenthQuestionAnswer = "Test",
                FifteenthQuestionAnswer = "Test",
                SixteenthQuestionAnswer = "Test",
                SeventeenthQuestionAnswer = "Test",
                EighteenthQuestionAnswer = "Test",
                NineteenthQuestionAnswer = "Test",
                TwentiethQuestionAnswer = "Test",
                PatientId = patientUserId,
            };

            // Add scale to database.
            await this.dbContext.ZungScalesForAnxiety.AddAsync(testScale);

            // Save changes.
            await this.dbContext.SaveChangesAsync();

            // Act
            var result = await this.zungScaleForAnxietyService.GetAllAsync(patientUserId, pageIndex, pageSize, orderByDate);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<PaginatedList<ZungScaleViewModel>>());
            if (pageSize == 2)
            {
                Assert.That(result.Count(), Is.EqualTo(2));
            }
            else if (pageSize > 2)
            {
                Assert.That(result.Count(), Is.LessThanOrEqualTo(pageSize));
            }

            if (pageSize < 2)
            {
                Assert.That(result.Count(), Is.EqualTo(pageSize));
            }
        }

        [TestCase(1, 1, "NewestFirst")]
        [TestCase(1, 2, "NewestFirst")]
        [TestCase(1, 5, "NewestFirst")]
        [TestCase(1, 1, "OldestFirst")]
        [TestCase(1, 2, "OldestFirst")]
        [TestCase(1, 5, "OldestFirst")]
        public async Task GetSharedScalesAsync_ReturnsAllSharedZungScalesForAnxiety(int pageIndex, int pageSize, string orderByDate)
        {
            // Arrange
            // Get registered scales.
            ZungScaleForAnxiety[] zungScalesForAnxiety = await this.dbContext.ZungScalesForAnxiety
                                                                             .Where(s => s.PatientId == patientUserId)
                                                                             .Select(s => s)
                                                                             .ToArrayAsync();

            // Get TestDoctor user.
            ApplicationUser user = await this.dbContext.Users
                                                       .FirstAsync(u => u.Id == doctorUserId);

            // Share registered scales.
            foreach (var scale in zungScalesForAnxiety)
            {
                user.SharedZungScalesForAnxietyWithMe.Add(scale);
            }

            // Add second scale (first is seeded in SeedTestData method called in Setup method).
            ZungScaleForAnxiety secondScale = new ZungScaleForAnxiety
            {
                Id = Guid.NewGuid().ToString(),
                CreatedOn = DateTime.Now,
                IsDeleted = false,
                DeletedOn = null,
                FirstQuestionAnswer = "Test",
                SecondQuestionAnswer = "Test",
                ThirdQuestionAnswer = "Test",
                FourthQuestionAnswer = "Test",
                FifthQuestionAnswer = "Test",
                SixthQuestionAnswer = "Test",
                SeventhQuestionAnswer = "Test",
                EighthQuestionAnswer = "Test",
                NinthQuestionAnswer = "Test",
                TenthQuestionAnswer = "Test",
                EleventhQuestionAnswer = "Test",
                TwelfthQuestionAnswer = "Test",
                ThirteenthQuestionAnswer = "Test",
                FourteenthQuestionAnswer = "Test",
                FifteenthQuestionAnswer = "Test",
                SixteenthQuestionAnswer = "Test",
                SeventeenthQuestionAnswer = "Test",
                EighteenthQuestionAnswer = "Test",
                NineteenthQuestionAnswer = "Test",
                TwentiethQuestionAnswer = "Test",
                PatientId = patientUserId,
            };

            // Add scale to database.
            await this.dbContext.ZungScalesForAnxiety.AddAsync(secondScale);

            // Share second scale.
            user.SharedZungScalesForAnxietyWithMe.Add(secondScale);

            // Save changes in database.
            await this.dbContext.SaveChangesAsync();

            // Act
            var result = await this.zungScaleForAnxietyService.GetSharedScalesAsync(doctorUserId, patientUserId, pageIndex, pageSize, orderByDate);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<PaginatedList<SharedZungScaleForAnxietyViewModel>>());
            Assert.That(user.SharedZungScalesForAnxietyWithMe.All(s => s.PatientId == patientUserId), Is.True);
            if (pageSize == 2)
            {
                Assert.That(result.Count(), Is.EqualTo(2));
            }
            else if (pageSize > 2)
            {
                Assert.That(result.Count(), Is.LessThanOrEqualTo(pageSize));
            }

            if (pageSize < 2)
            {
                Assert.That(result.Count(), Is.EqualTo(pageSize));
            }
        }

        [Test]
        public async Task GetById_ReturnsScale_WhenScaleExistsAndBelongsToUser()
        {
            // Arrange
            // Get user.
            ApplicationUser testUser = await this.dbContext.Users.FirstAsync(u => u.FirstName == "TestPatient");

            // Act
            ZungScaleViewModel result = await zungScaleForAnxietyService.GetByIdAsync(testScaleId, patientUserId);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<ZungScaleViewModel>());
            Assert.That(result.Id, Is.EqualTo(testScaleId));
        }

        [Test]
        public async Task GetById_ReturnsNull_WhenScaleDoesNotExist()
        {
            // Arrange
            // Get user.
            ApplicationUser testUser = await this.dbContext.Users.FirstAsync(u => u.FirstName == "TestPatient");

            // Act
            ZungScaleViewModel result = await this.zungScaleForAnxietyService.GetByIdAsync(Guid.NewGuid().ToString(), testUser.Id);

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task GetById_ReturnsNull_WhenScaleDoesNotBelongToUser()
        {
            // Arrange
            // Generate random guid Id.
            string scaleId = Guid.NewGuid().ToString();

            // Create scale.
            ZungScaleForAnxiety testScale = new ZungScaleForAnxiety
            {
                Id = scaleId,
                CreatedOn = DateTime.UtcNow,
                IsDeleted = false,
                DeletedOn = null,
                PatientId = Guid.NewGuid().ToString(),
                FirstQuestionAnswer = "Test",
                SecondQuestionAnswer = "Test",
                ThirdQuestionAnswer = "Test",
                FourthQuestionAnswer = "Test",
                FifthQuestionAnswer = "Test",
                SixthQuestionAnswer = "Test",
                SeventhQuestionAnswer = "Test",
                EighthQuestionAnswer = "Test",
                NinthQuestionAnswer = "Test",
                TenthQuestionAnswer = "Test",
                EleventhQuestionAnswer = "Test",
                TwelfthQuestionAnswer = "Test",
                ThirteenthQuestionAnswer = "Test",
                FourteenthQuestionAnswer = "Test",
                FifteenthQuestionAnswer = "Test",
                SixteenthQuestionAnswer = "Test",
                SeventeenthQuestionAnswer = "Test",
                EighteenthQuestionAnswer = "Test",
                NineteenthQuestionAnswer = "Test",
                TwentiethQuestionAnswer = "Test",
            };

            // Add scale to database.
            await this.dbContext.ZungScalesForAnxiety.AddAsync(testScale);

            // Save changes.
            await this.dbContext.SaveChangesAsync();

            // Act
            ZungScaleViewModel result = await this.zungScaleForAnxietyService.GetByIdAsync(scaleId, patientUserId);

            // Assert
            Assert.That(testScale.PatientId, Is.Not.EqualTo(patientUserId));
            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task SoftDeleteAsync_IsSettingIsDeletedFlagToTrueAndDeletedOnProperty()
        {
            // Arrange
            // Get scale.
            ZungScaleForAnxiety? scale = await this.dbContext.ZungScalesForAnxiety.FirstOrDefaultAsync(s => s.Id == testScaleId);

            // Act
            Task result = this.zungScaleForAnxietyService.SoftDeleteAsync(testScaleId, patientUserId);

            // Assert
            Assert.That(scale, Is.Not.Null);
            Assert.That(scale.IsDeleted, Is.True);
            Assert.That(scale.DeletedOn, Is.Not.Null);
        }

        [Test]
        public void SoftDelete_ThrowsArgumentException_WhenScaleDoesNotBelongToUser()
        {
            // Arrange - in Setup method.

            // Act and Assert
            Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                Task result = this.zungScaleForAnxietyService.SoftDeleteAsync(testScaleId, doctorUserId);
                await result;
            }, "\"Скалата на Zung не съществува или не принадлежи на потребителя.\"");
        }

        [Test]
        public void SoftDelete_ThrowsArgumentException_WhenScaleDoesNotExist()
        {
            // Arrange - in Setup method.

            // Act and Assert
            Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                Task result = this.zungScaleForAnxietyService.SoftDeleteAsync(Guid.NewGuid().ToString(), doctorUserId);
                await result;
            }, "\"Скалата на Zung не съществува или не принадлежи на потребителя.\"");
        }

        [Test]
        public async Task Share_IsAddingZungScaleToDoctorsCollectionOfSharedZungScales_WhenScaleAndDoctorAreExisting()
        {
            // Arrange - in Setup method.
            // Get user in role "Doctor".
            ApplicationUser? doctor = await this.dbContext.Users.FirstOrDefaultAsync(u => u.Id == doctorUserId);

            // Get test scale entity.
            ZungScaleForAnxiety? testScale = await this.dbContext.ZungScalesForAnxiety.FirstOrDefaultAsync(s => s.Id == testScaleId);

            // Act
            Task result = this.zungScaleForAnxietyService.Share(testScaleId, doctorUserId);
            await result;

            // Assert.
            Assert.That(doctor, Is.Not.Null);
            Assert.That(testScale, Is.Not.Null);
            Assert.That(result, Is.Not.Null);
            Assert.That(doctor.SharedZungScalesForAnxietyWithMe.Contains(testScale), Is.True);
        }

        [Test]
        public async Task Share_ThrowsArgumentException_WhenZungScaleIsAlreadyShared()
        {
            // Arrange
            // Get user in role "Doctor".
            ApplicationUser? doctor = await this.dbContext.Users.FirstOrDefaultAsync(u => u.Id == doctorUserId);

            // Get test scale entity.
            ZungScaleForAnxiety? testScale = await this.dbContext.ZungScalesForAnxiety.FirstOrDefaultAsync(s => s.Id == testScaleId);

            // Share scale.
            doctor!.SharedZungScalesForAnxietyWithMe.Add(testScale!);

            // Save changes in database.
            await this.dbContext.SaveChangesAsync();

            // Act and Assert
            Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                Task result = this.zungScaleForAnxietyService.Share(testScaleId, doctor.Id);
                await result;
            }, "Scale is already shared.");
        }

        [Test]
        public void Share_ThrowsArgumentException_WhenDoctorDoesNotExist()
        {
            // Arrange - in Setup method.

            // Act and Assert
            Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                Task result = this.zungScaleForAnxietyService.Share(testScaleId, Guid.NewGuid().ToString());
                await result;
            }, "User doesn't exists or isn't in role \"Doctor\"");
        }

        [Test]
        public void Share_ThrowsArgumentException_WhenUserIsNotInRoleDoctor()
        {
            // Arrange - in Setup method.

            // Act and Assert
            Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                Task result = this.zungScaleForAnxietyService.Share(testScaleId, patientUserId);
                await result;
            }, "User doesn't exists or isn't in role \"Doctor\"");
        }

        [Test]
        public void Share_ThrowsArgumentException_WhenScaleDoesNotExist()
        {
            // Arrange - in Setup method.

            // Act and Assert
            Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                Task result = this.zungScaleForAnxietyService.Share(Guid.NewGuid().ToString(), doctorUserId);
                await result;
            }, "Scale doesn't exist.");
        }

        [TestCase("Never or rarely")]
        [TestCase("Sometimes")]
        [TestCase("Often")]
        [TestCase("Very often or always")]
        public void ValidateAnswer_ReturnsFalse_WhenAnswerIsValid(string answer)
        {
            // Act
            bool result = this.zungScaleForAnxietyService.ValidateAnswer(answer);

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void ValidateAnswer_ReturnsTrue_InCaseOfParameterTampering()
        {
            // Arrange
            string answer = Guid.NewGuid().ToString();

            // Act
            bool result = this.zungScaleForAnxietyService.ValidateAnswer(answer);

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void ValidateAnswer_ReturnsTrue_WhenAnswerIsNotGiven()
        {
            // Arrange
            string answer = "NoAnswer";

            // Act
            bool result = this.zungScaleForAnxietyService.ValidateAnswer(answer);

            // Assert
            Assert.That(result, Is.True);
        }

        [TearDown]
        public async Task TearDown()
        {
            await this.dbContext.DisposeAsync();
        }

        private async Task SeedTestData(string doctorUserId, string patientUserId, string testScaleId)
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

            ZungScaleForAnxiety testScale = new ZungScaleForAnxiety
            {
                Id = testScaleId,
                CreatedOn = DateTime.Now,
                IsDeleted = false,
                DeletedOn = null,
                FirstQuestionAnswer = "Test",
                SecondQuestionAnswer = "Test",
                ThirdQuestionAnswer = "Test",
                FourthQuestionAnswer = "Test",
                FifthQuestionAnswer = "Test",
                SixthQuestionAnswer = "Test",
                SeventhQuestionAnswer = "Test",
                EighthQuestionAnswer = "Test",
                NinthQuestionAnswer = "Test",
                TenthQuestionAnswer = "Test",
                EleventhQuestionAnswer = "Test",
                TwelfthQuestionAnswer = "Test",
                ThirteenthQuestionAnswer = "Test",
                FourteenthQuestionAnswer = "Test",
                FifteenthQuestionAnswer = "Test",
                SixteenthQuestionAnswer = "Test",
                SeventeenthQuestionAnswer = "Test",
                EighteenthQuestionAnswer = "Test",
                NineteenthQuestionAnswer = "Test",
                TwentiethQuestionAnswer = "Test",
                PatientId = patientUserId,
            };

            // Add scale to database.
            await this.dbContext.ZungScalesForAnxiety.AddAsync(testScale);

            // Save changes.
            await this.dbContext.SaveChangesAsync();
        }
    }
}
