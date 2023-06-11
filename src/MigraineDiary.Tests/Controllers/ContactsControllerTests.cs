using Microsoft.AspNetCore.Mvc;
using MigraineDiary.Tests.Mocks.Models;
using MigraineDiary.Tests.Mocks.Services;
using MigraineDiary.Web.Controllers;
using NUnit.Framework;

namespace MigraineDiary.Tests.Controllers
{
    [TestFixture]
    public class ContactsControllerTests
    {
        private ContactsController controller;
        
        [SetUp]
        public async Task Setup()
        {
            controller = new ContactsController(await MessageServiceMock.Instance());
        }

        [Test]
        public void Index_GetMethod_ReturnsView()
        {
            // Arrange - in Setup method.

            // Act
            var result = controller.Index();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<ViewResult>());
        }

        [Test]
        public async Task Index_PostMethod_RedirectsToAction_WhenModelStateIsValid() 
        {
            // Arrange - in Setup method.

            // Act
            var result = await controller.Index(MessageAddModelMock.Instance());

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
        }

        [Test]
        public async Task Index_PostMethod_RedirectsToAction_WhenModelStateIsInvalid()
        {
            // Arrange - in Setup method.
            controller.ModelState.AddModelError("test", "test");

            // Act
            var result = await controller.Index(MessageAddModelMock.Instance());

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<ViewResult>());
        }

        [Test]
        public void Info_GetMethod_ReturnsView()
        {
            // Arrange - in Setup method.

            // Act
            var result = controller.Info();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<ViewResult>());
        }
    }
}
