using FinvoiceWeb.Models;
using FinvoiceWeb.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FinvoiceWeb.Controllers
{
    public class LoginController : Controller
    {
        private readonly string _LOGIN_USER = "LOGIN_USER";
        private readonly string _LOGIN_ADMIN = "LOGIN_ADMIN";
        private readonly string _VALUE_LOGIN = "VALUE_LOGIN";
        private string token = "";

        [HttpGet("/login")]
        public IActionResult Index()
        {
            ViewData[_VALUE_LOGIN] = "VALUE_LOGIN";
            return View("~/Pages/Login/Index.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(Account loginModel)
        {
            bool hasError = false;

            if (loginModel.Email == null && loginModel.Password == null)
            {
                ModelState.AddModelError("Email", "Please enter your email!");
                ModelState.AddModelError("Password", "Please enter your password!");
                hasError = true;
            }
            else if (loginModel.Email == null)
            {
                ModelState.AddModelError("Email", "Please enter your email!");
                hasError = true;
            }
            else if (loginModel.Password == null)
            {
                ModelState.AddModelError("Password", "Please enter your password!");
                hasError = true;
            }
            else
            {
                var accountAPI = new AccountAPI()
                {
                    Email = loginModel.Email,
                    Password = loginModel.Password,
                    RememberMe = true
                };

                using (var httpClient = new HttpClient())
                {
                    StringContent stringContent = new(JsonConvert.SerializeObject(accountAPI), Encoding.UTF8, "application/json");

                    using (var response = await httpClient.PostAsync("https://localhost:7050/api/Auth/Login", stringContent))
                    {
                        if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                        {
                            hasError = true;
                            token = await response.Content.ReadAsStringAsync();
                        }

                        token = await response.Content.ReadAsStringAsync();
                        HttpContext.Session.SetString("JWToken", token);
                    }

                }

            }

            if (hasError)
            {
                ViewData[_VALUE_LOGIN] = "VALUE_LOGIN";
                APIResultToken apiResult = JsonConvert.DeserializeObject<APIResultToken>(token);
                ModelState.AddModelError("Email", apiResult.ErrorMessage);
            }
            else
            {
                APIResultToken apiResult = JsonConvert.DeserializeObject<APIResultToken>(token);

                var handler = new JwtSecurityTokenHandler();
                var jwtSecurityToken = handler.ReadJwtToken(apiResult.Data.AccessToken);
                var tokenS = jwtSecurityToken as JwtSecurityToken;
                string roleUser = tokenS.Claims.First(claim => claim.Type == ClaimTypes.Role).Value;

                if (roleUser.Equals("User"))
                {
                    var url = "https://localhost:7050/api/User/GetProfileUser?userID=" + tokenS.Claims.First(claim => claim.Type == "UserID").Value;
                    HttpClient client = new();
                    client.DefaultRequestHeaders.Authorization = new("Bearer", apiResult.Data.AccessToken);
                    string jsonString = await client.GetStringAsync(url);
                    APIResultUserInfo apiResultProfile = JsonConvert.DeserializeObject<APIResultUserInfo>(jsonString);

                    SessionHelper.SetObjectAsJson(HttpContext.Session, _LOGIN_USER, apiResultProfile.Data);
                    return RedirectToPage("/Index");
                }
                else if (roleUser.Equals("Admin"))
                {

                }
            }

            return View("~/Pages/Login/Index.cshtml", loginModel);
        }
    }
}
