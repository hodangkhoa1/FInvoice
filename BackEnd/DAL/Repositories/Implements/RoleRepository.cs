using DAL.Entities;
using DAL.Infrastructure;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace DAL.Repositories.Implements
{
    public class RoleRepository : RepositoryBase<Role>, IRepository<Role>
    {
        private readonly FInvoiceDBContext _dbContext;

        public RoleRepository(IDbFactory dbFactory) : base(dbFactory)
        {
            _dbContext = dbFactory.Init();
        }

        public Task<bool> ActionEdit(Role entity, string action)
        {
            throw new NotImplementedException();
        }

        public int Count(Role? entity, string? action)
        {
            throw new NotImplementedException();
        }

        public async Task<Role> Get(Role entity, string action)
        {
            Role getRole = new();

            switch (action)
            {
                case "GetRoleID":
                    getRole = await _dbContext.Roles.Where(r => r.IdRole == entity.IdRole).FirstOrDefaultAsync();
                    break;
                case "GetRoleName":
                    getRole = await _dbContext.Roles.Where(r => r.Name == entity.Name).FirstOrDefaultAsync();
                    break;
            }

            return getRole;
        }

        public async Task<List<Role>> GetAll(Role entity, string action)
        {
            return await _dbContext.Roles.ToListAsync();
        }
    }
}
