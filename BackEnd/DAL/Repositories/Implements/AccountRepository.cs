using DAL.Entities;
using DAL.Infrastructure;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories.Implements
{
    public class AccountRepository : RepositoryBase<Account>, IRepository<Account>
    {
        private readonly FInvoiceDBContext _dbContext;
        private readonly IUnitOfWork _unitOfWork;

        public AccountRepository(IDbFactory dbFactory, IUnitOfWork unitOfWork) : base(dbFactory)
        {
            _dbContext = dbFactory.Init();
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> ActionEdit(Account entity, string action)
        {
            bool check = false;

            switch (action)
            {
                case "EditProfile":
                    _dbSet.Update(entity);
                    await _unitOfWork.CommitAsync();
                    check = true;
                    break;
                case "AddAccount":
                    await _dbContext.AddAsync(entity);
                    await _dbContext.SaveChangesAsync();
                    check = true;
                    break;
            }

            return check;
        }

        public int Count(Account? entity, string? action)
        {
            int countValue = 0;

            if (entity != null)
            {
                countValue = _dbContext.Accounts.Where(account => account.UserRole == entity.UserRole).Count();
            }

            return countValue;
        }

        public async Task<Account> Get(Account entity, string action)
        {
            Account result = new();

            switch (action)
            {
                case "GetByID":
                    result = await _dbContext.Accounts.FindAsync(entity.IdAccount);
                    break;
                case "FindByEmail":
                    result = await _dbContext.Accounts.Where(a => a.Email.Equals(entity.Email)).FirstOrDefaultAsync();
                    break;
                case "GetOtpCode":
                    result = await _dbContext.Accounts.Where(a => a.OtpCode.Equals(entity.OtpCode)).FirstOrDefaultAsync();
                    break;
            }

            return result;
        }

        public async Task<List<Account>> GetAll(Account entity, string action)
        {
            List<Account> result = new();

            switch (action)
            {
                case "GetAllAccount":
                    result = await _dbContext.Accounts.ToListAsync();
                    break;
                case "GetAllAccountWithRole":
                    result = await _dbContext.Accounts.Where(a => a.UserRole == entity.UserRole).ToListAsync();
                    break;
            }

            return result;
        }
    }
}
