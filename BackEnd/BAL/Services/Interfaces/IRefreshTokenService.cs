using DAL.Entities;

namespace BAL.Services.Interfaces
{
    public interface IRefreshTokenService
    {
        Task<bool> AddRefreshToken(RefreshToken refreshToken);
        Task<RefreshToken> FindToken(string refreshToken);
        Task<bool> ChangeStatusRefreshToken(RefreshToken refreshToken);
        Task DeleteAllRefreshToken(string userID);
    }
}
