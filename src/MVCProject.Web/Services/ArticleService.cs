using Microsoft.EntityFrameworkCore;
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

        public async Task<PaginatedList<ArticleViewModel>> GetArticles(int pageIndex, int pageSize, string orderByDate)
        {
            IQueryable<ArticleViewModel> articles = null!;

            if (orderByDate == "NewestFirst")
            {
                articles = this.dbContext.Articles
                                         .OrderByDescending(x => x.CreatedOn)
                                         .Select(x => new ArticleViewModel
                                         {
                                             Title = x.Title,
                                             Content = x.Content,
                                             Author = x.Author,
                                             SourceUrl = x.SourceUrl,
                                             CreatedOn = x.CreatedOn
                                         })
                                         .AsNoTracking();
            }
            else
            {
                articles = this.dbContext.Articles
                                         .OrderBy(x => x.CreatedOn)
                                         .Select(x => new ArticleViewModel
                                         {
                                             Title = x.Title,
                                             Content = x.Content,
                                             Author = x.Author,
                                             SourceUrl = x.SourceUrl,
                                             CreatedOn = x.CreatedOn
                                         })
                                         .AsNoTracking();
            }

            return await PaginatedList<ArticleViewModel>.CreateAsync(articles, pageIndex, pageSize);
        }
    }
}
