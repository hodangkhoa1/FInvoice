using BAL.Models;
using DAL.Entities;

namespace BAL.Services.Interfaces
{
    public interface IInvoiceFormService
    {
        Task<InvoiceForm> GetInvoiceForm(InvoiceForm invoiceForm, string action);
        Task<bool> ActionInvoiceForm(InvoiceForm invoiceForm, string action);
        //Task<AccountingSoftwareInfoViewModel> GetAccountingSoftwareTask(AccountingSoftware accountingSoftware, string action);
        int Count(InvoiceForm? invoiceForm, string? action);
        void Save();
        void SaveAsync();
    }
}
