﻿using MigraineDiary.Web.Models;

namespace MigraineDiary.Web.Services.Contracts
{
    public interface IMessageService
    {
        public Task AddAsync(MessageAddModel addModel);

        public Task<PaginatedList<MessageViewModel>> GetAllAsync(int pageIndex, int pageSize, string orderByDate);

        public Task SoftDeleteAsync(string messageId);

        public Task DeleteAsync(string messageId);
    }
}
