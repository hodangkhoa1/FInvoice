using DAL.Entities;
using DAL.Infrastructure;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories.Implements
{
    public class SuppliedInvoiceFormRepository : RepositoryBase<SuppliedInvoiceForm>, IRepository<SuppliedInvoiceForm>
    {
        private readonly FInvoiceDBContext _dbContext;
        private readonly IUnitOfWork _unitOfWork;

        public SuppliedInvoiceFormRepository(IDbFactory dbFactory, IUnitOfWork unitOfWork) : base(dbFactory)
        {
            _dbContext = dbFactory.Init();
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> ActionEdit(SuppliedInvoiceForm entity, string action)
        {
            bool check = false;

            switch (action)
            {
                case "EditSuppliedInvoiceForm":
                    _dbSet.Update(entity);
                    await _unitOfWork.CommitAsync();
                    check = true;
                    break;
                case "AddSuppliedInvoiceForm":
                    await _dbContext.AddAsync(entity);
                    await _dbContext.SaveChangesAsync();
                    check = true;
                    break;
            }

            return check;
        }

        public int Count(SuppliedInvoiceForm? entity, string? action)
        {
            return _dbContext.SuppliedInvoiceForms.Count();
        }

        public async Task<SuppliedInvoiceForm> Get(SuppliedInvoiceForm entity, string action)
        {
            SuppliedInvoiceForm result = new();

            switch (action)
            {
                case "GetByID":
                    result = await _dbContext.SuppliedInvoiceForms.FindAsync(entity.IdSuppliedInvoiceForm);
                    break;
            }

            return result;
        }

        public async Task<List<SuppliedInvoiceForm>> GetAll(SuppliedInvoiceForm entity, string action)
        {
            List<SuppliedInvoiceForm> result = new();

            switch (action)
            {
                case "GetAllSuppliedInvoiceForm":
                    result = await _dbContext.SuppliedInvoiceForms.ToListAsync();
                    break;
            }

            return result;
        }
    }
}
