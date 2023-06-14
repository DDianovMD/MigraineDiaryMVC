using MigraineDiary.Services.Contracts;
using Moq;

namespace MigraineDiary.Tests.Mocks.Services
{
    public static class MedicationServiceMock
    {
        public static IMedicationService Instance()
        {
            var medicationService = new Mock<IMedicationService>();

            medicationService.Setup(ms => ms.CalculateWholeTakenDosage(500m, 2m))
                .Returns(1000m);

            return medicationService.Object;
        }
    }
}
