using MigraineDiary.Data.Common.Contracts;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MigraineDiary.Data.DbModels
{
    public class Practicioner : IAuditable, ISoftDeletable
    {
        public Practicioner()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Practicioner's primary key.
        /// </summary>
        [Key]
        public string Id { get; set; } = null!;

        /// <summary>
        /// Practicioner's rank.
        /// </summary>
        [Required]
        public string Rank { get; set; } = null!;

        /// <summary>
        /// Practicioner's first name.
        /// </summary>
        [Required]
        public string FirstName { get; set; } = null!;

        /// <summary>
        /// Practicioner's last name.
        /// </summary>
        [Required]
        public string Lastname { get; set; } = null!;

        /// <summary>
        /// Practicioner's science degree.
        /// </summary>
        [MaxLength(30)]
        public string? ScienceDegree { get; set; }

        /// <summary>
        /// One to many relation with clinical trial in which practicioner participate.
        /// </summary>
        [Required]
        public string ClinicalTrialId { get; set;} = null!;

        /// <summary>
        /// Navigational property.
        /// </summary>
        [ForeignKey(nameof(ClinicalTrialId))]
        public ClinicalTrial ClinicalTrial { get; set; } = null!;

        /// <summary>
        /// Date when entity is created.
        /// </summary>
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Boolean flag showing if entity is soft deleted.
        /// </summary>
        public bool IsDeleted { get; set; } = false;

        /// <summary>
        /// Date when entity is marked as deleted (soft delete).
        /// </summary>
        public DateTime? DeletedOn { get; set; }
    }
}
