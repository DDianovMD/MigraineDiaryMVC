using System.ComponentModel.DataAnnotations;

namespace MigraineDiary.ViewModels
{
    public class MedicationAddFormModel
    {
        [Required]
        public string Name { get; set; } = null!;

        [Required]
        [Range(0, 2000, ConvertValueInInvariantCulture = true)]
        public decimal SinglePillDosage { get; set; }

        [Required]
        public string Units { get; set; } = null!;

        [Required]
        [Range(0, 10, ConvertValueInInvariantCulture = true)]
        public decimal NumberOfTakenPills { get; set; }
    }
}
