using FinvoiceWeb.Utils;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace FinvoiceWeb.Controllers
{
    public class LogoutController : Controller
    {
        [HttpGet("/logout")]
        public IActionResult Index()
        {
            HttpContext.Session.Remove("JWToken");
            HttpContext.Session.Clear();
            return RedirectToPage("/Index");
        }
    }
}
