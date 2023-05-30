using MigraineDiary.ViewModels;

namespace MigraineDiary.Services.Contracts
{
    public interface IHeadacheService
    {
        public Dictionary<string, int> CalculateDuration(DateTime onset, DateTime endtime);

        public Task<PaginatedList<RegisteredHeadacheViewModel>> GetRegisteredHeadachesAsync(string userId, int pageIndex, int pageSize, string orderByDate);

        public Task<DoctorViewModel[]> GetDoctorUsersByNameAsync(string name);
    }
}
