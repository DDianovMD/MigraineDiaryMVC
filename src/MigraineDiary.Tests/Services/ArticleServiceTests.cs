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

        [Test]
        public async Task GetByIdAsync_ReturnsArticleWithGivenId_WhenArticleExists()
        {
            // Arrange
            string articleId = Guid.NewGuid().ToString();
            Article article = new Article
            {
                Id = articleId,
                Author = "TestAuthor",
                Title = "TestTitle",
                Content = "TestContent",
                SourceUrl = "TestSourceUrl",
                CreatorId = Guid.NewGuid().ToString(),
            };

            await this.dbContext.Articles.AddAsync(article);
            await this.dbContext.SaveChangesAsync();

            // Act
            ArticleEditModel result = await this.articleService.GetByIdAsync(articleId);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<ArticleEditModel>());
            Assert.That(result.Id, Is.EqualTo(articleId));
        }

        [Test]
        public void GetByIdAsync_ThrowsArgumentException_WhenArticleDoesNotExist()
        {
            // Act and Assert
            Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                Task result = this.articleService.GetByIdAsync(Guid.NewGuid().ToString());
                await result;
            }, "Parameter tampering detected.");
        }

        [TestCase(1, 2, "NewestFirst")]
        [TestCase(1, 5, "NewestFirst")]
        [TestCase(1, int.MaxValue, "NewestFirst")]
        [TestCase(1, 2, "OldestFirst")]
        [TestCase(1, 5, "OldestFirst")]
        [TestCase(1, int.MaxValue, "OldestFirst")]
        public async Task GetArchivedArticles_ReturnsAllArticlesMarkedAsDeleted(int pageIndex, int pageSize, string orderByDate)
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
                    IsDeleted = false,
                    CreatedOn = DateTime.Now,
                },

                new Article
                {
                    Id = Guid.NewGuid().ToString(),
                    Title= "Test",
                    Author = "Test",
                    Content = "Test",
                    CreatorId = Guid.NewGuid().ToString(),
                    IsDeleted = true,
                    CreatedOn = DateTime.Now,
                },

                new Article
                {
                    Id = Guid.NewGuid().ToString(),
                    Title= "Test",
                    Author = "Test",
                    Content = "Test",
                    CreatorId = Guid.NewGuid().ToString(),
                    IsDeleted = true,
                    CreatedOn = DateTime.Now.AddDays(1).AddMinutes(15),
                },

                new Article
                {
                    Id = Guid.NewGuid().ToString(),
                    Title= "Test",
                    Author = "Test",
                    Content = "Test",
                    CreatorId = Guid.NewGuid().ToString(),
                    IsDeleted = true,
                    CreatedOn = DateTime.Now.AddDays(2).AddMinutes(15),
                }
            };

            await this.dbContext.Articles.AddRangeAsync(articles);
            await this.dbContext.SaveChangesAsync();

            // Act
            PaginatedList<ArticleViewModel> result = await this.articleService.GetArchivedArticles(pageIndex, pageSize, orderByDate);

            // Assert
            Assert.That(result, Is.Not.Null);
            if (pageIndex * pageSize > this.dbContext.Articles.Where(a => a.IsDeleted == true).Count())
            {
                Assert.That(result.Count, Is.LessThanOrEqualTo(pageSize));
            }
            else if (pageIndex * pageSize <= this.dbContext.Articles.Where(a => a.IsDeleted == true).Count())
            {
                Assert.That(result.Count, Is.EqualTo(pageSize));
            }


            if (orderByDate == "NewestFirst")
            {
                Assert.That(result.ElementAt(0).CreatedOn, Is.GreaterThan(result.ElementAt(1).CreatedOn));
            }
            else
            {
                Assert.That(result.ElementAt(0).CreatedOn, Is.LessThan(result.ElementAt(1).CreatedOn));
            }
        }

        [Test]
        public async Task EditAsync_IsChangingAllScalePropertyValues()
        {
            // Arrange
            string articleId = Guid.NewGuid().ToString();

            Article article = new Article
            {
                Id = articleId,
                Author = "TestAuthor",
                Title = "TestTitle",
                Content = "TestContent",
                SourceUrl = "TestSourceUrl",
                CreatorId = Guid.NewGuid().ToString(),
            };

            await this.dbContext.Articles.AddAsync(article);
            await this.dbContext.SaveChangesAsync();

            ArticleEditModel editModel = new ArticleEditModel
            {
                Id = articleId,
                Author = "EditedAuthor",
                Content = "EditedContent",
                SourceUrl = "EditedSourceUrl",
                Title = "EditedTitle",
            };

            // Act
            Task result = this.articleService.EditAsync(editModel);
            await result;

            // Assert
            Article? editedArticle = await this.dbContext.Articles.FirstOrDefaultAsync(a => a.Id == articleId);

            Assert.That(editedArticle, Is.Not.Null);
            Assert.That(editedArticle.Title, Is.EqualTo(editModel.Title));
            Assert.That(editedArticle.Author, Is.EqualTo(editModel.Author));
            Assert.That(editedArticle.Content, Is.EqualTo(editModel.Content));
            Assert.That(editedArticle.SourceUrl, Is.EqualTo(editModel.SourceUrl));
        }

        [Test]
        public async Task EditAsync_IsThrowingArgumentException_WhenArticleIdIsTampered()
        {
            // Arrange
            Article article = new Article
            {
                Id = Guid.NewGuid().ToString(),
                Author = "Test",
                Content = "Test",
                Title = "Test",
                SourceUrl = "Test",
                CreatorId = Guid.NewGuid().ToString(),
            };

            await this.dbContext.Articles.AddAsync(article);
            await this.dbContext.SaveChangesAsync();

            ArticleEditModel articleEditModel = new ArticleEditModel
            {
                Id = Guid.NewGuid().ToString(),
                Author = "Edited",
                Content = "Edited",
                Title = "Edited",
                SourceUrl = "Edited",
            };

            // Act and Assert
            Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                Task result = this.articleService.EditAsync(articleEditModel);
                await result;
            }, "Parameter tampering detected");
        }

        [Test]
        public async Task SoftDeleteAsync_IsSettingIsDeletedAndDeletedOnProperties_WhenArticleExists()
        {
            // Arrange
            string articleId = Guid.NewGuid().ToString();
            Article article = new Article
            {
                Id = articleId,
                Author = "Test",
                Content = "Test",
                Title = "Test",
                SourceUrl = "Test",
                CreatorId = Guid.NewGuid().ToString(),
                IsDeleted = false,
                DeletedOn = null,
            };

            await this.dbContext.Articles.AddAsync(article);
            await this.dbContext.SaveChangesAsync();

            // Act
            Task result = this.articleService.SoftDeleteAsync(articleId);
            await result;

            // Assert
            Article? editedArticle = await this.dbContext.Articles.FirstOrDefaultAsync(a => a.Id == articleId);
            
            Assert.That(editedArticle, Is.Not.Null);
            Assert.That(editedArticle.IsDeleted, Is.True);
            Assert.That(editedArticle.DeletedOn, Is.Not.Null);
            Assert.That(editedArticle.DeletedOn, Is.TypeOf<DateTime>());
        }

        [Test]
        public async Task SoftDeleteAsync_IsThrowingArgumentException_WhenArticleDoesNotExist()
        {
            // Arrange
            Article article = new Article
            {
                Id = Guid.NewGuid().ToString(),
                Author = "Test",
                Content = "Test",
                Title = "Test",
                SourceUrl = "Test",
                CreatorId = Guid.NewGuid().ToString(),
                IsDeleted = false,
                DeletedOn = null,
            };

            await this.dbContext.Articles.AddAsync(article);
            await this.dbContext.SaveChangesAsync();

            // Act and Assert
            Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                var result = this.articleService.SoftDeleteAsync(Guid.NewGuid().ToString());
                await result;
            }, "Parameter tampering detected.");
        }

        [Test]
        public async Task DeleteAsync_IsDeletingArticleFromDatabase_WhenArticleExists()
        {
            // Arrange
            string articleId = Guid.NewGuid().ToString();
            Article article = new Article
            {
                Id = articleId,
                Author = "Test",
                Content = "Test",
                Title = "Test",
                SourceUrl = "Test",
                CreatorId = Guid.NewGuid().ToString(),
                IsDeleted = false,
                DeletedOn = null,
            };

            await this.dbContext.Articles.AddAsync(article);
            await this.dbContext.SaveChangesAsync();

            // Act
            Task result = this.articleService.DeleteAsync(articleId);
            await result;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(this.dbContext.Articles.Any(a => a.Id == articleId), Is.False);
        }

        [Test]
        public async Task DeleteAsync_IsThrowingArgumentException_WhenArticleDoesNotExist()
        {
            // Arrange
            Article article = new Article
            {
                Id = Guid.NewGuid().ToString(),
                Author = "Test",
                Content = "Test",
                Title = "Test",
                SourceUrl = "Test",
                CreatorId = Guid.NewGuid().ToString(),
                IsDeleted = false,
                DeletedOn = null,
            };

            await this.dbContext.Articles.AddAsync(article);
            await this.dbContext.SaveChangesAsync();

            // Act and Assert
            Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                Task result = this.articleService.DeleteAsync(Guid.NewGuid().ToString());
                await result;
            }, "Parameter tampering detected.");
        }

        [Test]
        public async Task UndoDelete_IsRestoringDefaultValusOfIsDeletedAndDeletedOnProperties_WhenArticleExists()
        {
            // Arrange
            string articleId = Guid.NewGuid().ToString();
            Article article = new Article
            {
                Id = articleId,
                Title = "Test",
                Content = "Test",
                Author = "Test",
                SourceUrl = "Test",
                IsDeleted = true,
                DeletedOn = DateTime.UtcNow,
                CreatorId = Guid.NewGuid().ToString(),
            };

            await this.dbContext.Articles.AddAsync(article);
            await this.dbContext.SaveChangesAsync();

            // Act
            Task result = this.articleService.UndoDeleteAsync(articleId);
            await result;

            // Assert
            Article? restoredArticle = await this.dbContext.Articles.FirstOrDefaultAsync(a => a.Id == articleId);

            Assert.That(restoredArticle, Is.Not.Null);
            Assert.That(restoredArticle.IsDeleted, Is.False);
            Assert.That(restoredArticle.DeletedOn, Is.Null);
        }

        [Test]
        public void UndoDelete_IsThrowingArgumentException_WhenArticleDoesNotExist()
        {
            // Act and Assert
            Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                Task result = this.articleService.UndoDeleteAsync(Guid.NewGuid().ToString());
                await result;
            }, "Parameter tampering detected.");
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
