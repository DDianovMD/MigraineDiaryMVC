using MigraineDiary.Data.Common.Contracts;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MigraineDiary.Data.DbModels
{
    public class Medication : ISoftDeletable, IAuditable
    {
        public Medication()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Primary key.
        /// </summary>
        [Key]
        public string Id { get; set; }

        /// <summary>
        /// Medication's trade name.
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = null!;

        /// <summary>
        /// Medication generic name. This property can be set only by administrator.
        /// </summary>
        [MaxLength(50)]
        public string? GenericName { get; set; }

        /// <summary>
        /// Medication dosage in a single pill.
        /// </summary>
        [Required]
        [Range(0, 2000, ConvertValueInInvariantCulture = true)]
        public decimal SinglePillDosage { get; set; }

        /// <summary>
        /// Units for measurement - mcg, mg, g, ml.
        /// </summary>
        [Required]
        [MaxLength(5)]
        public string Units { get; set; } = null!;

        /// <summary>
        /// How many pills did the patient take while in pain.
        /// </summary>
        [Required]
        public decimal NumberOfTakenPills { get; set; } // 1 / 1,25 / 1,5 еtc...

        /// <summary>
        /// One to many relation with headaches.
        /// </summary>
        [Required]
        public string HeadacheId { get; set; } = null!;

        /// <summary>
        /// Navigational property.
        /// </summary>
        [Required]
        [ForeignKey(nameof(HeadacheId))]
        public Headache Headache { get; set; } = null!;

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
