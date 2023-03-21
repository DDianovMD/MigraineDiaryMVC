using MigraineDiary.Web.Services.Contracts;

namespace MigraineDiary.Web.Services
{
    public class MedicationService : IMedicationService
    {
        public decimal CalculateWholeTakenDosage(decimal singlePillDosage, decimal numberOfTakenPills)
        {
            return singlePillDosage * numberOfTakenPills;
        }
    }
}
