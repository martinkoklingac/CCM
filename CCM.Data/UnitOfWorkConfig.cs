namespace CCM.Data
{
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
