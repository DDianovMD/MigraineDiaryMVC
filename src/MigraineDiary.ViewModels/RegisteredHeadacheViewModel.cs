using System.ComponentModel.DataAnnotations;

namespace MigraineDiary.ViewModels
{
    public class RegisteredHeadacheViewModel
    {
        public RegisteredHeadacheViewModel()
        {
            this.UsedMedications = new HashSet<UsedMedicationViewModel>();
        }

        public string Id { get; set; }

        [Display(Name = "Начало")]
        public DateTime Onset { get; set; }

        [Display(Name = "Край")]
        public DateTime EndTime { get; set; }

        [Display(Name = "Дни")]
        public int DurationDays { get; set; }

        [Display(Name = "Часове")]
        public int DurationHours { get; set; }

        [Display(Name = "Минути")]
        public int DurationMinutes { get; set; }

        [Display(Name = "Локализация")]
        public string LocalizationSide { get; set; } = null!;

        [Display(Name = "Сила по десетобалната система")]
        public int Severity { get; set; }

        [Display(Name = "Характеристика на болката")]
        public string PainCharacteristics { get; set; } = null!;

        [Display(Name = "Фотофобия")]
        public bool Photophoby { get; set; }

        [Display(Name = "Фонофобия")]
        public bool Phonophoby { get; set; }

        [Display(Name = "Гадене")]
        public bool Nausea { get; set; }

        [Display(Name = "Повръщане")]
        public bool Vomiting { get; set; }

        [Display(Name = "Наличие на аура")]
        public bool Aura { get; set; }

        [Display(Name = "Описание на аурата")]
        public string? AuraDescriptionNotes { get; set; }

        [Display(Name = "Провокиращи фактори")]
        public string? Triggers { get; set; }

        public IEnumerable<UsedMedicationViewModel> UsedMedications { get; set; }
    }
}
