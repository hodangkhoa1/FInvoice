using DAL.Entities;

namespace BAL.Services.Interfaces
{
    public interface ISellerService
    {
        Task<Seller> GetSeller(Seller seller, string action);
        Task<bool> ActionSeller(Seller seller, string action);
        int Count(Seller? seller, string? action);
        void Save();
        void SaveAsync();
    }
}
