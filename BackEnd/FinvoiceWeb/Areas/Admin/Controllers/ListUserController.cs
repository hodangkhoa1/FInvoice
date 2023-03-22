using FinvoiceWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace FinvoiceWeb.Areas.Admin.Controllers
{
    public class ListUserController : Controller
    {
        [HttpGet("/admin/list-user")]
        public async Task<IActionResult> Index()
        {
            var accessToken = HttpContext.Session.GetString("JWToken");
            APIResultToken apiResult = JsonConvert.DeserializeObject<APIResultToken>(accessToken);

            HttpClient client = new();
            client.DefaultRequestHeaders.Authorization = new("Bearer", apiResult.Data.AccessToken);
            string jsonString = await client.GetStringAsync("https://localhost:7050/api/User/GetAllListUser");
            List<AccountAPI> accountList = 

            return View("~/Areas/Admin/Views/ListUser/Index.cshtml");
        }
    }
}
