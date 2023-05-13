using MigraineDiary.Web.Models;

namespace MigraineDiary.Web.Services.Contracts
{
    public interface IClinicalTrialService
    {
        public Task AddAsync(ClinicalTrialAddModel addModel, string uploadedFileName, string uploaderID);

        public string GenerateUniqueFileName(string fileExtension);

        public Task<PaginatedList<ClinicalTrialViewModel>> GetAllTrials(int pageIndex, int pageSize, string orderByDate);

        public Task<PaginatedList<ClinicalTrialViewModel>> GetAllTrials(int pageIndex, int pageSize, string orderByDate, string creatorId);
    }
}
