namespace MigraineDiary.ViewModels
{
    public class SharedZungScaleForAnxietyViewModel
    {
        /// <summary>
        /// Primary key.
        /// </summary>
        public string ZungScaleId { get; set; } = null!;

        /// <summary>
        /// Patient's first name.
        /// </summary>
        public string PatientFirstName { get; set; } = null!;

        /// <summary>
        /// Patient's middle name.
        /// </summary>
        public string? PatientMiddleName { get; set; }

        /// <summary>
        /// Patient's last name.
        /// </summary>
        public string PatientLastName { get; set; } = null!;

        /// <summary>
        /// First question's answer from Zung's scale for anxiety.
        /// </summary>
        public string FirstQuestionAnswer { get; set; } = null!;

        /// <summary>
        /// Second question's answer from Zung's scale for anxiety.
        /// </summary>
        public string SecondQuestionAnswer { get; set; } = null!;

        /// <summary>
        /// Third question's answer from Zung's scale for anxiety.
        /// </summary>
        public string ThirdQuestionAnswer { get; set; } = null!;

        /// <summary>
        /// Fourth question's answer from Zung's scale for anxiety.
        /// </summary>
        public string FourthQuestionAnswer { get; set; } = null!;

        /// <summary>
        /// Fifth question's answer from Zung's scale for anxiety.
        /// </summary>
        public string FifthQuestionAnswer { get; set; } = null!;

        /// <summary>
        /// Sixth question's answer from Zung's scale for anxiety.
        /// </summary>
        public string SixthQuestionAnswer { get; set; } = null!;

        /// <summary>
        /// Seventh question's answer from Zung's scale for anxiety.
        /// </summary>
        public string SeventhQuestionAnswer { get; set; } = null!;

        /// <summary>
        /// Eighth question's answer from Zung's scale for anxiety.
        /// </summary>
        public string EighthQuestionAnswer { get; set; } = null!;

        /// <summary>
        /// Ninth question's answer from Zung's scale for anxiety.
        /// </summary>
        public string NinthQuestionAnswer { get; set; } = null!;

        /// <summary>
        /// Tenth question's answer from Zung's scale for anxiety.
        /// </summary>
        public string TenthQuestionAnswer { get; set; } = null!;

        /// <summary>
        /// Eleventh question's answer from Zung's scale for anxiety.
        /// </summary>
        public string EleventhQuestionAnswer { get; set; } = null!;

        /// <summary>
        /// Twelfth question's answer from Zung's scale for anxiety.
        /// </summary>
        public string TwelfthQuestionAnswer { get; set; } = null!;

        /// <summary>
        /// Thirteenth question's answer from Zung's scale for anxiety.
        /// </summary>
        public string ThirteenthQuestionAnswer { get; set; } = null!;

        /// <summary>
        /// Fourteenth question's answer from Zung's scale for anxiety.
        /// </summary>
        public string FourteenthQuestionAnswer { get; set; } = null!;

        /// <summary>
        /// Fifteenth question's answer from Zung's scale for anxiety.
        /// </summary>
        public string FifteenthQuestionAnswer { get; set; } = null!;

        /// <summary>
        /// Sixteenth question's answer from Zung's scale for anxiety.
        /// </summary>
        public string SixteenthQuestionAnswer { get; set; } = null!;

        /// <summary>
        /// Seventeenth question's answer from Zung's scale for anxiety.
        /// </summary>
        public string SeventeenthQuestionAnswer { get; set; } = null!;

        /// <summary>
        /// Eighteenth question's answer from Zung's scale for anxiety.
        /// </summary>
        public string EighteenthQuestionAnswer { get; set; } = null!;

        /// <summary>
        /// Nineteenth question's answer from Zung's scale for anxiety.
        /// </summary>
        public string NineteenthQuestionAnswer { get; set; } = null!;

        /// <summary>
        /// Twentieth question's answer from Zung's scale for anxiety.
        /// </summary>
        public string TwentiethQuestionAnswer { get; set; } = null!;

        /// <summary>
        /// Scored result according to patient's given answers.
        /// </summary>
        public int TotalScore { get; set; }

        /// <summary>
        /// Date of registration.
        /// </summary>
        public DateTime CreatedOn { get; set; }
    }
}
