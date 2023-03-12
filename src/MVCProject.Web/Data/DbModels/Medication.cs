using MigraineDiary.Web.Data.Common.Contracts;
using System.ComponentModel.DataAnnotations;

namespace MigraineDiary.Web.Data.DbModels
{
    public class Medication : ISoftDeletable, IAuditable
    {
        public Medication()
        {
            this.Id = new Guid().ToString();
            this.Headaches = new List<Headache>();
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
        public string Name { get; set; }

        /// <summary>
        /// Medication generic name. This property can be set only by administrator.
        /// </summary>
        [MaxLength(50)]
        public string GenericName { get; set; }

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
        public string Units { get; set; }

        /// <summary>
        /// How many pills did the patient take while in pain.
        /// </summary>
        [Required]
        public decimal NumberOfTakenPills { get; set; } // 1 / 1,25 / 1,5 еtc...

        /// <summary>
        /// Many to many relation with headaches.
        /// </summary>
        public ICollection<Headache> Headaches { get; set; }

        /// <summary>
        /// Boolean flag indicating if the entity is soft deleted.
        /// </summary>
        public bool IsDeleted { get; set; } = false;

        /// <summary>
        /// When was the entity created?
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// When was the entity deleted?
        /// </summary>
        public DateTime? DeletedOn { get; set; }
    }
}
