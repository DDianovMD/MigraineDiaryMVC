using MigraineDiary.ViewModels;

namespace MigraineDiary.Services.Contracts
{
    public interface IHeadacheService
    {
        public Dictionary<string, int> CalculateDuration(DateTime onset, DateTime endtime);

        public Task AddAsync(HeadacheAddFormModel addModel, Dictionary<string, int> headacheDuration, string currentUserId);

        public Task<PaginatedList<RegisteredHeadacheViewModel>> GetRegisteredHeadachesAsync(string userId, int pageIndex, int pageSize, string orderByDate);

        public Task<PaginatedList<SharedHeadacheViewModel>> GetSharedHeadachesAsync(string doctorId, string patientId, int pageIndex, int pageSize, string orderByDate);

        public Task<DoctorViewModel[]> GetDoctorUsersByNameAsync(string name);

        public Task Share(string headacheId, string doctorID);
    }
}
