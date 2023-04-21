using MigraineDiary.Web.Data;
using MigraineDiary.Web.Data.DbModels;
using MigraineDiary.Web.Models;
using MigraineDiary.Web.Services.Contracts;

namespace MigraineDiary.Web.Services
{
    public class ArticleService : IArticleService
    {
        private readonly ApplicationDbContext dbContext;
        public ArticleService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task CreateArticle(ArticleAddFormModel model, string userId)
        {
            Article article = new Article()
            {
                Title = model.Title,
                Content = model.Content,
                Author = model.Author,
                SourceUrl = model.SourceUrl,
                CreatorId = userId,
            };

            await this.dbContext.Articles.AddAsync(article);
            await this.dbContext.SaveChangesAsync();
        }
    }
}
