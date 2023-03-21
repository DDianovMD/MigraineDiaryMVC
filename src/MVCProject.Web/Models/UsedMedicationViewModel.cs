using System.ComponentModel.DataAnnotations;

namespace MigraineDiary.Web.Models
{
    public class UsedMedicationViewModel
    {
        public string Name { get; set; }
        
        public decimal SinglePillDosage { get; set; }

        public string Units { get; set; }

        public decimal NumberOfTakenPills { get; set; }

        public decimal DosageTaken { get; set; }
    }
}

