using MigraineDiary.Web.Models;

namespace MigraineDiary.Web.Services.Contracts
{
    public interface IArticleService
    {
        public Task CreateArticle(ArticleAddFormModel model, string userId);
    }
}
