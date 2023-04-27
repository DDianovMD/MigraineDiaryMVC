using MigraineDiary.Web.Models;

namespace MigraineDiary.Web.Services.Contracts
{
    public interface IHIT6ScaleService
    {
        public Task AddAsync(HIT6ScaleAddModel addModel, string userId);

        public int CalculateTotalScore(string[] answers);

        public Task EditAsync(string scaleId, string userId, string[] editedAnswers);

        public Task<HIT6ScaleViewModel> GetByIdAsync(string scaleId, string userId);

        public Task<PaginatedList<HIT6ScaleViewModel>> GetAllAsync(string userId, int pageIndex, int pageSize, string orderByDate);

        public Task SoftDeleteAsync(string scaleId, string userId);

        public bool ValidateAnswer(string answer);
    }
}
