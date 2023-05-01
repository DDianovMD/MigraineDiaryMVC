using System.ComponentModel.DataAnnotations;

namespace MigraineDiary.Web.Models
{
    public class MessageViewModel
    {
        public string Id { get; set; } = null!;

        [Display(Name = "Подател")]
        public string SenderName { get; set; } = null!;

        [Display(Name = "Имейл за обратна връзка.")]
        public string SenderEmail { get; set; } = null!;

        [Display(Name = "Тема")]
        public string Title { get; set; } = null!;

        [Display(Name = "Съобщение")]
        public string MessageContent { get; set; } = null!;

        [Display(Name = "Дата на изпращане")]
        public DateTime Timestamp { get; set; }
    }
}
