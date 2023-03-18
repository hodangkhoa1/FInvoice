using DAL.Entities;
using DAL.Infrastructure;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories.Implements
{
    public class InvoiceRepository : RepositoryBase<Invoice>, IRepository<Invoice>
    {
        private readonly FInvoiceDBContext _dbContext;
        private readonly IUnitOfWork _unitOfWork;

        public InvoiceRepository(IDbFactory dbFactory, IUnitOfWork unitOfWork) : base(dbFactory)
        {
            _dbContext = dbFactory.Init();
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> ActionEdit(Invoice entity, string action)
        {
            bool check = false;

            switch (action)
            {
                case "EditInvoice":
                    _dbSet.Update(entity);
                    await _unitOfWork.CommitAsync();
                    check = true;
                    break;
                case "AddInvoice":
                    await _dbContext.AddAsync(entity);
                    await _dbContext.SaveChangesAsync();
                    check = true;
                    break;
            }

            return check;
        }

        public int Count(Invoice? entity, string? action)
        {
            return _dbContext.Invoices.Count();
        }

        public async Task<Invoice> Get(Invoice entity, string action)
        {
            Invoice result = new();

            switch (action)
            {
                case "GetByID":
                    result = await _dbContext.Invoices.FindAsync(entity.IdInvoice);
                    break;
            }

            return result;
        }

        public async Task<List<Invoice>> GetAll(Invoice entity, string action)
        {
            List<Invoice> result = new();

            switch (action)
            {
                case "GetAllInvoice":
                    result = await _dbContext.Invoices.ToListAsync();
                    break;
            }

            return result;
        }
    }
}
