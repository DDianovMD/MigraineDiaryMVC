using System.ComponentModel.DataAnnotations;

namespace MigraineDiary.Web.Models
{
    public class ClinicalTrialAddModel
    {
        public ClinicalTrialAddModel()
        {
            this.Practicioners = new List<PracticionerAddModel>();
        }

        /// <summary>
        /// City where clinical trial is performed.
        /// </summary>
        [Required(ErrorMessage = "Необходимо е да посочите град.")]
        [MinLength(4, ErrorMessage = "Полето не може да съдържа по-малко от 4 символа.")]
        [MaxLength(20, ErrorMessage = "Полето не може да съдържа повече от 20 символа.")]
        [Display(Name = "Град")]
        public string City { get; set; } = null!;

        /// <summary>
        /// Hospital in which clinical trial is performed.
        /// </summary>
        [Required(ErrorMessage = "Необходимо е да посочите болница.")]
        [MinLength(5, ErrorMessage = "Полето не може да съдържа по-малко от 5 символа.")]
        [MaxLength(50, ErrorMessage = "Полето не може да съдържа повече от 50 символа.")]
        [Display(Name = "Болница")]
        public string Hospital { get; set; } = null!;

        /// <summary>
        /// Document with information about the trial and informed consent.
        /// </summary>
        [Required]
        public IFormFile TrialAgreementDocument { get; set; } = null!;

        /// <summary>
        /// One to many relationship with Practicioner entity.
        /// </summary>
        [Required]
        public ICollection<PracticionerAddModel> Practicioners { get; set; }
    }
}
