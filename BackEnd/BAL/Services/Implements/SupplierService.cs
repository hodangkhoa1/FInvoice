using AutoMapper;
using BAL.Models;
using BAL.Services.Interfaces;
using DAL.Entities;
using DAL.Infrastructure;
using DAL.Repositories.Implements;
using DAL.Repositories.Interfaces;

namespace BAL.Services.Implements
{
    public class SupplierService : ISupplierService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IRepository<Supplier> _supplierRepository;

        public SupplierService(IUnitOfWork unitOfWork, IMapper mapper, IRepository<Supplier> supplierRepository)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _supplierRepository = supplierRepository;
        }

        public async Task<bool> ActionSupplier(Supplier supplier, string action)
        {
            Supplier getSupplier = new();
            bool check = false;

            switch (action)
            {
                case "AddSupplier":
                    getSupplier = await _supplierRepository.Get(supplier, "GetByName");

                    if (getSupplier == null)
                    {
                        check = await _supplierRepository.ActionEdit(supplier, "AddSupplier");
                    }
                    else
                    {
                        throw new Exception("Supplier already exists!");
                    }

                    break;
                case "EditSupplier":
                    getSupplier = await _supplierRepository.Get(supplier, "GetByID");

                    if (getSupplier != null)
                    {
                        getSupplier.Name = supplier.Name;
                        getSupplier.Logo = supplier.Logo;

                        var checkSupplierExist = await _supplierRepository.Get(getSupplier, "GetByName");

                        if (checkSupplierExist == null)
                        {
                            check = await _supplierRepository.ActionEdit(getSupplier, "EditSupplier");
                        }
                        else
                        {
                            throw new Exception("Supplier name already exist in the system!");
                        }
                    }
                    else
                    {
                        throw new Exception("Supplier does not exist in the system!");
                    }

                    break;
                case "EditStatus":
                    getSupplier = await _supplierRepository.Get(supplier, "GetByID");

                    if (getSupplier != null)
                    {
                        getSupplier.Status = supplier.Status;
                        check = await _supplierRepository.ActionEdit(getSupplier, "EditSupplier");
                    }
                    else
                    {
                        throw new Exception("Supplier does not exist in the system!");
                    }

                    break;
            }

            return check;
        }

        public int Count(Supplier? supplier, string? action)
        {
            return _supplierRepository.Count(supplier, action);
        }

        public async Task<Supplier> GetSupplier(Supplier supplier, string action)
        {
            Supplier result = new();

            switch (action)
            {
                case "GetByID":
                    result = await _supplierRepository.Get(supplier, "GetByID");
                    break;
            }

            return result;
        }

        public async Task<SupplierInfoViewModel> GetSupplierTask(Supplier supplier, string action)
        {
            SupplierInfoViewModel supplierInfoViewModel = new();

            switch (action)
            {
                case "GetByID":
                    supplierInfoViewModel = _mapper.Map<SupplierInfoViewModel>(await _supplierRepository.Get(supplier, "GetByID"));
                    break;
            }

            return supplierInfoViewModel;
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
