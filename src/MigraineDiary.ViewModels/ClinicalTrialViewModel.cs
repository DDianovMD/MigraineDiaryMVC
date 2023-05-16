namespace MigraineDiary.ViewModels
{
    public class ClinicalTrialViewModel
    {
        public string Id { get; set; } = null!;

        public string creatorId { get; set; } = null!;

        public string Heading { get; set; } = null!;

        public string City { get; set; } = null!;

        public string Hospital { get; set; } = null!;

        public string AgreementDocumentName { get; set; } = null!;

        public ICollection<PracticionerViewModel> Practicioners { get; set; } = new List<PracticionerViewModel>();

        public DateTime CreatedOn { get; set; }
    }
}
