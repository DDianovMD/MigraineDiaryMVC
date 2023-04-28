using MigraineDiary.Web.Models;

namespace MigraineDiary.Web.Services.Contracts
{
    public interface IMessageService
    {
        public Task AddAsync(MessageAddModel addModel);

        public Task GetAllAsync();

        public Task SoftDeleteAsync();
    }
}
