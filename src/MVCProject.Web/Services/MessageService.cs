using Microsoft.EntityFrameworkCore;
using MigraineDiary.Web.Data;
using MigraineDiary.Web.Data.DbModels;
using MigraineDiary.Web.Models;
using MigraineDiary.Web.Services.Contracts;
using System.Drawing.Printing;

namespace MigraineDiary.Web.Services
{
    public class MessageService : IMessageService
    {
        private readonly ApplicationDbContext dbContext;

        public MessageService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task AddAsync(MessageAddModel addModel)
        {
            Message message = new Message
            {
                SenderName = addModel.SenderName,
                SenderEmail = addModel.SenderEmail,
                Title = addModel.Title,
                MessageContent = addModel.MessageContent,
            };

            await this.dbContext.Messages.AddAsync(message);
            await this.dbContext.SaveChangesAsync();
        }

        public async Task<PaginatedList<MessageViewModel>> GetAllAsync(int pageIndex = 1, int pageSize = 1, string orderByDate = "NewestFirst")
        {
            IQueryable<MessageViewModel> messages = null!;

            if (orderByDate == "NewestFirst")
            {
                messages = this.dbContext.Messages
                                         .OrderByDescending(m => m.CreatedOn)
                                         .Select(m => new MessageViewModel
                                         {
                                             SenderName = m.SenderName,
                                             SenderEmail = m.SenderEmail,
                                             Title = m.Title,
                                             MessageContent = m.MessageContent,
                                             Timestamp = m.CreatedOn
                                         })
                                         .AsNoTracking();
            }
            else
            {
                messages = this.dbContext.Messages
                                         .OrderBy(m => m.CreatedOn)
                                         .Select(m => new MessageViewModel
                                         {
                                             SenderName = m.SenderName,
                                             SenderEmail = m.SenderEmail,
                                             Title = m.Title,
                                             MessageContent = m.MessageContent,
                                             Timestamp = m.CreatedOn
                                         })
                                         .AsNoTracking();
            }

            return await PaginatedList<MessageViewModel>.CreateAsync(messages, pageIndex, pageSize);
        }

        public Task SoftDeleteAsync()
        {
            throw new NotImplementedException();
        }
    }
}
