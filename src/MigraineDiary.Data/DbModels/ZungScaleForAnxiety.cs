using MigraineDiary.Data.Common.Contracts;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MigraineDiary.Data.DbModels
{
    public class ZungScaleForAnxiety : ISoftDeletable
    {
        public ZungScaleForAnxiety()
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
        /// First question's answer from Zung's scale for anxiety.
        /// </summary>
        [Required]
        [StringLength(20)]
        public string FirstQuestionAnswer { get; set; } = null!;

        /// <summary>
        /// Second question's answer from Zung's scale for anxiety.
        /// </summary>
        [Required]
        [StringLength(20)]
        public string SecondQuestionAnswer { get; set; } = null!;

        /// <summary>
        /// Third question's answer from Zung's scale for anxiety.
        /// </summary>
        [Required]
        [StringLength(20)]
        public string ThirdQuestionAnswer { get; set; } = null!;

        /// <summary>
        /// Fourth question's answer from Zung's scale for anxiety.
        /// </summary>
        [Required]
        [StringLength(20)]
        public string FourthQuestionAnswer { get; set; } = null!;

        /// <summary>
        /// Fifth question's answer from Zung's scale for anxiety.
        /// </summary>
        [Required]
        [StringLength(20)]
        public string FifthQuestionAnswer { get; set; } = null!;

        /// <summary>
        /// Sixth question's answer from Zung's scale for anxiety.
        /// </summary>
        [Required]
        [StringLength(20)]
        public string SixthQuestionAnswer { get; set; } = null!;

        /// <summary>
        /// Seventh question's answer from Zung's scale for anxiety.
        /// </summary>
        [Required]
        [StringLength(20)]
        public string SeventhQuestionAnswer { get; set; } = null!;

        /// <summary>
        /// Eighth question's answer from Zung's scale for anxiety.
        /// </summary>
        [Required]
        [StringLength(20)]
        public string EighthQuestionAnswer { get; set; } = null!;

        /// <summary>
        /// Ninth question's answer from Zung's scale for anxiety.
        /// </summary>
        [Required]
        [StringLength(20)]
        public string NinthQuestionAnswer { get; set; } = null!;

        /// <summary>
        /// Tenth question's answer from Zung's scale for anxiety.
        /// </summary>
        [Required]
        [StringLength(20)]
        public string TenthQuestionAnswer { get; set; } = null!;

        /// <summary>
        /// Eleventh question's answer from Zung's scale for anxiety.
        /// </summary>
        [Required]
        [StringLength(20)]
        public string EleventhQuestionAnswer { get; set; } = null!;

        /// <summary>
        /// Twelfth question's answer from Zung's scale for anxiety.
        /// </summary>
        [Required]
        [StringLength(20)]
        public string TwelfthQuestionAnswer { get; set; } = null!;

        /// <summary>
        /// Thirteenth question's answer from Zung's scale for anxiety.
        /// </summary>
        [Required]
        [StringLength(20)]
        public string ThirteenthQuestionAnswer { get; set; } = null!;

        /// <summary>
        /// Fourteenth question's answer from Zung's scale for anxiety.
        /// </summary>
        [Required]
        [StringLength(20)]
        public string FourteenthQuestionAnswer { get; set; } = null!;

        /// <summary>
        /// Fifteenth question's answer from Zung's scale for anxiety.
        /// </summary>
        [Required]
        [StringLength(20)]
        public string FifteenthQuestionAnswer { get; set; } = null!;

        /// <summary>
        /// Sixteenth question's answer from Zung's scale for anxiety.
        /// </summary>
        [Required]
        [StringLength(20)]
        public string SixteenthQuestionAnswer { get; set; } = null!;

        /// <summary>
        /// Seventeenth question's answer from Zung's scale for anxiety.
        /// </summary>
        [Required]
        [StringLength(20)]
        public string SeventeenthQuestionAnswer { get; set; } = null!;

        /// <summary>
        /// Eighteenth question's answer from Zung's scale for anxiety.
        /// </summary>
        [Required]
        [StringLength(20)]
        public string EighteenthQuestionAnswer { get; set; } = null!;

        /// <summary>
        /// Nineteenth question's answer from Zung's scale for anxiety.
        /// </summary>
        [Required]
        [StringLength(20)]
        public string NineteenthQuestionAnswer { get; set; } = null!;

        /// <summary>
        /// Twentieth question's answer from Zung's scale for anxiety.
        /// </summary>
        [Required]
        [StringLength(20)]
        public string TwentiethQuestionAnswer { get; set; } = null!;

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
        /// Patient's registered Zung's scales for anxiety shared with user in role "Doctor".
        /// </summary>
        [Required]
        public ICollection<ApplicationUser> SharedWith { get; set; }

        /// <summary>
        /// Record showing when Zung's scale for anxiety is registered by user.
        /// </summary>
        [Required]
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Boolean flag showing if user have deleted registered Zung's scale for anxiety.
        /// </summary>
        [Required]
        public bool IsDeleted { get; set; } = false;

        /// <summary>
        /// Record showing when Zung's scale for anxiety is deleted by user.
        /// </summary>
        public DateTime? DeletedOn { get; set; }
    }
}
