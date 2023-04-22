using MigraineDiary.Web.Data.DbModels;
using MigraineDiary.Web.Models;

namespace MigraineDiary.Web.Services.Contracts
{
    public interface IArticleService
    {
        public Task CreateArticle(ArticleAddFormModel model, string userId);
        public Task<ArticleViewModel[]> GetArticles();
    }
}
