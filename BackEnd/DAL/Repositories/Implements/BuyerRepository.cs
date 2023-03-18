using DAL.Entities;
using DAL.Infrastructure;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories.Implements
{
    public class BuyerRepository : RepositoryBase<Buyer>, IRepository<Buyer>
    {
        private readonly FInvoiceDBContext _dbContext;
        private readonly IUnitOfWork _unitOfWork;

        public BuyerRepository(IDbFactory dbFactory, IUnitOfWork unitOfWork) : base(dbFactory)
        {
            _dbContext = dbFactory.Init();
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> ActionEdit(Buyer entity, string action)
        {
            bool check = false;

            switch (action)
            {
                case "EditBuyer":
                    _dbSet.Update(entity);
                    await _unitOfWork.CommitAsync();
                    check = true;
                    break;
                case "AddBuyer":
                    await _dbContext.AddAsync(entity);
                    await _dbContext.SaveChangesAsync();
                    check = true;
                    break;
            }

            return check;
        }

        public int Count(Buyer? entity, string? action)
        {
            return _dbContext.Buyers.Count();
        }

        public async Task<Buyer> Get(Buyer entity, string action)
        {
            Buyer result = new();

            switch (action)
            {
                case "GetByID":
                    result = await _dbContext.Buyers.FindAsync(entity.IdBuyer);
                    break;
                case "GetByName":
                    result = await _dbContext.Buyers.Where(b => b.Name.Equals(entity.Name)).FirstOrDefaultAsync();
                    break;
            }

            return result;
        }

        public async Task<List<Buyer>> GetAll(Buyer entity, string action)
        {
            List<Buyer> result = new();

            switch (action)
            {
                case "GetAllAccount":
                    result = await _dbContext.Buyers.ToListAsync();
                    break;
            }

            return result;
        }
    }
}
