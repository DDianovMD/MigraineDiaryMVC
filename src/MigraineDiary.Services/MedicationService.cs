using MigraineDiary.Services.Contracts;

namespace MigraineDiary.Services
{
    public class MedicationService : IMedicationService
    {
        public decimal CalculateWholeTakenDosage(decimal singlePillDosage, decimal numberOfTakenPills)
        {
            return singlePillDosage * numberOfTakenPills;
        }
    }
}
