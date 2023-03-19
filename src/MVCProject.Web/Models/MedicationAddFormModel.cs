using System.ComponentModel.DataAnnotations;

namespace MigraineDiary.Web.Models
{
    public class MedicationAddFormModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [Range(0, 2000, ConvertValueInInvariantCulture = true)]
        public decimal SinglePillDosage { get; set; }

        [Required]
        public string Units { get; set; }

        [Required]
        [Range(0, 10, ConvertValueInInvariantCulture = true)]
        public decimal NumberOfTakenPills { get; set; }
    }
}
