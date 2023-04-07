using System.ComponentModel.DataAnnotations;

namespace MigraineDiary.Web.Models
{
    public class HIT6ScaleViewModel
    {
        [Required]
        public string Id { get; set; } = null!;

        [Required]
        [StringLength(10)]
        public string FirstQuestionAnswer { get; set; } = null!;

        [Required]
        [StringLength(10)]
        public string SecondQuestionAnswer { get; set; } = null!;

        [Required]
        [StringLength(10)]
        public string ThirdQuestionAnswer { get; set; } = null!;

        [Required]
        [StringLength(10)]
        public string FourthQuestionAnswer { get; set; } = null!;

        [Required]
        [StringLength(10)]
        public string FifthQuestionAnswer { get; set; } = null!;

        [Required]
        [StringLength(10)]
        public string SixthQuestionAnswer { get; set; } = null!;

        [Required]
        public int TotalScore { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }
    }
}
