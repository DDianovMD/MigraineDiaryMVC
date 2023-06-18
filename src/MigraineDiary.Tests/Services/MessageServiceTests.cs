using Microsoft.EntityFrameworkCore;
using MigraineDiary.Data;
using MigraineDiary.Data.DbModels;
using MigraineDiary.Services;
using MigraineDiary.Tests.Mocks.Database;
using MigraineDiary.ViewModels;
using NUnit.Framework;

namespace MigraineDiary.Tests.Services
{
    [TestFixture]
    public class MessageServiceTests
    {
        private ApplicationDbContext dbContext;
        private MessageService messageService;
        private string firstTestMessageId;
        private string secondTestMessageId;

        [SetUp]
        public async Task Setup()
        {
            this.dbContext = InMemoryDatabase.Instance();
            this.messageService = new MessageService(this.dbContext);
            firstTestMessageId = Guid.NewGuid().ToString();
            secondTestMessageId = Guid.NewGuid().ToString();
            await this.SeedTestData();
        }

        [Test]
        public async Task AddAsync_IsAddingMessageToDatabase()
        {
            // Arrange
            MessageAddModel testMessage = new MessageAddModel
            {
                SenderName = "Test",
                SenderEmail = "test@test.com",
                Title = "Test",
                MessageContent = "Test",
            };

            // Act
            Task result = this.messageService.AddAsync(testMessage);
            await result;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(this.dbContext.Messages.Count, Is.EqualTo(3)); // 2 messages are seeded in Setup method.
        }

        [TestCase(1, 1, "NewestFirst")]
        [TestCase(1, 5, "NewestFirst")]
        [TestCase(1, 10, "NewestFirst")]
        [TestCase(1, 1, "OldestFirst")]
        [TestCase(1, 5, "OldestFirst")]
        [TestCase(1, 10, "OldestFirst")]
        public async Task GetAllAsync_ReturnsAllMessagesInDatabase(int pageIndex, int pageSize, string orderByDate)
        {
            // Arrange - in Setup method.

            // Act
            PaginatedList<MessageViewModel> result = await this.messageService.GetAllAsync(pageIndex, pageSize, orderByDate);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<PaginatedList<MessageViewModel>>());
            if (result.HasNextPage == false)
            {
                Assert.That(result.Count, Is.LessThanOrEqualTo(pageSize));
            }
            else if (result.HasNextPage == true)
            {
                Assert.That(result.Count, Is.EqualTo(pageSize));
            }
        }

        [Test]
        public async Task SoftDeleteAsync_IsSettingIsDeletedAndDeletedOnProperties()
        {
            // Arrange - in Setup method.
            Message? testMessage = await this.dbContext.Messages.FirstOrDefaultAsync(m => m.Id == firstTestMessageId);

            // Act
            await this.messageService.SoftDeleteAsync(firstTestMessageId);

            // Assert
            Assert.That(testMessage!.IsDeleted, Is.True);
            Assert.That(testMessage!.DeletedOn, Is.Not.Null);
        }

        [Test]
        public void SoftDeleteAsync_IsThrowingNullReferanceException_WhenMessageIdDoesNotExist()
        {
            // Arrange
            string randomGuid = Guid.NewGuid().ToString();

            // Act and Assert
            Assert.ThrowsAsync<NullReferenceException>(async () =>
            {
                var result = this.messageService.SoftDeleteAsync(randomGuid);
                await result;
            }, $"Подаденото Id ({randomGuid}) на съобщението не съществува.");
        }

        [Test]
        public async Task DeleteAsync_IsDeletingMessageFromDatabase()
        {
            // Arrange - in Setup method.

            // Act
            await this.messageService.DeleteAsync(firstTestMessageId);
            var result = await this.dbContext.Messages.FirstOrDefaultAsync(m => m.Id == firstTestMessageId);

            // Assert
            Assert.That(result, Is.Null);
        }


        [Test]
        public void DeleteAsync_IsThrowingNullReferanceException_WhenMessageIdDoesNotExist()
        {
            // Arrange
            string randomGuid = Guid.NewGuid().ToString();

            // Act and Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                var result = this.messageService.DeleteAsync(randomGuid);
                await result;
            }, $"Подаденото Id ({randomGuid}) на съобщението не съществува.");
        }

        [Test]
        public async Task GetMessagesCountAsync_ReturnsRealCount()
        {
            // Arrange - in Setup method.

            // Act
            int result = await this.messageService.GetMessagesCountAsync();

            // Assert
            Assert.That(result, Is.EqualTo(2));
        }

        [TearDown]
        public async Task TearDown()
        {
            await this.dbContext.DisposeAsync();
        }

        public async Task SeedTestData()
        {
            // Create test messages.
            Message testMessage = new Message
            {
                Id = firstTestMessageId,
                CreatedOn = DateTime.UtcNow,
                DeletedOn = null,
                IsDeleted = false,
                SenderName = "Test",
                SenderEmail = "Test",
                MessageContent = "Test",
                Title = "Test",
            };

            Message secondTestMessage = new Message
            {
                Id = secondTestMessageId,
                CreatedOn = DateTime.UtcNow,
                DeletedOn = null,
                IsDeleted = false,
                SenderName = "Test",
                SenderEmail = "Test",
                MessageContent = "Test",
                Title = "Test",
            };

            // Add messages to database.
            await this.dbContext.Messages.AddAsync(testMessage);
            await this.dbContext.Messages.AddAsync(secondTestMessage);

            // Save changes to database.
            await this.dbContext.SaveChangesAsync();
        }
    }
}
