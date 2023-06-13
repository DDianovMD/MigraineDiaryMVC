using MigraineDiary.Data;
using MigraineDiary.Data.DbModels;
using MigraineDiary.Tests.Mocks.Database;
using MigraineDiary.ViewModels;
using NUnit.Framework;

namespace MigraineDiary.Tests.Mocks.Models
{
    [TestFixture]
    public class PaginatedListTests
    {
        private ApplicationDbContext dbContext = InMemoryDatabase.Instance();

        [OneTimeSetUp]
        public async Task SeedTestData()
        {
            List<Message> testMessages = new List<Message>();

            for (int i = 0; i < 5; i++)
            {
                var message = new Message
                {
                    Id = Guid.NewGuid().ToString(),
                    MessageContent = "test1",
                    CreatedOn = DateTime.UtcNow,
                    DeletedOn = null,
                    IsDeleted = false,
                    SenderEmail = "test@test.com",
                    SenderName = "Test",
                    Title = $"Test{i}"
                };

                testMessages.Add(message);
            }

            await this.dbContext.Messages.AddRangeAsync(testMessages);
            await this.dbContext.SaveChangesAsync();
        }

        [TestCase(1, 2)]
        [TestCase(2, 2)]
        public void ReturnedCollectionHasPageSizeCount_WhenSourceHasEnoughEntities(int pageIndex, int pageSize)
        {
            // Arrange
            ICollection<string> testCollection = new List<string>
            {
                "Test string 1",
                "Test string 2",
                "Test string 3",
                "Test string 4",
                "Test string 5",
            };

            // Act
            var result = PaginatedList<string>.CreateAsync(testCollection, pageIndex, pageSize);

            // Assert
            Assert.That(result.Count(), Is.EqualTo(pageSize));
        }

        [TestCase(1, 1)]
        [TestCase(1, 2)]
        [TestCase(2, 1)]
        [TestCase(2, 2)]
        public async Task ReturnedCollectionHasPageSizeCount_WhenSourceIsIQueryableAndHasEnoughEntities(int pageIndex, int pageSize)
        {
            // Arrange
            IQueryable<Message> queryable = this.dbContext.Messages;

            // Act
            PaginatedList<Message> result = await PaginatedList<Message>.CreateAsync(queryable, pageIndex, pageSize);

            // Assert
            Assert.That(result.Count(), Is.EqualTo(pageSize));
        }

        [TestCase(1, 3)]
        [TestCase(2, 3)]
        [TestCase(3, 2)]
        [TestCase(4, 1)]
        public void ReturnedCollectionCountIsLessThanPageSize_WhenSourceDoesNotHaveEnoughEntities(int pageIndex, int pageSize)
        {
            // Arrange
            ICollection<string> testCollection = new List<string>
            {
                "Test string 1",
                "Test string 2",
            };

            // Act
            var result = PaginatedList<string>.CreateAsync(testCollection, pageIndex, pageSize);

            // Assert
            Assert.That(result.Count(), Is.LessThan(pageSize));
        }

        [TestCase(1, 6)]
        [TestCase(2, 3)]
        [TestCase(3, 2)]
        public async Task ReturnedCollectionCountIsLessThanPageSize_WhenSourceIsIQueryableAndDoesNotHaveEnoughEntities(int pageIndex, int pageSize)
        {
            // Arrange
            IQueryable<Message> queryable = this.dbContext.Messages;

            // Act
            var result = await PaginatedList<Message>.CreateAsync(queryable, pageIndex, pageSize);

            // Assert
            Assert.That(result.Count(), Is.LessThan(pageSize));
        }

        [TestCase(0, 0)]
        [TestCase(0, 1)]
        [TestCase(1, 0)]
        [TestCase(1, 1)]
        [TestCase(5, 1)]
        public void TotalPagesCount_IsEqualToOne_WhenSourceCollectionIsEmpty(int pageIndex, int pageSize)
        {
            // Arrange
            var emptyCollection = new List<string>();

            // Act
            var result = PaginatedList<string>.CreateAsync(emptyCollection, pageIndex, pageSize);

            // Assert
            Assert.That(result.TotalPagesCount, Is.EqualTo(1));
            Assert.That(result.HasNextPage, Is.False);
            Assert.That(result.HasPreviousPage, Is.False);
            Assert.That(result.TotalPagesCount, Is.EqualTo(result.CurrentPageIndex));
        }
    }
}
