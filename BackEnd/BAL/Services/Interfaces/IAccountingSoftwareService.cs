using BAL.Models;
using DAL.Entities;

namespace BAL.Services.Interfaces
{
    public interface IAccountingSoftwareService
    {
        Task<AccountingSoftware> GetAccountingSoftware(AccountingSoftware accountingSoftware, string action);
        Task<bool> ActionAccountingSoftware(AccountingSoftware accountingSoftware, string action);
        Task<AccountingSoftwareInfoViewModel> GetAccountingSoftwareTask(AccountingSoftware accountingSoftware, string action);
        Task<List<AccountingSoftware>> GetAllAccountingSoftware(AccountingSoftware accountingSoftware, string action);
        int Count(AccountingSoftware? accountingSoftware, string? action);
        void Save();
        void SaveAsync();
    }
}
