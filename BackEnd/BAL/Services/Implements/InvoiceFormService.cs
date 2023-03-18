using AutoMapper;
using BAL.Services.Interfaces;
using DAL.Entities;
using DAL.Infrastructure;
using DAL.Repositories.Implements;
using DAL.Repositories.Interfaces;

namespace BAL.Services.Implements
{
    public class InvoiceFormService : IInvoiceFormService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IRepository<InvoiceForm> _invoiceFormRepository;

        public InvoiceFormService(IUnitOfWork unitOfWork, IMapper mapper, IRepository<InvoiceForm> invoiceFormRepository)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _invoiceFormRepository = invoiceFormRepository;
        }

        public async Task<bool> ActionInvoiceForm(InvoiceForm invoiceForm, string action)
        {
            InvoiceForm getInvoiceForm = new();
            bool check = false;

            switch (action)
            {
                case "AddInvoiceForm":
                    getInvoiceForm = await _invoiceFormRepository.Get(invoiceForm, "GetByCodeForm");

                    if (getInvoiceForm == null)
                    {
                        check = await _invoiceFormRepository.ActionEdit(invoiceForm, "AddInvoiceForm");
                    }
                    else
                    {
                        throw new Exception("Invoice Form already exists!");
                    }

                    break;
                case "EditInvoiceForm":
                    getInvoiceForm = await _invoiceFormRepository.Get(invoiceForm, "GetByID");

                    if (getInvoiceForm != null)
                    {
                        getInvoiceForm.CodeForm = invoiceForm.CodeForm;
                        getInvoiceForm.NameInvoiceType = invoiceForm.NameInvoiceType;

                        var checkInvoiceFormExist = await _invoiceFormRepository.Get(getInvoiceForm, "GetByCodeForm");

                        if (checkInvoiceFormExist == null)
                        {
                            check = await _invoiceFormRepository.ActionEdit(getInvoiceForm, "EditInvoiceForm");
                        }
                        else
                        {
                            throw new Exception("Invoice Form already exist in the system!");
                        }
                    }
                    else
                    {
                        throw new Exception("Invoice Form does not exist in the system!");
                    }
                    break;
                case "EditStatus":
                    getInvoiceForm = await _invoiceFormRepository.Get(invoiceForm, "GetByID");

                    if (getInvoiceForm != null)
                    {
                        getInvoiceForm.Status = invoiceForm.Status;
                        check = await _invoiceFormRepository.ActionEdit(getInvoiceForm, "EditInvoiceForm");
                    }
                    else
                    {
                        throw new Exception("Invoice Form does not exist in the system!");
                    }
                    break;
            }

            return check;
        }

        public int Count(InvoiceForm? invoiceForm, string? action)
        {
            return _invoiceFormRepository.Count(invoiceForm, action);
        }

        public async Task<InvoiceForm> GetInvoiceForm(InvoiceForm invoiceForm, string action)
        {
            InvoiceForm result = new();

            switch (action)
            {
                case "GetByID":
                    result = await _invoiceFormRepository.Get(invoiceForm, "GetByID");
                    break;
                case "GetByCodeForm":
                    result = await _invoiceFormRepository.Get(invoiceForm, "GetByCodeForm");
                    break;
            }

            return result;
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void SaveAsync()
        {
            _unitOfWork.CommitAsync();
        }
    }
}
