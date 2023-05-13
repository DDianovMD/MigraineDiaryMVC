using MigraineDiary.Data.Common.Contracts;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MigraineDiary.Data.DbModels
{
    public class HIT6Scale : ISoftDeletable
    {
        public HIT6Scale()
        {
            this.Id = Guid.NewGuid().ToString();
            this.SharedWith = new List<ApplicationUser>();
        }

        /// <summary>
        /// Primary key.
        /// </summary>
        [Key]
        public string Id { get; set; } = null!;

        /// <summary>
        /// First question's answer from HIT6 scale (Headache Impact Test).
        /// </summary>
        [Required]
        [StringLength(10)]
        public string FirstQuestionAnswer { get; set; } = null!;

        /// <summary>
        /// Second question's answer from HIT6 scale (Headache Impact Test).
        /// </summary>
        [Required]
        [StringLength(10)]
        public string SecondQuestionAnswer { get; set; } = null!;

        /// <summary>
        /// Third question's answer from HIT6 scale (Headache Impact Test).
        /// </summary>
        [Required]
        [StringLength(10)]
        public string ThirdQuestionAnswer { get; set; } = null!;

        /// <summary>
        /// Fourth question's answer from HIT6 scale (Headache Impact Test).
        /// </summary>
        [Required]
        [StringLength(10)]
        public string FourthQuestionAnswer { get; set; } = null!;

        /// <summary>
        /// Fifth question's answer from HIT6 scale (Headache Impact Test).
        /// </summary>
        [Required]
        [StringLength(10)]
        public string FifthQuestionAnswer { get; set; } = null!;

        /// <summary>
        /// Sixth question's answer from HIT6 scale (Headache Impact Test).
        /// </summary>
        [Required]
        [StringLength(10)]
        public string SixthQuestionAnswer { get; set; } = null!;

        /// <summary>
        /// Scored result according to patient's given answers.
        /// </summary>
        [Required]
        public int TotalScore { get; set; }

        /// <summary>
        /// One to one relation with patient (ApplicationUser in role "Patient")
        /// </summary>
        [Required]
        public string PatientId { get; set; } = null!;

        /// <summary>
        /// Navigational property.
        /// </summary>
        [Required]
        [ForeignKey(nameof(PatientId))]
        public ApplicationUser Patient { get; set; } = null!;

        /// <summary>
        /// Patient's registered HIT6 scales shared with user in role "Doctor".
        /// </summary>
        [Required]
        public ICollection<ApplicationUser> SharedWith { get; set; }

        /// <summary>
        /// Record showing when HIT6 scale is registered by user.
        /// </summary>
        [Required]
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Boolean flag showing if user have deleted registered HIT6 scale.
        /// </summary>
        [Required]
        public bool IsDeleted { get; set; } = false;

        /// <summary>
        /// Record showing when HIT6 scale is deleted by user.
        /// </summary>
        public DateTime? DeletedOn { get; set; }
    }
}
