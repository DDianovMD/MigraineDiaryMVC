using System.ComponentModel.DataAnnotations;

namespace MigraineDiary.ViewModels
{
    public class ArticleEditModel
    {
        [Required]
        public string Id { get; set; } = null!;

        /// <summary>
        /// Article's title.
        /// </summary>
        [Required(ErrorMessage = "Необходимо е да въведете заглавие.")]
        [MaxLength(100)]
        public string Title { get; set; } = null!;

        /// <summary>
        /// Article's content
        /// </summary>
        [Required(ErrorMessage = "Необходимо е да въведете съдържание.")]
        public string Content { get; set; } = null!;

        [Required(ErrorMessage = "Необходимо е да посочите автор(и).")]
        [MaxLength(200)]
        public string Author { get; set; } = null!;

        /// <summary>
        /// URL leading to origical source of information.
        /// </summary>
        [Url(ErrorMessage = "Въведете валиден URL адрес.")]
        public string? SourceUrl { get; set; }
    }
}
