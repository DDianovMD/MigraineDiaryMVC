using System.ComponentModel.DataAnnotations;

namespace MigraineDiary.ViewModels
{
    public class MedicationAddFormModel
    {
        [Required(ErrorMessage = "Необходимо е да въведете име на медикамента.")]
        public string Name { get; set; } = null!;

        [Required]
        [Range(1, 2000, ConvertValueInInvariantCulture = true, ErrorMessage = "Дозата не може да надвишава 2000 мерни единици (1000 mg = 1 g).")]
        public decimal SinglePillDosage { get; set; }

        [Required]
        public string Units { get; set; } = null!;

        [Required]
        [Range(1, 10, ConvertValueInInvariantCulture = true, ErrorMessage = "Броят таблетки не може да бъде по-малък от 1 и повече от 10.")]
        public decimal NumberOfTakenPills { get; set; }
    }
}
