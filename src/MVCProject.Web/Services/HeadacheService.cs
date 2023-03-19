using MigraineDiary.Web.Services.Contracts;

namespace MigraineDiary.Web.Services
{
    public class HeadacheService : IHeadacheService
    {
        public Dictionary<string, int> CalculateDuration(DateTime onset, DateTime endtime)
        {
            TimeSpan headacheDuration = endtime - onset;
            Dictionary<string, int> daysHoursMinutes = new Dictionary<string, int>();

            daysHoursMinutes.Add("Days", headacheDuration.Days);
            daysHoursMinutes.Add("Hours", headacheDuration.Hours);
            daysHoursMinutes.Add("Minutes", headacheDuration.Minutes);
            
            return daysHoursMinutes;
        }
    }
}
