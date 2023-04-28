using MigraineDiary.Web.Data.Common.Contracts;
using System.ComponentModel.DataAnnotations;

namespace MigraineDiary.Web.Data.DbModels
{
    public class Message : IAuditable, ISoftDeletable
    {
        public Message()
        {
            this.Id = Guid.NewGuid().ToString();
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

        [Required]
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        [Required]
        public bool IsDeleted { get; set; } = false;

        public DateTime? DeletedOn { get; set; }
    }
}
