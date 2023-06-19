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
    public class PatientServiceTests
    {
        private string doctorUserId;
        private string patientUserId;
        private string secondPatientUserId;
        private ApplicationDbContext dbContext;
        private IPatientService patientService;

        [SetUp]
        public async Task Setup()
        {
            this.dbContext = InMemoryDatabase.Instance();
            this.patientService = new PatientService(this.dbContext);

            this.doctorUserId = Guid.NewGuid().ToString();
            this.patientUserId = Guid.NewGuid().ToString();
            this.secondPatientUserId = Guid.NewGuid().ToString();

            await this.SeedTestUsers();
        }

        [Test]
        public async Task GetAllPatientsAsync_ReturnsExpectedPatientsCount()
        {
            // Arrange
            // Create headaches.
            Headache testHeadache = new Headache
            {
                Id = Guid.NewGuid().ToString(),
                Onset = DateTime.UtcNow,
                LocalizationSide = "test1",
                PainCharacteristics = "test1",
            };

            HIT6Scale testHIT6Scale = new HIT6Scale
            {
                Id = Guid.NewGuid().ToString(),
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

            ZungScaleForAnxiety testZungScaleForAnxiety = new ZungScaleForAnxiety
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

            // Get users.
            ApplicationUser? doctor = await this.dbContext.Users.FirstOrDefaultAsync(u => u.Id == doctorUserId);
            ApplicationUser? firstPatient = await this.dbContext.Users.FirstOrDefaultAsync(u => u.Id == patientUserId);
            ApplicationUser? secondPatient = await this.dbContext.Users.FirstOrDefaultAsync(u => u.Id == secondPatientUserId);

            // Add test data to user's collections.
            firstPatient!.Headaches.Add(testHeadache);

            secondPatient!.HIT6Scales.Add(testHIT6Scale);
            secondPatient.ZungScalesForAnxiety.Add(testZungScaleForAnxiety);

            // Share scales.
            doctor!.SharedWithMe.Add(testHeadache);
            doctor.SharedHIT6ScalesWithMe.Add(testHIT6Scale);
            doctor.SharedZungScalesForAnxietyWithMe.Add(testZungScaleForAnxiety);

            // Save changes in database.
            await this.dbContext.SaveChangesAsync();

            // Act
            PatientViewModel[] patients = await this.patientService.GetAllPatientsAsync(doctorUserId);

            // Assert
            Assert.That(patients, Is.Not.Null);
            Assert.That(patients.Length, Is.EqualTo(2));
            Assert.That(patients.All(p => p.PatientId == patientUserId || p.PatientId == secondPatientUserId), Is.True);
        }

        [TearDown]
        public async Task TearDown()
        {
            await this.dbContext.DisposeAsync();
        }

        public async Task SeedTestUsers()
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

            ApplicationUser secondPatientUser = new ApplicationUser
            {
                Id = secondPatientUserId,
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
            await this.dbContext.Users.AddAsync(secondPatientUser);

            // Save changes in database.
            await this.dbContext.SaveChangesAsync();
        }
    }
}
