using MigraineDiary.Web.Data;
using MigraineDiary.Web.Data.DbModels;
using MigraineDiary.Web.Models;
using MigraineDiary.Web.Services.Contracts;

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

        public Task GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task SoftDeleteAsync()
        {
            throw new NotImplementedException();
        }
    }
}
