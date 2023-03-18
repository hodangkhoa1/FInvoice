using FinvoiceWeb.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Text;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace FinvoiceWeb.Controllers
{
    public class RegisterController : Controller
    {
        private readonly string _VALUE_LOGIN = "VALUE_LOGIN";
        private readonly string _USER_EMAIL = "USER_EMAIL";
        private string errorMessage = "";

        [HttpGet]
        public IActionResult Register()
        {
            ViewData[_VALUE_LOGIN] = "VALUE_REGISTER";
            return View("~/Pages/Login/Index.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(Account account)
        {
            bool hasError = false;

            if (account.FullName == null && account.Email == null && account.Password == null && account.ConfirmPassword == null)
            {
                hasError = true;
                ModelState.AddModelError("FullName", "Please enter your full name!");
                ModelState.AddModelError("Email", "Please enter your email!");
                ModelState.AddModelError("Password", "Please enter your password!");
                ModelState.AddModelError("ConfirmPassword", "Please enter your confirm password!");
            }
            else if (account.FullName == null)
            {
                hasError = true;
                ModelState.AddModelError("FullName", "Please enter your full name!");
            }
            else if (account.Email == null)
            {
                hasError = true;
                ModelState.AddModelError("Email", "Please enter your email!");
            }
            else if (account.Password == null)
            {
                hasError = true;
                ModelState.AddModelError("Password", "Please enter your password!");
            }
            else if (account.ConfirmPassword == null)
            {
                hasError = true;
                ModelState.AddModelError("ConfirmPassword", "Please enter your confirm password!");
            }
            else
            {
                var accountAPI = new AccountAPI()
                {
                    FullName = account.FullName,
                    Email = account.Email,
                    Password = account.Password,
                    ConfirmPassword = account.ConfirmPassword,
                };

                using (var httpClient = new HttpClient())
                {
                    StringContent stringContent = new(JsonConvert.SerializeObject(accountAPI), Encoding.UTF8, "application/json");

                    using (var response = await httpClient.PostAsync("https://localhost:7050/api/Auth/SignUpAccount", stringContent))
                    {
                        if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                        {
                            hasError = true;
                            errorMessage = await response.Content.ReadAsStringAsync();
                        }
                    }

                }
            }

            if (hasError)
            {
                ViewData[_VALUE_LOGIN] = "VALUE_REGISTER";
                APIResultToken apiResult = JsonConvert.DeserializeObject<APIResultToken>(errorMessage);
                ModelState.AddModelError("Email", apiResult.ErrorMessage);
            }
            else
            {
                TempData[_USER_EMAIL] = account.Email;
                return RedirectToAction("Index", "PinCode");
            }

            return View("~/Pages/Login/Index.cshtml", account);
        }
    }
}
