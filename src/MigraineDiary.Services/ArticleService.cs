using Microsoft.EntityFrameworkCore;
using MigraineDiary.Data;
using MigraineDiary.Data.DbModels;
using MigraineDiary.Services.Contracts;
using MigraineDiary.ViewModels;

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
                                         .Where(a => a.IsDeleted == false)
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
                                         .Where(a => a.IsDeleted == false)
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

        public async Task<ArticleEditModel> GetByIdAsync(string articleId)
        {
            ArticleEditModel? articleEditModel = await this.dbContext.Articles
                                                                     .Where(a => a.IsDeleted == false)
                                                                     .Select(a => new ArticleEditModel
                                                                     {
                                                                         Id = a.Id,
                                                                         Author = a.Author,
                                                                         Title = a.Title,
                                                                         Content = a.Content,
                                                                         SourceUrl = a.SourceUrl,
                                                                     })
                                                                     .FirstOrDefaultAsync(a => a.Id == articleId);

            if (articleEditModel != null)
            {
                return articleEditModel;
            }
            else
            {
                throw new ArgumentException("Parameter tampering detected.", articleId);
            }
        }

        public async Task<PaginatedList<ArticleViewModel>> GetArchivedArticles(int pageIndex, int pageSize, string orderByDate)
        {
            IQueryable<ArticleViewModel> articles = null!;

            if (orderByDate == "NewestFirst")
            {
                articles = this.dbContext.Articles
                                         .Where(a => a.IsDeleted == true)
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
                                         .Where(a => a.IsDeleted == true)
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

        public async Task EditAsync(ArticleEditModel editedArticle)
        {
            // Get article from database.
            Article? article = await this.dbContext.Articles
                                                   .Where(a => a.IsDeleted == false)
                                                   .FirstOrDefaultAsync(a => a.Id == editedArticle.Id);

            // Check if article exists.
            // If article is null that means Id is tampered.
            if (article != null)
            {
                // Assign new values.
                article.Author = editedArticle.Author;
                article.Title = editedArticle.Title;
                article.Content = editedArticle.Content;
                article.SourceUrl = editedArticle.SourceUrl;

                // Save changes in database.
                await this.dbContext.SaveChangesAsync();
            }
            else
            {
                throw new ArgumentException("Parameter tampering detected", editedArticle.Id);
            }
        }

        public async Task SoftDeleteAsync(string articleId)
        {
            // Retrieve article from database.
            Article? article = await this.dbContext.Articles.FirstOrDefaultAsync(a => a.Id == articleId);

            // Check if article exists.
            // If article is null that means articleId is tampered.
            if (article != null)
            {
                // Set IsDeleted and DeletedOn properties.
                article.IsDeleted = true;
                article.DeletedOn = DateTime.UtcNow;

                // Save changes in database.
                await this.dbContext.SaveChangesAsync();
            }
            else
            {
                throw new ArgumentException("Parameter tampering detected.", articleId);
            }
        }

        public async Task DeleteAsync(string articleId)
        {
            // Retrieve article from database.
            Article? article = await this.dbContext.Articles.FirstOrDefaultAsync(a => a.Id == articleId);

            // Check if article exists.
            // If article is null that means articleId is tampered.
            if (article != null)
            {
                // Delete article from database.
                this.dbContext.Articles.Remove(article);

                // Save changes in database.
                await this.dbContext.SaveChangesAsync();
            }
            else
            {
                throw new ArgumentException("Parameter tampering detected.", articleId);
            }
        }

        public async Task UndoDeleteAsync(string articleId)
        {
            // Retrieve article from database.
            Article? article = await this.dbContext.Articles.FirstOrDefaultAsync(a => a.Id == articleId);

            // Check if article exists.
            // If article is null that means articleId is tampered.
            if (article != null)
            {
                // Revert soft delete.
                article.IsDeleted = false;
                article.DeletedOn = null;

                // Save changes in database.
                await this.dbContext.SaveChangesAsync();
            }
            else
            {
                throw new ArgumentException("Parameter tampering detected.", articleId);
            }
        }
    }
}
