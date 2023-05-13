namespace MigraineDiary.Services.Contracts
{
    public interface IMedicationService
    {
        public decimal CalculateWholeTakenDosage(decimal singlePillDosage, decimal numberOfTakenPills);
    }
}
