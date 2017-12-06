namespace CCMAdmin
{
    public interface IUnitOfWorkConfig
    {
        string ConnectionString { get; }
    }

    public class UnitOfWorkConfig :
        IUnitOfWorkConfig
    {
        public UnitOfWorkConfig(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        public string ConnectionString { get; }
    }
}
