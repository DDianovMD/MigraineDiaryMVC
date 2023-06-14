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
    public class ArticleServiceTests
    {
        private ArticleService articleService;
        private ApplicationDbContext dbContext;

        [SetUp]
        public async Task Setup()
        {
            this.dbContext = InMemoryDatabase.Instance();
            this.articleService = new ArticleService(this.dbContext);
            await SeedTestData();
        }

        [Test]
        public async Task CreateArticle_IsInsertingNewArticleInDatabase()
        {
            // Arrange
            ArticleAddFormModel articleModel = ArticleAddFormModelMock.Instance();
            string userId = await this.dbContext.Users
                                                .Where(u => u.FirstName == "TestUser")
                                                .Select(u => u.Id)
                                                .FirstAsync();

            // Act
            Task result = this.articleService.CreateArticle(articleModel, userId);
            await result;

            // Get article
            Article? article = await this.dbContext.Articles.SingleOrDefaultAsync(x => x.Content == articleModel.Content);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsCompleted, Is.True);
            Assert.That(this.dbContext.Articles.Count(), Is.EqualTo(1));
            Assert.That(article, Is.Not.Null);
        }

        [TestCase(1, 1, "NewestFirst")]
        [TestCase(1, 5, "NewestFirst")]
        [TestCase(1, 10, "NewestFirst")]
        [TestCase(1, 1, "OldestFirst")]
        [TestCase(1, 5, "OldestFirst")]
        [TestCase(1, 10, "OldestFirst")]
        public async Task GetArticles_ReturnsOrderedCollectionOfArticles(int pageIndex, int pageSize, string orderByDate)
        {
            // Arrange
            List<Article> articles = new List<Article>
            {
                new Article
                {
                    Id = Guid.NewGuid().ToString(),
                    Title = "Test",
                    Author = "Test",
                    Content = "Test",
                    CreatorId = Guid.NewGuid().ToString(),
                },

                new Article
                {
                    Id= Guid.NewGuid().ToString(),
                    Title= "Test",
                    Author = "Test",
                    Content = "Test",
                    CreatorId = Guid.NewGuid().ToString(),
                }
            };

            await this.dbContext.Articles.AddRangeAsync(articles);
            await this.dbContext.SaveChangesAsync();

            // Act
            var result = await this.articleService.GetArticles(pageIndex, pageSize, orderByDate);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<PaginatedList<ArticleViewModel>>());
            Assert.That(result.Count(), Is.GreaterThan(0));
            Assert.That(result.Count(), Is.LessThanOrEqualTo(pageSize));
        }

        [TearDown]
        public async Task Teardown()
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

            // Add user to database.
            await this.dbContext.Users.AddAsync(user);

            // Save changes.
            await this.dbContext.SaveChangesAsync();
        }
    }
}
