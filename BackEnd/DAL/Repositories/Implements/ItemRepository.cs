using DAL.Entities;
using DAL.Infrastructure;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories.Implements
{
    public class ItemRepository : RepositoryBase<Item>, IRepository<Item>
    {
        private readonly FInvoiceDBContext _dbContext;
        private readonly IUnitOfWork _unitOfWork;

        public ItemRepository(IDbFactory dbFactory, IUnitOfWork unitOfWork) : base(dbFactory)
        {
            _dbContext = dbFactory.Init();
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> ActionEdit(Item entity, string action)
        {
            bool check = false;

            switch (action)
            {
                case "EditItem":
                    _dbSet.Update(entity);
                    await _unitOfWork.CommitAsync();
                    check = true;
                    break;
                case "AddItem":
                    await _dbContext.AddAsync(entity);
                    await _dbContext.SaveChangesAsync();
                    check = true;
                    break;
            }

            return check;
        }

        public int Count(Item? entity, string? action)
        {
            return _dbContext.Items.Count();
        }

        public async Task<Item> Get(Item entity, string action)
        {
            Item result = new();

            switch (action)
            {
                case "GetByID":
                    result = await _dbContext.Items.FindAsync(entity.IdItem);
                    break;
                case "GetByName":
                    result = await _dbContext.Items.Where(a => a.Name.Equals(entity.Name)).FirstOrDefaultAsync();
                    break;
            }

            return result;
        }

        public async Task<List<Item>> GetAll(Item entity, string action)
        {
            List<Item> result = new();

            switch (action)
            {
                case "GetAllItem":
                    result = await _dbContext.Items.ToListAsync();
                    break;
            }

            return result;
        }
    }
}
