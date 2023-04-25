using MigraineDiary.Web.Models;

namespace MigraineDiary.Web.Services.Contracts
{
    public interface IArticleService
    {
        public Task CreateArticle(ArticleAddFormModel model, string userId);
        public Task<PaginatedList<ArticleViewModel>> GetArticles(int pageIndex, int pageSize, string orderByDate);
    }
}
