using DAL.Entities;
using DAL.Infrastructure;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories.Implements
{
    public class SellerRepository : RepositoryBase<Seller>, IRepository<Seller>
    {
        private readonly FInvoiceDBContext _dbContext;
        private readonly IUnitOfWork _unitOfWork;

        public SellerRepository(IDbFactory dbFactory, IUnitOfWork unitOfWork) : base(dbFactory)
        {
            _dbContext = dbFactory.Init();
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> ActionEdit(Seller entity, string action)
        {
            bool check = false;

            switch (action)
            {
                case "EditSeller":
                    _dbSet.Update(entity);
                    await _unitOfWork.CommitAsync();
                    check = true;
                    break;
                case "AddSeller":
                    await _dbContext.AddAsync(entity);
                    await _dbContext.SaveChangesAsync();
                    check = true;
                    break;
            }

            return check;
        }

        public int Count(Seller? entity, string? action)
        {
            return _dbContext.Sellers.Count();
        }

        public async Task<Seller> Get(Seller entity, string action)
        {
            Seller result = new();

            switch (action)
            {
                case "GetByID":
                    result = await _dbContext.Sellers.FindAsync(entity.IdSeller);
                    break;
                case "GetByEmail":
                    result = await _dbContext.Sellers.Where(s => s.Email.Equals(entity.Email)).FirstOrDefaultAsync();
                    break;
            }

            return result;
        }

        public async Task<List<Seller>> GetAll(Seller entity, string action)
        {
            List<Seller> result = new();

            switch (action)
            {
                case "GetAllSeller":
                    result = await _dbContext.Sellers.ToListAsync();
                    break;
            }

            return result;
        }
    }
}
