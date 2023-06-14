using MigraineDiary.Services;
using MigraineDiary.Services.Contracts;
using NUnit.Framework;

namespace MigraineDiary.Tests.Services
{
    [TestFixture]
    public class MedicationServiceTests
    {
        private IMedicationService medicationService;

        [SetUp]
        public void SetUp() 
        {
            this.medicationService = new MedicationService();
        }

        [TestCase(500, 3)]
        [TestCase(200, 1)]
        [TestCase(0, 1)]
        public void CalculateWholeTakenDosage_MultiplicatesAsExpected(decimal singlePillDosage, decimal numberOfPillsTaken)
        {
            // Assert - in Setup method.

            // Act
            decimal result = this.medicationService.CalculateWholeTakenDosage(singlePillDosage, numberOfPillsTaken);

            // Assert
            Assert.That(result, Is.EqualTo(singlePillDosage * numberOfPillsTaken));
        }
    }
}
