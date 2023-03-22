using AutoMapper;
using BAL.Services.Interfaces;
using DAL.Entities;
using DAL.Infrastructure;
using DAL.Repositories.Implements;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Services.Implements
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IRepository<Invoice> _invoiceRepository;

        public InvoiceService(IUnitOfWork unitOfWork, IMapper mapper, IRepository<Invoice> invoiceRepository)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _invoiceRepository = invoiceRepository;
        }

        public async Task<bool> ActionInvoice(Invoice invoice, string action)
        {
            Invoice getInvoice = new();
            bool check = false;

            switch (action)
            {
                case "AddInvoice":
                    getInvoice = await _invoiceRepository.Get(invoice, "GetByID");

                    if (getInvoice == null)
                    {
                        check = await _invoiceRepository.ActionEdit(invoice, "AddInvoice");
                    }

                    break;
                case "EditStatus":
                    getInvoice = await _invoiceRepository.Get(invoice, "GetByID");

                    if (getInvoice != null)
                    {
                        getInvoice.Status = invoice.Status;
                        check = await _invoiceRepository.ActionEdit(getInvoice, "EditInvoice");
                    }
                    break;
            }

            return check;
        }

        public int Count(Invoice? invoice, string? action)
        {
            throw new NotImplementedException();
        }

        public Task<Invoice> GetInvoice(Invoice invoice, string action)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Invoice>> GetInvoiceList(Invoice invoice, string action)
        {
            List<Invoice> result = new();

            switch (action)
            {
                case "GetAllInvoiceByUserID":
                    result = await _invoiceRepository.GetAll(invoice, "GetAllInvoiceByUserID");
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
