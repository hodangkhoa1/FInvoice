using Microsoft.EntityFrameworkCore;

namespace DAL.Infrastructure
{
    public class RepositoryBase<T> where T : class
    {
        private FInvoiceDBContext _dbContext;
        protected DbSet<T> _dbSet { get; private set; }
        private IDbFactory DbFactory { get; }

        private FInvoiceDBContext DbContext
        {
            get
            {
                _dbContext ??= DbFactory.Init();
                return _dbContext;
            }
        }

        protected RepositoryBase(IDbFactory dbFactory)
        {
            DbFactory = dbFactory;
            _dbSet = DbContext.Set<T>();
        }
    }
}
