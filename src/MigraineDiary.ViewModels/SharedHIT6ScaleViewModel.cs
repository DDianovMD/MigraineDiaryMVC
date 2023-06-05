namespace MigraineDiary.ViewModels
{
    public class SharedHIT6ScaleViewModel
    {
        public string HIT6ScaleId { get; set; } = null!;

        public string PatientFirstName { get; set; } = null!;

        public string? PatientMiddleName { get; set; }

        public string PatientLastName { get; set; } = null!;

        public string FirstQuestionAnswer { get; set; } = null!;

        public string SecondQuestionAnswer { get; set; } = null!;

        public string ThirdQuestionAnswer { get; set; } = null!;

        public string FourthQuestionAnswer { get; set; } = null!;

        public string FifthQuestionAnswer { get; set; } = null!;

        public string SixthQuestionAnswer { get; set; } = null!;

        public int TotalScore { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
