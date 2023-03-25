using MigraineDiary.Web.Services.Contracts;

namespace MigraineDiary.Web.Services
{
    public class HIT6ScaleService : IHIT6ScaleService
    {
        private const string noAnswer = "NoAnswer";
        private const string never = "Never";
        private const string rarely = "Rarely";
        private const string sometimes = "Sometimes";
        private const string veryOften = "Very often";
        private const string always = "Always";

        public int CalculateTotalScore(string[] answers)
        {
            int totalScore = 0;

            foreach (var answer in answers)
            {
                if (answer == never)
                {
                    totalScore += 6;
                }
                else if (answer == rarely)
                {
                    totalScore += 8;
                }
                else if (answer == sometimes)
                {
                    totalScore += 10;
                }
                else if (answer == veryOften)
                {
                    totalScore += 11;
                }
                else if (answer == always)
                {
                    totalScore += 13;
                }
            }

            return totalScore;
        }

        public bool ValidateAnswer(string answer)
        {
            if (answer == noAnswer ||
               (answer != never &&
                answer != rarely &&
                answer != sometimes &&
                answer != veryOften &&
                answer != always))
            {
                return true;
            }

            return false;
        }
    }
}
