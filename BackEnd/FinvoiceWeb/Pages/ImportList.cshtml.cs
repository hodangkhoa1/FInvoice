using FinvoiceWeb.Models;
using FinvoiceWeb.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace FinvoiceWeb.Pages
{
    public class ImportListModel : PageModel
    {
        private readonly string _VALUE_LIST = "VALUE_LIST";

        public async Task<IActionResult> OnGet()
        {
            var accessToken = HttpContext.Session.GetString("JWToken");
            APIResultToken apiResult = JsonConvert.DeserializeObject<APIResultToken>(accessToken);

            UserInfo userInfo = SessionHelper.GetObjectFromJson<UserInfo>(HttpContext.Session, "LOGIN_USER");
            var url = "https://localhost:7050/api/XMLUser/GetProfileUser?userID=" + userInfo.IdAccount;
            HttpClient client = new();
            client.DefaultRequestHeaders.Authorization = new("Bearer", apiResult.Data.AccessToken);
            string jsonString = await client.GetStringAsync(url);
            List<string> apiResultList = JsonConvert.DeserializeObject<List<string>>(jsonString);
            TempData[_VALUE_LIST] = apiResultList;

            return Page();
        }
    }
}
