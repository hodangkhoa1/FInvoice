using FinvoiceWeb.Models;
using FinvoiceWeb.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;

namespace FinvoiceWeb.Pages
{
    public class ChangePasswordModel : PageModel
    {
        private readonly string _LINK_LOTTIEFILES = "LINK_LOTTIEFILES";
        private string errorMessage = "";

        public void OnGet()
        {
            ViewData[_LINK_LOTTIEFILES] = "https://unpkg.com/@lottiefiles/lottie-player@latest/dist/lottie-player.js";
        }

        [BindProperty]
        public ChangePassword ChangePassword { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            bool hasError = false;

            if (!ModelState.IsValid)
            {
                ViewData[_LINK_LOTTIEFILES] = "https://unpkg.com/@lottiefiles/lottie-player@latest/dist/lottie-player.js";
                return Page();
            }

            UserInfo userInfo = SessionHelper.GetObjectFromJson<UserInfo>(HttpContext.Session, "LOGIN_USER");
            bool hashPassword = BCrypt.Net.BCrypt.Verify(ChangePassword.OldPassword, userInfo.Password);

            if (hashPassword == false)
            {
                hasError = true;
                ViewData["OldPassword"] = "Old password does not match!";
            }
            else
            {
                var changePasswordAPI = new ChangePasswordAPI()
                {
                    UserEmail = userInfo.Email,
                    NewPassword = ChangePassword.NewPassword,
                    ComrfirmPassword = ChangePassword.ConfirmPassword
                };

                using (var httpClient = new HttpClient())
                {
                    StringContent stringContent = new(JsonConvert.SerializeObject(changePasswordAPI), Encoding.UTF8, "application/json");

                    using (var response = await httpClient.PostAsync("https://localhost:7050/api/Auth/ResetPassword", stringContent))
                    {
                        if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                        {
                            hasError = true;
                            errorMessage = await response.Content.ReadAsStringAsync();
                        }
                    }

                }
            }

            if (hasError == false)
            {
                return RedirectToPage("/profile");
            }
            else
            {
                if (!errorMessage.Equals(""))
                {
                    APIResultToken apiResult = JsonConvert.DeserializeObject<APIResultToken>(errorMessage);
                    if (apiResult != null)
                    {
                        ModelState.AddModelError("OldPassword", apiResult.ErrorMessage);
                        ModelState.AddModelError("NewPassword", apiResult.ErrorMessage);
                        ModelState.AddModelError("ConfirmPassword", apiResult.ErrorMessage);
                    }
                }

                ViewData[_LINK_LOTTIEFILES] = "https://unpkg.com/@lottiefiles/lottie-player@latest/dist/lottie-player.js";
                return Page();
            }
        }
    }
}
