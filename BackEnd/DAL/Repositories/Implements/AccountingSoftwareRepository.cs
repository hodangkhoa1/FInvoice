using DAL.Entities;
using DAL.Infrastructure;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories.Implements
{
    public class AccountingSoftwareRepository : RepositoryBase<AccountingSoftware>, IRepository<AccountingSoftware>
    {
        private readonly FInvoiceDBContext _dbContext;
        private readonly IUnitOfWork _unitOfWork;

        public AccountingSoftwareRepository(IDbFactory dbFactory, IUnitOfWork unitOfWork) : base(dbFactory)
        {
            _dbContext = dbFactory.Init();
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> ActionEdit(AccountingSoftware entity, string action)
        {
            bool check = false;

            switch (action)
            {
                case "EditAccountingSoftware":
                    _dbSet.Update(entity);
                    await _unitOfWork.CommitAsync();
                    check = true;
                    break;
                case "AddAccountingSoftware":
                    await _dbContext.AddAsync(entity);
                    await _dbContext.SaveChangesAsync();
                    check = true;
                    break;
            }

            return check;
        }

        public int Count(AccountingSoftware? entity, string? action)
        {
            return _dbContext.AccountingSoftwares.Count();
        }

        public async Task<AccountingSoftware> Get(AccountingSoftware entity, string action)
        {
            AccountingSoftware result = new();

            switch (action)
            {
                case "GetByID":
                    result = await _dbContext.AccountingSoftwares.FindAsync(entity.IdAccountingSoftware);
                    break;
                case "GetByName":
                    result = await _dbContext.AccountingSoftwares.Where(a => a.Name.Equals(entity.Name)).FirstOrDefaultAsync();
                    break;
            }

            return result;
        }

        public async Task<List<AccountingSoftware>> GetAll(AccountingSoftware entity, string action)
        {
            List<AccountingSoftware> result = new();

            switch (action)
            {
                case "GetAllAccountingSoftware":
                    result = await _dbContext.AccountingSoftwares.ToListAsync();
                    break;
            }

            return result;
        }
    }
}
