using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MigraineDiary.Tests.Mocks.Models;
using MigraineDiary.Tests.Mocks.Services;
using MigraineDiary.Web.Areas.Admin.Controllers;
using NUnit.Framework;
using System.Security.Claims;

namespace MigraineDiary.Tests.Controllers
{
    [TestFixture]
    public class ArticleControllerTests
    {
        private ArticleController controller;
        private const string SEEDED_ADMIN_ID = "ff3d52a7-7288-42aa-9955-6c4c4ad4caed";

        [SetUp]
        public async Task SetUp()
        {
            this.controller = new ArticleController(await ArticleServiceMock.Instance());

            ClaimsPrincipal user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "example name"),
                new Claim(ClaimTypes.NameIdentifier, SEEDED_ADMIN_ID),
            }, "mock"));

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
                {
                    User = user
                }
            };
        }

        [Test]
        public void Add_GetMethodReturnsView()
        {
            // Arrange - in Setup method.

            // Act
            var result = this.controller.Add();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<ViewResult>());
        }

        [Test]
        public async Task Add_PostMethodReturnsRedirectToAction_WhenModelStateIsValid()
        {
            // Arrange - in Setup method.

            // Act
            var result = await this.controller.Add(ArticleAddFormModelMock.Instance());

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
        }

        [Test]
        public async Task Add_PostMethodReturnsRedirectToAction_WhenModelStateIsInvalid()
        {
            // Arrange - in Setup method.
            controller.ModelState.AddModelError("test", "test");

            // Act
            var result = await this.controller.Add(ArticleAddFormModelMock.Instance());

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<ViewResult>());
        }
    }
}
