using AutoMapper;
using BAL.Models;
using BAL.Services.Interfaces;
using BAL.Utils;
using DAL.Entities;
using DAL.Infrastructure;
using DAL.Repositories.Interfaces;

namespace BAL.Services.Implements
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IRepository<Account> _accountRepository;

        public AccountService(IUnitOfWork unitOfWork, IMapper mapper, IRepository<Account> accountRepository)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _accountRepository = accountRepository;
        }

        public async Task<bool> ActionAccount(Account account, string action)
        {
            Account getAccount = new();
            bool check = false;

            switch (action)
            {
                case "SendOTP":
                    getAccount = await _accountRepository.Get(account, "FindByEmail");

                    if (getAccount != null)
                    {
                        getAccount.OtpCode = FunctionRandom.RandomCode(6);
                        getAccount.OtpCodeTimeOut = DateTime.Now.AddMinutes(10);

                        bool checkUpdateCodeOtp = await _accountRepository.ActionEdit(getAccount, "EditProfile");

                        if (checkUpdateCodeOtp == true)
                        {
                            string bodyEmail = "<html><section class=\"confirm-email\"\n"
                            + "        style=\"margin: auto; width: 700px; height: 100vh; display: flex; align-items: center; font-family: sans-serif; font-weight: 500;\">\n"
                            + "        <div class=\"container\"\n"
                            + "            style=\"overflow: hidden; box-shadow: rgba(100, 100, 111, 0.2) 0px 7px 29px 0px;border-radius: 5px;\">\n"
                            + "            <div class=\"confirm-img\">\n"
                            + "                <img style=\"height: 300px;width: 700px; object-fit: cover;\"\n"
                            + "                    src=\"https://blog.trello.com/hubfs/They-Use-Email-You-Use-Trello-final.png\" alt=\"Xin chao\">\n"
                            + "            </div>\n"
                            + "\n"
                            + "            <div class=\"confirm-content\" style=\"margin-bottom: 2rem; padding: 0 1rem;\">\n"
                            + "                <div class=\"content\" style=\"margin-bottom: 3rem;\">\n"
                            + "                    <h1 style=\"padding-top: 5px; color: #6D5FCF; text-align: center; font-size: 38px;\">Email Confirmation</h1>\n"
                            + "                    <p style=\"padding-top: 15px; font-size: 17px; \">Dear <span\n"
                            + "                            style=\"color: #7D8EF0;\">" + getAccount.FullName + "</span></p>\n"
                            + "                    <p style=\"font-size: 15px;\">Your code: " + getAccount.OtpCode + "</p>\n"
                            + "                    <p style=\"font-size: 15px;\">Thank you for creating a FInvoice account.</p>\n"
                            + "                    <p style=\"font-size: 15px;\">Please click the button below to complete the registration process.</p>\n"
                            + "                    <p style=\"font-size: 15px;\">Failure to confirm your email account within 10 minutes will result in\n"
                            + "                        account deletion.\n"
                            + "                        If so, you will have to start the membership registration process again and receive a new\n"
                            + "                        confirmation email.</p>\n"
                            + "                </div>\n"
                            + "            </div>\n"
                            + "        </div>\n"
                            + "    </section></html>";

                            await FunctionSendMail.SendEmail(getAccount.Email, "Active Account", bodyEmail);
                            check = true;
                        }
                    }
                    break;
                case "AddAccount":
                    getAccount = await _accountRepository.Get(account, "FindByEmail");

                    if (getAccount == null)
                    {
                        account.Password = BCrypt.Net.BCrypt.HashPassword(account.Password);

                        check = await _accountRepository.ActionEdit(account, "AddAccount");
                    }

                    break;
                case "ResetPassword":
                    getAccount = await _accountRepository.Get(account, "FindByEmail");

                    if (getAccount != null)
                    {
                        getAccount.Password = BCrypt.Net.BCrypt.HashPassword(account.Password);
                        getAccount.OtpCode = null;
                        getAccount.OtpCodeTimeOut = null;

                        check = await _accountRepository.ActionEdit(getAccount, "EditProfile");
                    }
                    break;
                case "ConfirmOTP":
                    if (account != null)
                    {
                        getAccount = account;

                        getAccount.OtpCode = null;
                        getAccount.OtpCodeTimeOut = null;

                        check = await _accountRepository.ActionEdit(getAccount, "EditProfile");
                    }
                    break;
                case "EditProfile":
                    getAccount = await _accountRepository.Get(account, "FindByEmail");

                    if (getAccount != null)
                    {
                        getAccount.FullName = account.FullName;
                        getAccount.DateOfBirth = account.DateOfBirth;
                        getAccount.Address = account.Address;
                        getAccount.Phone = account.Phone;
                        getAccount.Gender = account.Gender;
                        getAccount.Avatar = account.Avatar;

                        check = await _accountRepository.ActionEdit(getAccount, "EditProfile");
                    }
                    break;
                case "ChangePassword":
                    getAccount = await _accountRepository.Get(account, "FindByEmail");

                    if (getAccount != null)
                    {
                        getAccount.Password = BCrypt.Net.BCrypt.HashPassword(account.Password);

                        check = await _accountRepository.ActionEdit(getAccount, "EditProfile");
                    }
                    break;
                case "EditStatus":
                    getAccount = await _accountRepository.Get(account, "GetByID");

                    if (getAccount != null)
                    {
                        getAccount.Status = account.Status;
                        check = await _accountRepository.ActionEdit(getAccount, "EditProfile");
                    }
                    break;
            }

            return check;
        }

        public int Count(Account? account, string? action)
        {
            return _accountRepository.Count(account, action);
        }

        public async Task<Account> GetAccount(Account account, string action)
        {
            Account result = new();

            switch (action)
            {
                case "Login":
                    var getAccountByEmail = await _accountRepository.Get(account, "FindByEmail");

                    if (getAccountByEmail != null)
                    {
                        bool isValidPassword = BCrypt.Net.BCrypt.Verify(account.Password, getAccountByEmail.Password);

                        if (isValidPassword)
                        {
                            if (getAccountByEmail.LoginTimeOut < DateTime.Now)
                            {
                                getAccountByEmail.LoginAttemps = 0;
                                getAccountByEmail.LoginTimeOut = DateTime.Now.AddDays(12);

                                await _accountRepository.ActionEdit(getAccountByEmail, "EditProfile");
                            }

                            result = getAccountByEmail;
                        }
                        else
                        {
                            getAccountByEmail.LoginAttemps += 1;
                            await _accountRepository.ActionEdit(getAccountByEmail, "EditProfile");
                        }

                        if (getAccountByEmail.LoginAttemps == 3)
                        {
                            getAccountByEmail.LoginTimeOut = DateTime.Now.AddMinutes(15);

                            var checkUpdateAccount = await _accountRepository.ActionEdit(getAccountByEmail, "EditProfile");

                            if (checkUpdateAccount == true)
                            {
                                result = getAccountByEmail;
                            }
                        }

                        if (getAccountByEmail.LoginAttemps > 3)
                        {
                            result = getAccountByEmail;
                        }
                    }
                    break;
                case "GetByEmail":
                    result = await _accountRepository.Get(account, "FindByEmail");
                    break;
                case "GetByID":
                    result = await _accountRepository.Get(account, "GetByID");
                    break;
            }

            return result;
        }

        public async Task<UserInfoViewModel> GetAccountTask(Account account, string action)
        {
            UserInfoViewModel userInfoViewModel = new();

            switch (action)
            {
                case "GetByID":
                    userInfoViewModel = _mapper.Map<UserInfoViewModel>(await _accountRepository.Get(account, "GetByID"));
                    break;
            }

            return userInfoViewModel;
        }

        public async Task<List<Account>> GetAllAccount(Account account, string action)
        {
            List<Account> result = new();

            switch (action)
            {
                case "PagingAccount":
                    result = await _accountRepository.GetAll(account, "GetAllAccountWithRole");
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
