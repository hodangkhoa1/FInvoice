using AutoMapper;
using BAL.Models;
using BAL.Services.Interfaces;
using BAL.Utils;
using DAL;
using DAL.Entities;
using DAL.Infrastructure;
using DAL.Repositories.Implements;
using DAL.Repositories.Interfaces;
using System.Security.Principal;

namespace BAL.Services.Implements
{
    public class AccountingSoftwareService : IAccountingSoftwareService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IRepository<AccountingSoftware> _accountingSoftwareRepository;

        public AccountingSoftwareService(IUnitOfWork unitOfWork, IMapper mapper, IRepository<AccountingSoftware> accountingSoftwareRepository)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _accountingSoftwareRepository = accountingSoftwareRepository;
        }

        public async Task<bool> ActionAccountingSoftware(AccountingSoftware accountingSoftware, string action)
        {
            AccountingSoftware getAccountingSoftware = new();
            bool check = false;

            switch (action)
            {
                case "AddAccountingSoftware":
                    getAccountingSoftware = await _accountingSoftwareRepository.Get(accountingSoftware, "GetByName");

                    if (getAccountingSoftware == null)
                    {
                        check = await _accountingSoftwareRepository.ActionEdit(accountingSoftware, "AddAccountingSoftware");
                    }
                    else
                    {
                        throw new Exception("Accounting Software already exists!");
                    }

                    break;
                case "EditAccountingSoftware":
                    getAccountingSoftware = await _accountingSoftwareRepository.Get(accountingSoftware, "GetByID");

                    if (getAccountingSoftware != null)
                    {
                        getAccountingSoftware.Name = accountingSoftware.Name;
                        getAccountingSoftware.Logo = accountingSoftware.Logo;

                        var checkAccountingSoftwareExist = await _accountingSoftwareRepository.Get(getAccountingSoftware, "GetByName");
                        
                        if (checkAccountingSoftwareExist == null)
                        {
                            check = await _accountingSoftwareRepository.ActionEdit(getAccountingSoftware, "EditAccountingSoftware");
                        }
                        else
                        {
                            throw new Exception("Accounting Software name already exist in the system!");
                        }
                    }
                    else
                    {
                        throw new Exception("Accounting Software does not exist in the system!");
                    }
                    break;
                case "EditStatus":
                    getAccountingSoftware = await _accountingSoftwareRepository.Get(accountingSoftware, "GetByID");

                    if (getAccountingSoftware != null)
                    {
                        getAccountingSoftware.Status = accountingSoftware.Status;
                        check = await _accountingSoftwareRepository.ActionEdit(getAccountingSoftware, "EditAccountingSoftware");
                    }
                    else
                    {
                        throw new Exception("Accounting Software does not exist in the system!");
                    }
                    break;
            }

            return check;
        }

        public int Count(AccountingSoftware? accountingSoftware, string? action)
        {
            return _accountingSoftwareRepository.Count(accountingSoftware, action);
        }

        public async Task<AccountingSoftware> GetAccountingSoftware(AccountingSoftware accountingSoftware, string action)
        {
            AccountingSoftware result = new();

            switch (action)
            {
                case "GetByID":
                    result = await _accountingSoftwareRepository.Get(accountingSoftware, "GetByID");
                    break;
            }

            return result;
        }

        public async Task<AccountingSoftwareInfoViewModel> GetAccountingSoftwareTask(AccountingSoftware accountingSoftware, string action)
        {
            AccountingSoftwareInfoViewModel accountingSoftwareInfoViewModel = new();

            switch (action)
            {
                case "GetByID":
                    accountingSoftwareInfoViewModel = _mapper.Map<AccountingSoftwareInfoViewModel>(await _accountingSoftwareRepository.Get(accountingSoftware, "GetByID"));
                    break;
            }

            return accountingSoftwareInfoViewModel;
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
