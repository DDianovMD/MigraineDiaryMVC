using MigraineDiary.Web.Models;

namespace MigraineDiary.Web.Services.Contracts
{
    public interface IZungScaleForAnxietyService
    {
        public Task AddAsync(ZungScaleAddModel addModel, string userId);

        public int CalculateTotalScore(string[] answers);

        public Task EditAsync(string scaleId, string userId, string[] editedAnswers);

        public Task<PaginatedList<ZungScaleViewModel>> GetAllAsync(string userId, int pageIndex, int pageSize, string orderByDate);

        public Task<ZungScaleViewModel> GetByIdAsync(string scaleId, string userId);

        public Task SoftDeleteAsync(string scaleId, string userId);

        public bool ValidateAnswer(string answer);
    }
}
