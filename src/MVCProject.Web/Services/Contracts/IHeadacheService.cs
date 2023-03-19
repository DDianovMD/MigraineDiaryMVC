namespace MigraineDiary.Web.Services.Contracts
{
    public interface IHeadacheService
    {
        public Dictionary<string, int> CalculateDuration(DateTime onset, DateTime endtime);
    }
}
