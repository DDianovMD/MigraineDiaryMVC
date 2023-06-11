using MigraineDiary.ViewModels;

namespace MigraineDiary.Tests.Mocks.Models
{
    public static class MessageAddModelMock
    {
        public static MessageAddModel Instance()
        {
            return new MessageAddModel()
            {
                SenderName = "Test",
                SenderEmail = "test@test.com",
                Title = "Test",
                MessageContent = "test",
            };
        }
    }
}
