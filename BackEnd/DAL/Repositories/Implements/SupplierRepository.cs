using DAL.Entities;
using DAL.Infrastructure;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories.Implements
{
    public class SupplierRepository : RepositoryBase<Supplier>, IRepository<Supplier>
    {
        private readonly FInvoiceDBContext _dbContext;
        private readonly IUnitOfWork _unitOfWork;

        public SupplierRepository(IDbFactory dbFactory, IUnitOfWork unitOfWork) : base(dbFactory)
        {
            _dbContext = dbFactory.Init();
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> ActionEdit(Supplier entity, string action)
        {
            bool check = false;

            switch (action)
            {
                case "EditSupplier":
                    _dbSet.Update(entity);
                    await _unitOfWork.CommitAsync();
                    check = true;
                    break;
                case "AddSupplier":
                    await _dbContext.AddAsync(entity);
                    await _dbContext.SaveChangesAsync();
                    check = true;
                    break;
            }

            return check;
        }

        public int Count(Supplier? entity, string? action)
        {
            return _dbContext.Suppliers.Count();
        }

        public async Task<Supplier> Get(Supplier entity, string action)
        {
            Supplier result = new();

            switch (action)
            {
                case "GetByID":
                    result = await _dbContext.Suppliers.FindAsync(entity.IdSupplier);
                    break;
                case "GetByName":
                    result = await _dbContext.Suppliers.Where(s => s.Name.Equals(entity.Name)).FirstOrDefaultAsync();
                    break;
            }

            return result;
        }

        public async Task<List<Supplier>> GetAll(Supplier entity, string action)
        {
            List<Supplier> result = new();

            switch (action)
            {
                case "GetAllSupplier":
                    result = await _dbContext.Suppliers.ToListAsync();
                    break;
            }

            return result;
        }
    }
}
