using FinvoiceWeb.Areas.Admin.Models;
using FinvoiceWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace FinvoiceWeb.Areas.Admin.Controllers
{
    public class ListUserController : Controller
    {
        private static readonly string LIST_USER = "LIST_USER";
        private static readonly string END_PAGE = "END_PAGE";
        private static readonly string CURRENT_PAGE = "CURRENT_PAGE";

        [HttpGet("/admin/list-user")]
        public async Task<IActionResult> Index()
        {
            var accessToken = HttpContext.Session.GetString("JWToken");
            APIResultToken apiResult = JsonConvert.DeserializeObject<APIResultToken>(accessToken);

            HttpClient client = new();
            client.DefaultRequestHeaders.Authorization = new("Bearer", apiResult.Data.AccessToken);
            string jsonString = await client.GetStringAsync("https://localhost:7050/api/User/GetAllListUser");
            APIResultPagingList aPIResultPagingList = JsonConvert.DeserializeObject<APIResultPagingList>(jsonString);

            TempData[LIST_USER] = aPIResultPagingList.Data;
            TempData[END_PAGE] = aPIResultPagingList.EndPage;
            TempData[CURRENT_PAGE] = aPIResultPagingList.CurrentPage;

            return View("~/Areas/Admin/Views/ListUser/Index.cshtml");
        }
    }
}
