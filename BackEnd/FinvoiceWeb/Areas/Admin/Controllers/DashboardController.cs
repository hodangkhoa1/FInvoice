using FinvoiceWeb.Areas.Admin.Models;
using FinvoiceWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Policy;

namespace FinvoiceWeb.Areas.Admin.Controllers
{
    public class DashboardController : Controller
    {
        private static readonly string TOTAL_USER = "TOTAL_USER";
        private static readonly string TOTAL_EXCEL_TEMPLATES = "TOTAL_EXCEL_TEMPLATES";
        private static readonly string TOTAL_SUPPLIERS = "TOTAL_SUPPLIERS";
        private static readonly string TOTAL_ACCOUNTING_SOFTWARE = "TOTAL_ACCOUNTING_SOFTWARE";


        [HttpGet("/admin/dashboard")]
        public async Task<IActionResult> Index()
        {
            var accessToken = HttpContext.Session.GetString("JWToken");
            APIResultToken apiResult = JsonConvert.DeserializeObject<APIResultToken>(accessToken);

            HttpClient client = new();
            client.DefaultRequestHeaders.Authorization = new("Bearer", apiResult.Data.AccessToken);
            string jsonString = await client.GetStringAsync("https://localhost:7050/api/Dashboard/GetProbability");
            DashboardAPI apiResultList = JsonConvert.DeserializeObject<DashboardAPI>(jsonString);

            TempData[TOTAL_USER] = apiResultList.TotalUsers;
            TempData[TOTAL_EXCEL_TEMPLATES] = apiResultList.TotalExcelTemplates;
            TempData[TOTAL_SUPPLIERS] = apiResultList.TotalSuppliers;
            TempData[TOTAL_ACCOUNTING_SOFTWARE] = apiResultList.TotalAccountingSoftwares;

            return View("~/Areas/Admin/Views/Dashboard/Index.cshtml");
        }
    }
}
