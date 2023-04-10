using MigraineDiary.Web.Services.Contracts;
using System.Diagnostics.Metrics;

namespace MigraineDiary.Web.Services
{
    public class ZungScaleForAnxietyService : IZungScaleForAnxietyService
    {
        private const string noAnswer = "NoAnswer";
        private const string neverOrRarely = "Never or rarely";
        private const string sometimes = "Sometimes";
        private const string often = "Often";
        private const string veryOftenOrAlways = "Very often or always";

        public int CalculateTotalScore(string[] answers)
        {
            int totalScore = 0;

            foreach (var answer in answers)
            {
                if (answer == neverOrRarely)
                {
                    totalScore += 1;
                }
                else if (answer == sometimes) 
                {
                    totalScore += 2;
                }
                else if(answer == often)
                {
                    totalScore += 3;
                }
                else if(answer == veryOftenOrAlways)
                {
                    totalScore += 4;
                }
            }

            return totalScore;
        }

        public bool ValidateAnswer(string answer)
        {
            if (answer == noAnswer ||
               (answer != neverOrRarely &&
                answer != sometimes &&
                answer != often &&
                answer != veryOftenOrAlways))
            {
                return true;
            }

            return false;
        }
    }
}
