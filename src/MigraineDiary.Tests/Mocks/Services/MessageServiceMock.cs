using MigraineDiary.Services.Contracts;
using Moq;

namespace MigraineDiary.Tests.Mocks.Services
{
    public static class MessageServiceMock
    {
        public static async Task<IMessageService> Instance()
        {
            var messageService = new Mock<IMessageService>();

            return messageService.Object;
        }
    }
}
