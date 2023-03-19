using MigraineDiary.Web.Services.Contracts;

namespace MigraineDiary.Web.Services
{
    public class HeadacheService : IHeadacheService
    {
        public TimeSpan CalculateDuration(DateTime endTime, DateTime onset)
        {
            return endTime - onset;
        }
    }
}
