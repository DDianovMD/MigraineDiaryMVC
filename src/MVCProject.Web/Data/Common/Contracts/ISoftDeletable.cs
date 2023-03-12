namespace MigraineDiary.Web.Data.Common.Contracts
{
    public interface ISoftDeletable
    {
        public bool IsDeleted { get; set; }
    }
}
