using MigraineDiary.Web.Data.Common.Contracts;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Policy;

namespace MigraineDiary.Web.Data.DbModels
{
    public class Article : IAuditable, ISoftDeletable
    {
        public Article()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Article's primary key.
        /// </summary>
        [Key]
        public string Id { get; set; }

        /// <summary>
        /// Article's title.
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string Title { get; set; } = null!;

        /// <summary>
        /// Article's content
        /// </summary>
        [Required]
        public string Content { get; set; } = null!;

        [Required]
        [MaxLength(200)]
        public string Author { get; set; } = null!;

        /// <summary>
        /// URL leading to origical source of information.
        /// </summary>
        [Url]
        public string? SourceUrl { get; set; }

        /// <summary>
        /// One to many relation with ApplicationUser who created the article.
        /// </summary>
        public string CreatorId { get; set; }

        /// <summary>
        /// Navigational property.
        /// </summary>
        [ForeignKey(nameof(CreatorId))]
        public ApplicationUser Creator { get; set; } = null!;

        /// <summary>
        /// Record showing when article is created by user.
        /// </summary>
        [Required]
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Boolean flag showing if user have deleted the article.
        /// </summary>
        [Required]
        public bool IsDeleted { get; set; } = false;

        /// <summary>
        /// Record showing when article is deleted by user.
        /// </summary>
        public DateTime? DeletedOn { get; set; }    }
}
