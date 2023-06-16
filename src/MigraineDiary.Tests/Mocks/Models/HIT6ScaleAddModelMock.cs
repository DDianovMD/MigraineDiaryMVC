using MigraineDiary.ViewModels;

namespace MigraineDiary.Tests.Mocks.Models
{
    public static class HIT6ScaleAddModelMock
    {
        public static HIT6ScaleAddModel Instance(string answer)
        {
            HIT6ScaleAddModel hit6scaleAddModel = new HIT6ScaleAddModel
            {
                FirstQuestionAnswer = answer,
                SecondQuestionAnswer = answer,
                ThirdQuestionAnswer = answer,
                FourthQuestionAnswer = answer,
                FifthQuestionAnswer = answer,
                SixthQuestionAnswer = answer,
                TotalScore = null,
            };

            return hit6scaleAddModel;
        }
    }
}
