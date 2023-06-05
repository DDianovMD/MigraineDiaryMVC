namespace MigraineDiary.ViewModels
{
    public class PatientViewModel
    {
        public string PatientId { get; init; } = null!;

        public string FirstName { get; init; } = null!;

        public string? MiddleName { get; init; }

        public string LastName { get; init; } = null!;
    }
}
