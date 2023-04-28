using System.ComponentModel.DataAnnotations;

namespace MigraineDiary.Web.Data.DbModels
{
    public class Message
    {
        public Message()
        {
            this.Id = new Guid().ToString();
        }

        [Key]
        public string Id { get; set; }

        [Required]
        [MaxLength(60)]
        public string SenderName { get; set; } = null!;

        [Required]
        [EmailAddress]
        public string SenderEmail { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        public string Title { get; set; } = null!;

        [Required]
        [MaxLength(1000)]
        public string MessageContent { get; set; } = null!;
    }
}
