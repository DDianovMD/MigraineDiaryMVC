namespace MigraineDiary.Data.Common.Contracts
{
    public interface IAuditable
    {
        public DateTime CreatedOn { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}
