using MigraineDiary.ViewModels;

namespace MigraineDiary.Services.Contracts
{
    public interface IClinicalTrialService
    {
        public Task AddAsync(ClinicalTrialAddModel addModel, string uploadedFileName, string uploaderID);

        public string GenerateUniqueFileName(string fileExtension);

        public Task<PaginatedList<ClinicalTrialViewModel>> GetAllTrialsAsync(int pageIndex, int pageSize, string orderByDate);

        public Task<PaginatedList<ClinicalTrialViewModel>> GetAllTrialsAsync(int pageIndex, int pageSize, string orderByDate, string creatorId);

        public Task<ClinicalTrialEditModel> GetByIdAsync(string trialId, string creatorId);

        public Task EditTrialAsync(ClinicalTrialEditModel editmodel);

        public Task SoftDeleteAsync(string trialId, string creatorId);
    }
}
