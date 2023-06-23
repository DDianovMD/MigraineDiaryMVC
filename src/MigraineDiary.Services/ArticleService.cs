using Microsoft.EntityFrameworkCore;
using MigraineDiary.Data;
using MigraineDiary.Data.DbModels;
using MigraineDiary.ViewModels;
using MigraineDiary.Services.Contracts;

namespace MigraineDiary.Services
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
                                         .OrderByDescending(a => a.CreatedOn)
                                         .Select(a => new ArticleViewModel
                                         {
                                             Id = a.Id,
                                             Title = a.Title,
                                             Content = a.Content,
                                             Author = a.Author,
                                             SourceUrl = a.SourceUrl,
                                             CreatedOn = a.CreatedOn
                                         })
                                         .AsNoTracking();
            }
            else
            {
                articles = this.dbContext.Articles
                                         .OrderBy(a => a.CreatedOn)
                                         .Select(a => new ArticleViewModel
                                         {
                                             Id = a.Id,
                                             Title = a.Title,
                                             Content = a.Content,
                                             Author = a.Author,
                                             SourceUrl = a.SourceUrl,
                                             CreatedOn = a.CreatedOn
                                         })
                                         .AsNoTracking();
            }

            return await PaginatedList<ArticleViewModel>.CreateAsync(articles, pageIndex, pageSize);
        }
    }
}
