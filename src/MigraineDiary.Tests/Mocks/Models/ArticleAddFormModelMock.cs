using MigraineDiary.ViewModels;

namespace MigraineDiary.Tests.Mocks.Models
{
    public static class ArticleAddFormModelMock
    {
        public static ArticleAddFormModel Instance()
        {
            var articleAddFormModel = new ArticleAddFormModel()
            {
                Author = "Test",
                Content = "Test",
                SourceUrl = "https://www.test.com",
                Title = "Test"
            };

            return articleAddFormModel;
        }
    }
}
