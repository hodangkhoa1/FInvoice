using DAL.Entities;

namespace BAL.Services.Interfaces
{
    public interface IInvoiceService
    {
        Task<Invoice> GetInvoice(Invoice invoice, string action);
        Task<bool> ActionInvoice(Invoice invoice, string action);
        Task<List<Invoice>> GetInvoiceList(Invoice invoice, string action);
        int Count(Invoice? invoice, string? action);
        void Save();
        void SaveAsync();
    }
}
