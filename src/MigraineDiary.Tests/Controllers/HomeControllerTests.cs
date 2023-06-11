using Microsoft.AspNetCore.Mvc;
using MigraineDiary.Tests.Mocks.Models;
using MigraineDiary.Tests.Mocks.Services;
using MigraineDiary.ViewModels;
using MigraineDiary.Web.Controllers;
using NUnit.Framework;

namespace MigraineDiary.Tests.Controllers
{
    [TestFixture]
    public class HomeControllerTests
    {
        private HomeController controller;

        [SetUp]
        public async Task Setup()
        {
            this.controller = new HomeController(LoggerMock.Instance(), await ArticleServiceMock.Instance());
        }

        [TestCase(1, 2, "NewestFirst")]
        [TestCase(1, 2, "OldestFirst")]
        [TestCase(1, 1, "RandomString")]
        [TestCase(1, 5, "RandomString")]
        [TestCase(1, 10, "RandomString")]
        public async Task Index_ParameterTamperingReturnsBadRequest(int pageIndex, int pageSize, string orderByDate)
        {
            // Arrange - in Setup method.

            // Act
            var result = await controller.Index(pageIndex, pageSize, orderByDate);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<BadRequestResult>());
        }

        [TestCase(1, 1, "NewestFirst")]
        [TestCase(1, 5, "NewestFirst")]
        [TestCase(1, 10, "NewestFirst")]
        [TestCase(1, 1, "OldestFirst")]
        [TestCase(1, 5, "OldestFirst")]
        [TestCase(1, 10, "OldestFirst")]
        public async Task Index_ReturnsViewWithValidDataProvided(int pageIndex, int pageSize, string orderByDate)
        {
            // Arrange - in Setup method.

            // Act
            var result = await controller.Index(pageIndex, pageSize, orderByDate);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<ViewResult>());
        }

        [Test]
        public void Privacy_ReturnsViewWhenCalled()
        {
            // Arrange - in Setup method.

            // Act
            var result = controller.Privacy();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<ViewResult>());
        }
    }
}
