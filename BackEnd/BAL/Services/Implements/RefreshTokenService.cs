using BAL.Services.Interfaces;
using DAL.Entities;
using DAL.Repositories.Interfaces;

namespace BAL.Services.Implements
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;

        public RefreshTokenService(IRefreshTokenRepository refreshTokenRepository)
        {
            _refreshTokenRepository = refreshTokenRepository;
        }

        public async Task<bool> AddRefreshToken(RefreshToken refreshToken)
        {
            return await _refreshTokenRepository.AddRefreshToken(refreshToken);
        }

        public async Task<bool> ChangeStatusRefreshToken(RefreshToken refreshToken)
        {
            return await _refreshTokenRepository.ChangeStatusRefreshToken(refreshToken);
        }

        public async Task DeleteAllRefreshToken(string userID)
        {
            await _refreshTokenRepository.DeleteAllRefreshToken(userID);
        }

        public async Task<RefreshToken> FindToken(string refreshToken)
        {
            return await _refreshTokenRepository.FindToken(refreshToken);
        }
    }
}
