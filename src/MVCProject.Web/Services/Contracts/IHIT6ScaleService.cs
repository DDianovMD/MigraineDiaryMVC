namespace MigraineDiary.Web.Services.Contracts
{
    public interface IHIT6ScaleService
    {
        public int CalculateTotalScore(string[] answers);

        public bool ValidateAnswer(string answer);
    }
}
