using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MigraineDiary.Data;
using MigraineDiary.Data.DbModels;
using MigraineDiary.Services;
using MigraineDiary.Tests.Mocks.Database;
using MigraineDiary.Tests.Mocks.Models;
using MigraineDiary.ViewModels;
using NUnit.Framework;

namespace MigraineDiary.Tests.Services
{
    [TestFixture]
    public class HIT6ScaleServiceTests
    {
        private HIT6ScaleService hit6scaleService;
        private ApplicationDbContext dbContext;

        [SetUp]
        public async Task Setup()
        {
            this.dbContext = InMemoryDatabase.Instance();
            this.hit6scaleService = new HIT6ScaleService(this.dbContext);
            await SeedTestData();
        }

        [Test]
        public async Task AddAsync_IsAddingHIT6ScaleToDatabase()
        {
            // Arrange
            var hit6scale = HIT6ScaleAddModelMock.Instance("Never");
            string userId = await this.dbContext.Users
                                                .Where(x => x.FirstName == "TestUser")
                                                .Select(x => x.Id)
                                                .FirstAsync();

            // Act
            Task result = this.hit6scaleService.AddAsync(hit6scale, userId);
            await result;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(this.dbContext.HIT6Scales.Count(), Is.EqualTo(1));
            Assert.That(this.dbContext.HIT6Scales.All(x => x.PatientId == userId), Is.True);
        }

        [TestCase("Never")]
        [TestCase("Rarely")]
        [TestCase("Sometimes")]
        [TestCase("Very often")]
        [TestCase("Always")]
        public void CalculateResult_IsCalculatingAsExpected(string answer)
        {
            // Arrange.
            string[] answers = new string[6];

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
                case "Never":
                    expectedResult = 6 * 6;
                    break;
                case "Rarely":
                    expectedResult = 6 * 8;
                    break;
                case "Sometimes":
                    expectedResult = 6 * 10;
                    break;
                case "Very often":
                    expectedResult = 6 * 11;
                    break;
                case "Always":
                    expectedResult = 6 * 13;
                    break;
                default:
                    break;
            }

            // Act
            var result = this.hit6scaleService.CalculateTotalScore(answers);

            // Assert
            Assert.That(result, Is.Not.Zero);
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [TestCase("Never")]
        [TestCase("Rarely")]
        [TestCase("Sometimes")]
        [TestCase("Very often")]
        [TestCase("Always")]
        public async Task EditAsync_IsSettingCorrectAnswers_WhenScaleExists(string answer)
        {
            // Arrange
            string[] newAnswers = new string[6];

            // Fill the array with identical answers.
            for (int i = 0; i < newAnswers.Length; i++)
            {
                newAnswers[i] = answer;
            }

            // Generate random guid string used scale's Id value.
            string scaleId = Guid.NewGuid().ToString();

            // Create scale.
            HIT6Scale testScale = new HIT6Scale
            {
                Id = scaleId,
                FirstQuestionAnswer = "test",
                SecondQuestionAnswer = "test",
                ThirdQuestionAnswer = "test",
                FourthQuestionAnswer = "test",
                FifthQuestionAnswer = "test",
                SixthQuestionAnswer = "test",
            };

            // Get seeded TestUser.
            ApplicationUser testUser = await this.dbContext.Users.FirstAsync(x => x.FirstName == "TestUser");

            // Add scale to seeded user's scale collection.
            testUser.HIT6Scales.Add(testScale);

            // Save changes.
            await this.dbContext.SaveChangesAsync();

            // Act
            Task result = this.hit6scaleService.EditAsync(scaleId, testUser.Id, newAnswers);
            await result;

            // Get edited scale.
            HIT6Scale editedScale = await this.dbContext.HIT6Scales.FirstAsync(x => x.Id == scaleId);

            // Assert
            Assert.That(editedScale.FirstQuestionAnswer, Is.EqualTo(answer));
            Assert.That(editedScale.SecondQuestionAnswer, Is.EqualTo(answer));
            Assert.That(editedScale.ThirdQuestionAnswer, Is.EqualTo(answer));
            Assert.That(editedScale.FourthQuestionAnswer, Is.EqualTo(answer));
            Assert.That(editedScale.FifthQuestionAnswer, Is.EqualTo(answer));
            Assert.That(editedScale.SixthQuestionAnswer, Is.EqualTo(answer));
        }

