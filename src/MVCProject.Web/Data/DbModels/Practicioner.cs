using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MigraineDiary.Web.Data.DbModels
{
    public class Practicioner
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
    }
}
