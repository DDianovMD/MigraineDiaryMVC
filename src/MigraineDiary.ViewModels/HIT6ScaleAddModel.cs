﻿using System.ComponentModel.DataAnnotations;

namespace MigraineDiary.ViewModels
{
    public class HIT6ScaleAddModel
    {
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

        public int? TotalScore { get; set; }
    }
}
