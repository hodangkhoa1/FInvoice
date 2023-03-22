using DAL.Entities;

namespace BAL.Services.Interfaces
{
    public interface IBuyerService
    {
        Task<Buyer> GetBuyer(Buyer buyer, string action);
        Task<bool> ActionBuyer(Buyer buyer, string action);
        int Count(Buyer? buyer, string? action);
        void Save();
        void SaveAsync();
    }
}
