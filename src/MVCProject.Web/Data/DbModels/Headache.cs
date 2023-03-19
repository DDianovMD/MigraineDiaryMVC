using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MigraineDiary.Web.Data.DbModels
{
    public class Headache
    {
        public Headache()
        {
            this.Id = Guid.NewGuid().ToString();
            this.SharedWith = new List<ApplicationUser>();
            this.MedicationsTaken = new List<Medication>();    
        }

        /// <summary>
        /// Primary key.
        /// </summary>
        [Key]
        public string Id { get; set; }

        /// <summary>
        /// When did the headache start?
        /// </summary>
        [Required]
        public DateTime Onset { get; set; }

        /// <summary>
        /// When did the headache end?
        /// </summary>
        [Required]
        public DateTime EndTime { get; set; }

        /// <summary>
        /// Headache days duration.
        /// </summary>
        [Required]
        public int DurationDays { get; set; }

        /// <summary>
        /// Headache hours duration.
        /// </summary>
        [Required]
        public int DurationHours { get; set; }

        /// <summary>
        /// Headache minutes duration.
        /// </summary>
        [Required]
        public int DurationMinutes { get; set; }


        /// <summary>
        /// Where was the pain localized - frontal, occipital, parietal, unilateral, bilateral?
        /// </summary>
        [Required]
        [MaxLength(300)]
        public string LocalizationSide { get; set; } = null!;

        /// <summary>
        /// Pain severity (0-10) where 0 means no pain at all and 10 means the worst pain that can be imagined.
        /// </summary>
        [Required]
        [Range(0, 10)]
        public int Severity { get; set; }

        /// <summary>
        /// Characteristics of the pain - sharp, dull, pulsating, etc...
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string PainCharacteristics { get; set; } = null!;

        /// <summary>
        /// Did the light make the pain more severe?
        /// </summary>
        [Required]
        public bool Photophoby { get; set; }

        /// <summary>
        /// Did the sound make the pain more severe?
        /// </summary>
        [Required]
        public bool Phonophoby { get; set; }

        /// <summary>
        /// Did you felt nausea?
        /// </summary>
        [Required]
        public bool Nausea { get; set; }

        /// <summary>
        /// Did you vomit?
        /// </summary>
        [Required]
        public bool Vomiting { get; set; }

        /// <summary>
        /// Did you had aura?
        /// </summary>
        [Required]
        public bool Aura { get; set; }

        /// <summary>
        /// If you had aura what it felt like?
        /// </summary>
        [MaxLength(350)]
        public string? AuraDescriptionNotes { get; set; }

        /// <summary>
        /// Are there any triggers which provoke migraine headache?
        /// </summary>
        public string? Triggers { get; set; }

        /// <summary>
        /// One to one relation with patient (ApplicationUser in role "Patient")
        /// </summary>
        public string? PatientId { get; set; }

        /// <summary>
        /// Navigational property.
        /// </summary>
        [ForeignKey(nameof(PatientId))]
        public ApplicationUser Patient { get; set; }

        /// <summary>
        /// Many to many relation with doctors (ApplicationUsers in role "Doctor")
        /// </summary>
        public ICollection<ApplicationUser> SharedWith { get; set; }

        /// <summary>
        /// Many to many relation with medications.
        /// </summary>
        public ICollection<Medication> MedicationsTaken { get; set; }
    }
}
