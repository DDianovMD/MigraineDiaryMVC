using MigraineDiary.ViewModels;

namespace MigraineDiary.Services.Contracts
{
    public interface IArticleService
    {
        public Task CreateArticle(ArticleAddFormModel model, string userId);

        public Task<PaginatedList<ArticleViewModel>> GetArticles(int pageIndex, int pageSize, string orderByDate);

        public Task<ArticleEditModel> GetByIdAsync(string articleId);

        public Task EditAsync(ArticleEditModel editedArticle);

        public Task SoftDeleteAsync(string articleId);

        public Task DeleteAsync(string articleId);
    }
}
