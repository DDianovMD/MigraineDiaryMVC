namespace MigraineDiary.ViewModels
{
    public class UsedMedicationViewModel
    {
        public string Name { get; set; } = null!;
        
        public decimal SinglePillDosage { get; set; }

        public string Units { get; set; } = null!;

        public decimal NumberOfTakenPills { get; set; }

        public decimal DosageTaken { get; set; }
    }
}

