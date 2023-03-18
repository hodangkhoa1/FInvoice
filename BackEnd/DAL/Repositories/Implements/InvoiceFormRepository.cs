using DAL.Entities;
using DAL.Infrastructure;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories.Implements
{
    public class InvoiceFormRepository : RepositoryBase<InvoiceForm>, IRepository<InvoiceForm>
    {
        private readonly FInvoiceDBContext _dbContext;
        private readonly IUnitOfWork _unitOfWork;

        public InvoiceFormRepository(IDbFactory dbFactory, IUnitOfWork unitOfWork) : base(dbFactory)
        {
            _dbContext = dbFactory.Init();
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> ActionEdit(InvoiceForm entity, string action)
        {
            bool check = false;

            switch (action)
            {
                case "EditInvoiceForm":
                    _dbSet.Update(entity);
                    await _unitOfWork.CommitAsync();
                    check = true;
                    break;
                case "AddInvoiceForm":
                    await _dbContext.AddAsync(entity);
                    await _dbContext.SaveChangesAsync();
                    check = true;
                    break;
            }

            return check;
        }

        public int Count(InvoiceForm? entity, string? action)
        {
            return _dbContext.InvoiceForms.Count();
        }

        public async Task<InvoiceForm> Get(InvoiceForm entity, string action)
        {
            InvoiceForm result = new();

            switch (action)
            {
                case "GetByID":
                    result = await _dbContext.InvoiceForms.FindAsync(entity.IdInvoiceForm);
                    break;
                case "GetByCodeForm":
                    result = await _dbContext.InvoiceForms.Where(a => a.CodeForm.Equals(entity.CodeForm)).FirstOrDefaultAsync();
                    break;
            }

            return result;
        }

        public async Task<List<InvoiceForm>> GetAll(InvoiceForm entity, string action)
        {
            List<InvoiceForm> result = new();

            switch (action)
            {
                case "GetAllInvoiceForm":
                    result = await _dbContext.InvoiceForms.ToListAsync();
                    break;
            }

            return result;
        }
    }
}
