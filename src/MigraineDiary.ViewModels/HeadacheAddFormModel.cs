using System.ComponentModel.DataAnnotations;

namespace MigraineDiary.ViewModels
{
    public class HeadacheAddFormModel
    {
        public HeadacheAddFormModel()
        {
            this.MedicationsTaken = new List<MedicationAddFormModel>();
        }

        [Required(ErrorMessage = "Необходимо е да въведете начало на главоболието.")]
        public DateTime Onset { get; set; }

        [Required(ErrorMessage = "Необходимо е да въведете край на главоболието.")]
        public DateTime EndTime { get; set; }

        [Required(ErrorMessage = "Необходимо е да посочите в коя част на главата изпитвахте болка.")]
        [MaxLength(300, ErrorMessage = "Описанието на локализацията не може да надвишава 300 символа.")]
        public string LocalizationSide { get; set; } = null!;

        [Required]
        public int Severity { get; set; }

        [Required(ErrorMessage = "Необходимо е да опишете характеристиката на болката.")]
        [MaxLength(100, ErrorMessage = "Описанието на характеристиката не може да надвишава 100 символа.")]
        public string PainCharacteristics { get; set; } = null!;

        [Required(ErrorMessage = "Полето е задължително.")]
        public bool Photophoby { get; set; }

        [Required(ErrorMessage = "Полето е задължително.")]
        public bool Phonophoby { get; set; }

        [Required(ErrorMessage = "Полето е задължително.")]
        public bool Nausea { get; set; }

        [Required(ErrorMessage = "Полето е задължително.")]
        public bool Vomiting { get; set; }

        [Required(ErrorMessage = "Полето е задължително.")]
        public bool Aura { get; set; }

        [MaxLength(350, ErrorMessage = "Описанието на аурата не може да бъде повече от 350 символа.")]
        public string? AuraDescriptionNotes { get; set; }

        [MaxLength(350, ErrorMessage = "Описанието на провокиращите фактори не може да бъде повече от 350 символа.")]
        public string? Triggers { get; set; }

        public List<MedicationAddFormModel> MedicationsTaken { get; set; }
    }
}
