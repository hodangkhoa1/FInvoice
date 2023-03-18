using DAL.Entities;
using DAL.Infrastructure;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories.Implements
{
    public class ExcelResultRepository : RepositoryBase<ExcelResult>, IRepository<ExcelResult>
    {
        private readonly FInvoiceDBContext _dbContext;
        private readonly IUnitOfWork _unitOfWork;

        public ExcelResultRepository(IDbFactory dbFactory, IUnitOfWork unitOfWork) : base(dbFactory)
        {
            _dbContext = dbFactory.Init();
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> ActionEdit(ExcelResult entity, string action)
        {
            bool check = false;

            switch (action)
            {
                case "EditExcelResult":
                    _dbSet.Update(entity);
                    await _unitOfWork.CommitAsync();
                    check = true;
                    break;
                case "AddExcelResult":
                    await _dbContext.AddAsync(entity);
                    await _dbContext.SaveChangesAsync();
                    check = true;
                    break;
            }

            return check;
        }

        public int Count(ExcelResult? entity, string? action)
        {
            return _dbContext.ExcelResults.Count();
        }

        public async Task<ExcelResult> Get(ExcelResult entity, string action)
        {
            ExcelResult result = new();

            switch (action)
            {
                case "GetByID":
                    result = await _dbContext.ExcelResults.FindAsync(entity.IdExcelResult);
                    break;
            }

            return result;
        }

        public async Task<List<ExcelResult>> GetAll(ExcelResult entity, string action)
        {
            List<ExcelResult> result = new();

            switch (action)
            {
                case "GetAllAccount":
                    result = await _dbContext.ExcelResults.ToListAsync();
                    break;
            }

            return result;
        }
    }
}
