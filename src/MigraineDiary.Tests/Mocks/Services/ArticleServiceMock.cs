using MigraineDiary.Services.Contracts;
using MigraineDiary.ViewModels;
using Moq;

namespace MigraineDiary.Tests.Mocks.Services
{
    public static class ArticleServiceMock
    {
        public static async Task<IArticleService> Instance()
        {
            var articleServiceMock = new Mock<IArticleService>();

            articleServiceMock.Setup(x => x.GetArticles(1, 5, "NewestFirst"))
                .ReturnsAsync(PaginatedList<ArticleViewModel>.CreateAsync(new List<ArticleViewModel>()
                {
                    new ArticleViewModel()
                    {
                        Author = "Test",
                        Content = "Test",
                        CreatedOn = DateTime.Now,
                        SourceUrl = "https://www.test.com",
                        Title = "Test"
                    },

                    new ArticleViewModel()
                    {
                        Author = "Test2",
                        Content = "Test2",
                        CreatedOn = DateTime.Now,
                        SourceUrl = "https://www.test2.com",
                        Title = "Test2"
                    }
                }, 1, 5));

            return articleServiceMock.Object;
        }
    }
}
