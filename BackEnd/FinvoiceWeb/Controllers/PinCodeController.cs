using FinvoiceWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using System.Text;

namespace FinvoiceWeb.Controllers
{
    public class PinCodeController : Controller
    {
        private readonly string _USER_EMAIL = "USER_EMAIL";
        private readonly string _VALUE_LOGIN = "VALUE_LOGIN";
        private string errorMessage = "";

        [HttpGet("/pincode")]
        public IActionResult Index()
        {
            return View("~/Pages/PinCode/Index.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(ConfirmOTP confirmOTP)
        {
            bool hasError = false;

            if (confirmOTP.OtpCode == null)
            {
                ModelState.AddModelError("OtpCode", "Please enter your otp code!");
            }
            else
            {
                var confirmOtpAPI = new ConfirmOtpAPI()
                {
                    UserEmail = TempData[_USER_EMAIL].ToString(),
                    OtpCode = confirmOTP.OtpCode,
                };

                using (var httpClient = new HttpClient())
                {
                    StringContent stringContent = new(JsonConvert.SerializeObject(confirmOtpAPI), Encoding.UTF8, "application/json");

                    using (var response = await httpClient.PostAsync("https://localhost:7050/api/Auth/ConfirmOTP", stringContent))
                    {
                        if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                        {
                            hasError = true;
                            errorMessage = await response.Content.ReadAsStringAsync();
                        }
                    }

                }
            }

            if (hasError == true)
            {
                APIResultToken apiResult = JsonConvert.DeserializeObject<APIResultToken>(errorMessage);
                ModelState.AddModelError("OtpCode", apiResult.ErrorMessage);
                return RedirectToAction("Index", "PinCode");
            }

            ViewData[_VALUE_LOGIN] = "VALUE_LOGIN";
            return View("~/Pages/Login/Index.cshtml");
        }
    }
}
