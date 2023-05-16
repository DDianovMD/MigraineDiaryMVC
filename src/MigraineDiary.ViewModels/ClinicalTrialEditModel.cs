using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace MigraineDiary.ViewModels
{
    public class ClinicalTrialEditModel
    {
        /// <summary>
        /// Clinical trial's Id.
        /// </summary>
        public string Id { get; set; } = null!;

        /// <summary>
        /// Clinical trial heading.
        /// </summary>
        [Required(ErrorMessage = "Необходимо е да въведете предмет на изследването.")]
        [Display(Name = "Предмет на изследването")]
        public string Heading { get; set; } = null!;

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
        [Display(Name = "Лечебно заведение")]
        public string Hospital { get; set; } = null!;

        /// <summary>
        /// Unique document name, generated when trial is added.
        /// </summary>
        public string AgreementDocumentName { get; set; } = null!;

        /// <summary>
        /// Document with information about the trial and informed consent.
        /// </summary>
        public IFormFile? TrialAgreementDocument { get; set; }

        /// <summary>
        /// Practicioners involved in research process.
        /// </summary>
        [Required]
        public ICollection<PracticionerEditModel> Practicioners { get; set; } = new List<PracticionerEditModel>();
    }
}
