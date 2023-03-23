using BAL.Models;
using DAL.Entities;

namespace BAL.Services.Interfaces
{
    public interface ISupplierService
    {
        Task<Supplier> GetSupplier(Supplier supplier, string action);
        Task<bool> ActionSupplier(Supplier supplier, string action);
        Task<SupplierInfoViewModel> GetSupplierTask(Supplier supplier, string action);
        Task<List<Supplier>> GetAllSupplier(Supplier supplier, string action);
        int Count(Supplier? supplier, string? action);
        void Save();
        void SaveAsync();
    }
}
