using System.ComponentModel.DataAnnotations;

namespace MigraineDiary.ViewModels
{
    public class ZungScaleViewModel
    {
        public ZungScaleViewModel()
        {
            this.Doctors = new HashSet<DoctorViewModel>();
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
        /// Date of registration.
        /// </summary>
        [Required]
        public DateTime CreatedOn { get; set; }

        public IEnumerable<DoctorViewModel> Doctors { get; set; }
    }
}
