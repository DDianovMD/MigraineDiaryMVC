using MigraineDiary.Web.Data.Common.Contracts;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MigraineDiary.Web.Data.DbModels
{
    public class ClinicalTrial : IAuditable, ISoftDeletable
    {
        public ClinicalTrial()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Practicioners = new List<Practicioner>();
        }

        /// <summary>
        /// Clinical trial's primary key.
        /// </summary>
        [Key]
        public string Id { get; set; } = null!;

        /// <summary>
        /// Trial's heading.
        /// </summary>
        [Required]
        public string Heading { get; set; } = null!;

        /// <summary>
        /// City where clinical trial is performed.
        /// </summary>
        [Required]
        public string City { get; set; } = null!;

        /// <summary>
        /// Hospital in which clinical trial is performed.
        /// </summary>
        [Required]
        public string Hospital { get; set; } = null!;

        /// <summary>
        /// Name of the document with information about the clinical trial.
        /// </summary>
        [Required]
        public string AgreementDocumentName { get; set; } = null!;

        /// <summary>
        /// One to many relationship with Practicioner entity.
        /// </summary>
        public ICollection<Practicioner> Practicioners { get; set; }

        /// <summary>
        /// One to many relation with ApplicationUser who created the clinical trial.
        /// </summary>
        public string CreatorId { get; set; }

        /// <summary>
        /// Navigational property.
        /// </summary>
        [ForeignKey(nameof(CreatorId))]
        public ApplicationUser Creator { get; set; } = null!;

        /// <summary>
        /// Boolean flag indicating if the entity is soft deleted.
        /// </summary>
        public bool IsDeleted { get; set; } = false;

        /// <summary>
        /// When was the entity created?
        /// </summary>
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// When was the entity deleted?
        /// </summary>
        public DateTime? DeletedOn { get; set; }
    }
}
