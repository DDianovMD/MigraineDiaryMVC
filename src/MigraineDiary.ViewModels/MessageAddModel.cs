using System.ComponentModel.DataAnnotations;

namespace MigraineDiary.ViewModels
{
    public class MessageAddModel
    {
        [Required(ErrorMessage = "Необходимо е да въведете име.")]
        [StringLength(60, MinimumLength = 3, ErrorMessage = "Името трябва да бъде между 3 и 60 символа.")]
        public string SenderName { get; set; } = null!;

        [Required(ErrorMessage = "Необходимо е да въведете имейл за връзка.")]
        [EmailAddress]
        public string SenderEmail { get; set; } = null!;

        [Required(ErrorMessage = "Необходимо е да посочите тема.")]
        [MaxLength(100)]
        public string Title { get; set; } = null!;

        [Required(ErrorMessage = "Необходимо е да въведете текст на съобщението.")]
        [MaxLength(1000, ErrorMessage = "Максимално допустимата дължина на съобщението е 1000 символа.")]
        public string MessageContent { get; set; } = null!;
    }
}
