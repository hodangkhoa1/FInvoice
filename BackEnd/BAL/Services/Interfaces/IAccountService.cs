using BAL.Models;
using DAL.Entities;

namespace BAL.Services.Interfaces
{
    public interface IAccountService
    {
        Task<Account> GetAccount(Account account, string action);
        Task<bool> ActionAccount(Account account, string action);
        Task<UserInfoViewModel> GetAccountTask(Account account, string action);
        Task<List<Account>> GetAllAccount(Account account, string action);
        int Count(Account? account, string? action);
        void Save();
        void SaveAsync();
    }
}
