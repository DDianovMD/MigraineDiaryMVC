namespace MigraineDiary.Web.Services.Contracts
{
    public interface IHeadacheService
    {
        public TimeSpan CalculateDuration(DateTime endtime, DateTime onset);
    }
}
