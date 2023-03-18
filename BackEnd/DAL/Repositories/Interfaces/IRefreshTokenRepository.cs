using DAL.Entities;

namespace DAL.Repositories.Interfaces
{
    public interface IRefreshTokenRepository
    {
        Task<bool> AddRefreshToken(RefreshToken refreshToken);
        Task<RefreshToken> FindToken(string refreshToken);
        Task<bool> ChangeStatusRefreshToken(RefreshToken refreshToken);
        Task DeleteAllRefreshToken(string userID);
    }
}
