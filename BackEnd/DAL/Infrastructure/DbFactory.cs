namespace DAL.Infrastructure
{
    public class DbFactory : IDbFactory
    {
        private FInvoiceDBContext _dbContext;

        public DbFactory(FInvoiceDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public FInvoiceDBContext Init()
        {
            _dbContext ??= new FInvoiceDBContext();

            return _dbContext;
        }
    }
}
