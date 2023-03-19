using MigraineDiary.Web.Data.DbModels;
using MigraineDiary.Web.Models;

namespace MigraineDiary.Web.Services.Contracts
{
    public interface IMedicationService
    {
        public Task<ICollection<UsedMedicationViewModel>> GetUsedMedicationsAsync(string userId);
    }
}
