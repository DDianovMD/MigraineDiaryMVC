namespace MigraineDiary.Web.Services.Contracts
{
    public interface IZungScaleForAnxietyService
    {
        public int CalculateTotalScore(string[] answers);

        public bool ValidateAnswer(string answer);
    }
}
