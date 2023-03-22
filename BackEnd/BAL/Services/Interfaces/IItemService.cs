using DAL.Entities;

namespace BAL.Services.Interfaces
{
    public interface IItemService
    {
        Task<Item> GetItem(Item item, string action);
        Task<bool> ActionItem(Item item, string action);
        int Count(Item? item, string? action);
        void Save();
        void SaveAsync();
    }
}
