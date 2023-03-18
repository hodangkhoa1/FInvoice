using DAL.Entities;
using DAL.Infrastructure;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories.Implements
{
    public class ExcelFormRepository : RepositoryBase<ExcelForm>, IRepository<ExcelForm>
    {
        private readonly FInvoiceDBContext _dbContext;
        private readonly IUnitOfWork _unitOfWork;

        public ExcelFormRepository(IDbFactory dbFactory, IUnitOfWork unitOfWork) : base(dbFactory)
        {
            _dbContext = dbFactory.Init();
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> ActionEdit(ExcelForm entity, string action)
        {
            bool check = false;

            switch (action)
            {
                case "EditExcelForm":
                    _dbSet.Update(entity);
                    await _unitOfWork.CommitAsync();
                    check = true;
                    break;
                case "AddExcelForm":
                    await _dbContext.AddAsync(entity);
                    await _dbContext.SaveChangesAsync();
                    check = true;
                    break;
            }

            return check;
        }

        public int Count(ExcelForm? entity, string? action)
        {
            return _dbContext.ExcelForms.Count();
        }

        public async Task<ExcelForm> Get(ExcelForm entity, string action)
        {
            ExcelForm result = new();

            switch (action)
            {
                case "GetByID":
                    result = await _dbContext.ExcelForms.FindAsync(entity.IdExcelForm);
                    break;
                case "GetByName":
                    result = await _dbContext.ExcelForms.Where(ef => ef.Name.Equals(entity.Name)).FirstOrDefaultAsync();
                    break;
            }

            return result;
        }

        public async Task<List<ExcelForm>> GetAll(ExcelForm entity, string action)
        {
            List<ExcelForm> result = new();

            switch (action)
            {
                case "GetAllExcelForm":
                    result = await _dbContext.ExcelForms.ToListAsync();
                    break;
            }

            return result;
        }
    }
}
