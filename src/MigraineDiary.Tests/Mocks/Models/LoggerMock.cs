using Microsoft.Extensions.Logging;
using MigraineDiary.Web.Controllers;
using Moq;

namespace MigraineDiary.Tests.Mocks.Models
{
    public static class LoggerMock
    {
        public static ILogger<HomeController> Instance()
        {
            var loggerMock = new Mock<ILogger<HomeController>>();

            return loggerMock.Object;
        }
    }
}
