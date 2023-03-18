using DAL.Entities;
using DAL.Infrastructure;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories.Implements
{
    public class RefreshTokenRepository : RepositoryBase<RefreshToken>, IRefreshTokenRepository
    {
        private readonly FInvoiceDBContext _dbContext;

        public RefreshTokenRepository(IDbFactory dbFactory) : base(dbFactory)
        {
            _dbContext = dbFactory.Init();
        }

        public async Task<bool> AddRefreshToken(RefreshToken refreshToken)
        {
            if (refreshToken != null)
            {
                await _dbContext.AddAsync(refreshToken);
                await _dbContext.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<bool> ChangeStatusRefreshToken(RefreshToken refreshToken)
        {
            if (refreshToken != null)
            {
                refreshToken.IsUsed = true;
                refreshToken.IsRevoked = true;
                _dbContext.Update(refreshToken);
                await _dbContext.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task DeleteAllRefreshToken(string userID)
        {
            IEnumerable<RefreshToken> refreshTokens = await _dbSet.Where(t => t.IdAccount == userID).ToListAsync();
            _dbSet.RemoveRange(refreshTokens);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<RefreshToken> FindToken(string refreshToken)
        {
            var result = await _dbSet.Where(t => t.Token == refreshToken).FirstOrDefaultAsync();
            return result;
        }
    }
}