        [Test]
        public async Task EditAsync_ThrowsArgumentException_WhenScaleDoesNotExist()
        {
            // Arrange
            // Get seeded TestUser.
            string userId = await this.dbContext.Users
                                                .Where(x => x.FirstName == "TestUser")
                                                .Select(x => x.Id)
                                                .FirstAsync();

            // Act and Assert
            Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                var result = this.hit6scaleService.EditAsync(Guid.NewGuid().ToString(), userId, new string[6]);
                await result;
            }, "HIT-6 скалата не съществува или не принадлежи на потребителя.");
        }

        [TestCase(1, 1, "NewestFirst")]
        [TestCase(1, 2, "NewestFirst")]
        [TestCase(1, 5, "NewestFirst")]
        [TestCase(1, 1, "OldestFirst")]
        [TestCase(1, 2, "OldestFirst")]
        [TestCase(1, 5, "OldestFirst")]
        public async Task GetAllAsync_ReturnsAllRegisteredScales(int pageIndex, int pageSize, string orderByDate)
        {
            // Arrange
            ApplicationUser testUser = this.dbContext.Users.First(u => u.FirstName == "TestUser");

            HIT6Scale testScale = new HIT6Scale
            {
                Id = Guid.NewGuid().ToString(),
                CreatedOn = DateTime.UtcNow,
                IsDeleted = false,
                DeletedOn = null,
                PatientId = testUser.Id,
                FirstQuestionAnswer = "Test",
                SecondQuestionAnswer = "Test",
                ThirdQuestionAnswer = "Test",
                FourthQuestionAnswer = "Test",
                FifthQuestionAnswer = "Test",
                SixthQuestionAnswer = "Test",
            };

            HIT6Scale secondTestScale = new HIT6Scale
            {
                Id = Guid.NewGuid().ToString(),
                CreatedOn = DateTime.UtcNow,
                IsDeleted = false,
                DeletedOn = null,
                PatientId = testUser.Id,
                FirstQuestionAnswer = "Test",
                SecondQuestionAnswer = "Test",
                ThirdQuestionAnswer = "Test",
                FourthQuestionAnswer = "Test",
                FifthQuestionAnswer = "Test",
                SixthQuestionAnswer = "Test",
            };

            testUser.HIT6Scales.Add(testScale);
            testUser.HIT6Scales.Add(secondTestScale);
            await this.dbContext.SaveChangesAsync();

            // Act
            var result = await this.hit6scaleService.GetAllAsync(testUser.Id, pageIndex, pageSize, orderByDate);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<PaginatedList<HIT6ScaleViewModel>>());
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
        public async Task GetSharedScalesAsync_ReturnsAllSharedScales(int pageIndex, int pageSize, string orderByDate)
        {
            // Arrange
            ApplicationUser testDoctor = this.dbContext.Users.First(u => u.FirstName == "TestUser");
            ApplicationUser testPatient = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
            };

            HIT6Scale testScale = new HIT6Scale
            {
                Id = Guid.NewGuid().ToString(),
                CreatedOn = DateTime.UtcNow,
                IsDeleted = false,
                DeletedOn = null,
                PatientId = testPatient.Id,
                FirstQuestionAnswer = "Test",
                SecondQuestionAnswer = "Test",
                ThirdQuestionAnswer = "Test",
                FourthQuestionAnswer = "Test",
                FifthQuestionAnswer = "Test",
                SixthQuestionAnswer = "Test",
            };

            HIT6Scale secondTestScale = new HIT6Scale
            {
                Id = Guid.NewGuid().ToString(),
                CreatedOn = DateTime.UtcNow,
                IsDeleted = false,
                DeletedOn = null,
                PatientId = testPatient.Id,
                FirstQuestionAnswer = "Test",
                SecondQuestionAnswer = "Test",
                ThirdQuestionAnswer = "Test",
                FourthQuestionAnswer = "Test",
                FifthQuestionAnswer = "Test",
                SixthQuestionAnswer = "Test",
            };

            testPatient.HIT6Scales.Add(testScale);
            testPatient.HIT6Scales.Add(secondTestScale);
            testDoctor.SharedHIT6ScalesWithMe.Add(testScale);
            testDoctor.SharedHIT6ScalesWithMe.Add(secondTestScale);

            await this.dbContext.Users.AddAsync(testPatient);
            await this.dbContext.SaveChangesAsync();

            // Act
            var result = await this.hit6scaleService.GetSharedScalesAsync(testDoctor.Id, testPatient.Id, pageIndex, pageSize, orderByDate);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<PaginatedList<SharedHIT6ScaleViewModel>>());
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
            ApplicationUser testUser = await this.dbContext.Users.FirstAsync(u => u.FirstName == "TestUser");

            // Generate random guid Id.
            string scaleId = Guid.NewGuid().ToString();

            // Create scale.
            HIT6Scale testScale = new HIT6Scale
            {
                Id = scaleId,
                CreatedOn = DateTime.Now,
                IsDeleted = false,
                DeletedOn = null,
                FirstQuestionAnswer = "Test",
                SecondQuestionAnswer = "Test",
                ThirdQuestionAnswer = "Test",
                FourthQuestionAnswer = "Test",
                FifthQuestionAnswer = "Test",
                SixthQuestionAnswer = "Test",
            };

            // Add scale to user's collection of registered scales.
            testUser.HIT6Scales.Add(testScale);

            // Save changes to database.
            await this.dbContext.SaveChangesAsync();

            // Act
            HIT6ScaleViewModel result = await hit6scaleService.GetByIdAsync(scaleId, testUser.Id);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<HIT6ScaleViewModel>());
            Assert.That(result.Id, Is.EqualTo(scaleId));
        }

        [Test]
        public async Task GetById_ReturnsNull_WhenScaleDoesNotExist()
        {
            // Arrange
            // Get user.
            ApplicationUser testUser = await this.dbContext.Users.FirstAsync(u => u.FirstName == "TestUser");

            // Act
            HIT6ScaleViewModel result = await this.hit6scaleService.GetByIdAsync(Guid.NewGuid().ToString(), testUser.Id);

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
            HIT6Scale testScale = new HIT6Scale
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
            };

            // Add scale to database.
            await this.dbContext.HIT6Scales.AddAsync(testScale);

            // Save changes.
            await this.dbContext.SaveChangesAsync();

            // Generate random guid Id (not existing user in role "Doctor").
            string doctorId = Guid.NewGuid().ToString();

            // Act
            HIT6ScaleViewModel result = await this.hit6scaleService.GetByIdAsync(scaleId, doctorId);

            // Assert
            Assert.That(testScale.PatientId, Is.Not.EqualTo(doctorId));
            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task SoftDeleteAsync_IsSettingIsDeletedFlagToTrueAndDeletedOnProperty()
        {
            // Arrange
            ApplicationUser testDoctor = this.dbContext.Users.First(u => u.FirstName == "TestUser");
            string scaleId = Guid.NewGuid().ToString();

            HIT6Scale testScale = new HIT6Scale
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
            };

            testDoctor.HIT6Scales.Add(testScale);
            await this.dbContext.SaveChangesAsync();

            // Act
            Task result = this.hit6scaleService.SoftDeleteAsync(scaleId, testDoctor.Id);

            // Assert
            Assert.That(testScale.IsDeleted, Is.True);
            Assert.That(testScale.DeletedOn, Is.Not.Null);
        }

        [Test]
        public async Task SoftDelete_ThrowsArgumentException_WhenScaleDoesNotBelongToUser()
        {
            // Arrange
            ApplicationUser testPatient = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
            };

            string scaleId = Guid.NewGuid().ToString();

            HIT6Scale testScale = new HIT6Scale
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
            };

            testPatient.HIT6Scales.Add(testScale);
            await this.dbContext.Users.AddAsync(testPatient);
            await this.dbContext.SaveChangesAsync();

            // Act and Assert

            Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                Task result = this.hit6scaleService.SoftDeleteAsync(scaleId, Guid.NewGuid().ToString());
                await result;
            }, "HIT-6 скалата не съществува или не принадлежи на потребителя.");
        }

        [Test]
        public async Task Share_IsAddingHIT6ScaleToDoctorsCollectionOfSharedHIT6Scales_WhenScaleAndDoctorAreExisting()
        {
            // Arrange
            // Role is assigned to user in SeedTestData method which is called in Setup method.
            ApplicationUser doctor = await this.dbContext.Users.FirstAsync(u => u.FirstName == "TestUser");

            // Generate random testScale guid Id.
            string testScaleId = Guid.NewGuid().ToString();

            HIT6Scale testScale = new HIT6Scale
            {
                Id = testScaleId,
                CreatedOn = DateTime.UtcNow,
                IsDeleted = false,
                DeletedOn = null,
                FirstQuestionAnswer = "Test",
                SecondQuestionAnswer = "Test",
                ThirdQuestionAnswer = "Test",
                FourthQuestionAnswer = "Test",
                FifthQuestionAnswer = "Test",
                SixthQuestionAnswer = "Test",
                PatientId = Guid.NewGuid().ToString(),
            };

            await this.dbContext.HIT6Scales.AddAsync(testScale);

            // Save changes in database.
            await this.dbContext.SaveChangesAsync();

            // Act
            Task result = this.hit6scaleService.Share(testScaleId, doctor.Id);
            await result;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(doctor.SharedHIT6ScalesWithMe.Contains(testScale), Is.True);
        }

        [Test]
        public async Task Share_ThrowsArgumentException_WhenHIT6ScaleIsAlreadyShared()
        {
            // Arrange
            // Role is assigned to user in SeedTestData method which is called in Setup method.
            ApplicationUser doctor = await this.dbContext.Users.FirstAsync(u => u.FirstName == "TestUser");

            // Generate random testScale guid Id.
            string testScaleId = Guid.NewGuid().ToString();

            HIT6Scale testScale = new HIT6Scale
            {
                Id = testScaleId,
                CreatedOn = DateTime.UtcNow,
                IsDeleted = false,
                DeletedOn = null,
                FirstQuestionAnswer = "Test",
                SecondQuestionAnswer = "Test",
                ThirdQuestionAnswer = "Test",
                FourthQuestionAnswer = "Test",
                FifthQuestionAnswer = "Test",
                SixthQuestionAnswer = "Test",
                PatientId = Guid.NewGuid().ToString(),
            };

            // Add testScale in database.
            await this.dbContext.HIT6Scales.AddAsync(testScale);

            // Share scale with "Doctor" user.
            doctor.SharedHIT6ScalesWithMe.Add(testScale);

            // Save changes in database.
            await this.dbContext.SaveChangesAsync();

            // Act and Assert
            Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                Task result = this.hit6scaleService.Share(testScaleId, doctor.Id);
                await result;
            }, "Scale is already shared.");
        }

        [Test]
        public async Task Share_ThrowsArgumentException_WhenDoctorDoesNotExist()
        {
            // Arrange
            // Generate random testScale guid Id.
            string testScaleId = Guid.NewGuid().ToString();

            HIT6Scale testScale = new HIT6Scale
            {
                Id = testScaleId,
                CreatedOn = DateTime.UtcNow,
                IsDeleted = false,
                DeletedOn = null,
                FirstQuestionAnswer = "Test",
                SecondQuestionAnswer = "Test",
                ThirdQuestionAnswer = "Test",
                FourthQuestionAnswer = "Test",
                FifthQuestionAnswer = "Test",
                SixthQuestionAnswer = "Test",
                PatientId = Guid.NewGuid().ToString(),
            };

            // Add testScale in database.
            await this.dbContext.HIT6Scales.AddAsync(testScale);

            // Save changes in database.
            await this.dbContext.SaveChangesAsync();

            // Act and Assert
            Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                Task result = this.hit6scaleService.Share(testScaleId, Guid.NewGuid().ToString());
                await result;
            }, "User doesn't exists or isn't in role \"Doctor\"");
        }

        [Test]
        public async Task Share_ThrowsArgumentException_WhenUserIsNotInRoleDoctor()
        {
            // Arrange
            // Get user's Id which is in role "Doctor"
            string doctorId = this.dbContext.Users.First(u => u.FirstName == "TestUser").Id;

            // Get "Doctor" role entity.
            IdentityRole doctorRole = this.dbContext.Roles.First(r => r.Name == "Doctor");

            // Remove "Doctor" role.
            this.dbContext.Roles.Remove(doctorRole);

            // Generate random testScale guid Id.
            string testScaleId = Guid.NewGuid().ToString();

            HIT6Scale testScale = new HIT6Scale
            {
                Id = testScaleId,
                CreatedOn = DateTime.UtcNow,
                IsDeleted = false,
                DeletedOn = null,
                FirstQuestionAnswer = "Test",
                SecondQuestionAnswer = "Test",
                ThirdQuestionAnswer = "Test",
                FourthQuestionAnswer = "Test",
                FifthQuestionAnswer = "Test",
                SixthQuestionAnswer = "Test",
                PatientId = Guid.NewGuid().ToString(),
            };

            // Add testScale in database.
            await this.dbContext.HIT6Scales.AddAsync(testScale);

            // Save changes in database.
            await this.dbContext.SaveChangesAsync();

            // Act and Assert
            Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                Task result = this.hit6scaleService.Share(testScaleId, Guid.NewGuid().ToString());
                await result;
            }, "User doesn't exists or isn't in role \"Doctor\"");
        }

        [Test]
        public void Share_ThrowsArgumentException_WhenScaleDoesNotExist()
        {
            // Arrange
            string doctorId = this.dbContext.Users.First(u => u.FirstName == "TestUser").Id;

            // Act and Assert
            Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                Task result = this.hit6scaleService.Share(Guid.NewGuid().ToString(), doctorId);
                await result;
            }, "Scale doesn't exist.");
        }

        [TestCase("Never")]
        [TestCase("Rarely")]
        [TestCase("Sometimes")]
        [TestCase("Very often")]
        [TestCase("Always")]
        public void ValidateAnswer_ReturnsFalse_WhenAnswerIsValid(string answer)
        {
            // Act
            bool result = this.hit6scaleService.ValidateAnswer(answer);

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void ValidateAnswer_ReturnsTrue_InCaseOfParameterTampering()
        {
            // Arrange
            string answer = Guid.NewGuid().ToString();

            // Act
            bool result = this.hit6scaleService.ValidateAnswer(answer);

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void ValidateAnswer_ReturnsTrue_WhenAnswerIsNotGiven()
        {
            // Arrange
            string answer = "NoAnswer";

            // Act
            bool result = this.hit6scaleService.ValidateAnswer(answer);

            // Assert
            Assert.That(result, Is.True);
        }

        [TearDown]
        public async Task TearDown()
        {
            await this.dbContext.DisposeAsync();
        }

        private async Task SeedTestData()
        {
            // Create test user.
            string userId = Guid.NewGuid().ToString();

            ApplicationUser user = new ApplicationUser
            {
                Id = userId,
                FirstName = "TestUser",
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
                UserId = user.Id,
            };

            // Add role and assign it to doctor user.
            await this.dbContext.Roles.AddAsync(role);
            await this.dbContext.UserRoles.AddAsync(userRole);

            // Add user to database.
            await this.dbContext.Users.AddAsync(user);

            // Save changes.
            await this.dbContext.SaveChangesAsync();
        }
    }
}
