using Microsoft.EntityFrameworkCore;
using MigraineDiary.Data;
using MigraineDiary.Data.DbModels;
using MigraineDiary.ViewModels;
using MigraineDiary.Services.Contracts;

namespace MigraineDiary.Services
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
                                         .Where(m => m.IsDeleted == false)
                                         .OrderByDescending(m => m.CreatedOn)
                                         .Select(m => new MessageViewModel
                                         {
                                             Id = m.Id,
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
                                         .Where(m => m.IsDeleted == false)
                                         .OrderBy(m => m.CreatedOn)
                                         .Select(m => new MessageViewModel
                                         {
                                             Id = m.Id,
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

        public async Task SoftDeleteAsync(string messageId)
        {
            Message? message = await this.dbContext.Messages
                                                   .FirstOrDefaultAsync(m => m.Id == messageId);

            try
            {
                message!.IsDeleted = true;
                message.DeletedOn = DateTime.UtcNow;

                await this.dbContext.SaveChangesAsync();
            }
            catch (NullReferenceException nre)
            {
                throw new NullReferenceException($"Подаденото Id ({messageId}) на съобщението не съществува.", nre);
            }
        }

        public async Task DeleteAsync(string messageId)
        {
            Message? message = await this.dbContext.Messages
                                                   .FirstOrDefaultAsync(m => m.Id == messageId);

            try
            {
                this.dbContext.Messages
                              .Remove(message!);

                await this.dbContext.SaveChangesAsync();
            }
            catch (ArgumentNullException nre)
            {
                throw new ArgumentNullException($"Подаденото Id ({messageId}) на съобщението не съществува.", nre);
            }
        }

        public async Task<int> GetMessagesCountAsync()
        {
            return await this.dbContext.Messages.Where(m => m.IsDeleted == false).CountAsync();
        }
    }
}
