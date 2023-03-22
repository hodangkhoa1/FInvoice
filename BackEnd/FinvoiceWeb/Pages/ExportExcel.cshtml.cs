using FinvoiceWeb.Models;
using FinvoiceWeb.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Text;

namespace FinvoiceWeb.Pages
{
    public class ExportExcelModel : PageModel
    {
        [BindProperty]
        public UploadFile UploadFile { get; set; }

        public void OnGet()
        {
            
        }

        public async Task<IActionResult> OnPostAsync()
        {
            bool hasError = false;
            FileInfo fileInfo = new(UploadFile.File.FileName);
            UserInfo userInfo = SessionHelper.GetObjectFromJson<UserInfo>(HttpContext.Session, "LOGIN_USER");

            if (fileInfo.Extension.Equals(".xml"))
            {
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/files");

                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                string fileNameWithPath = Path.Combine(path, fileInfo.Name);

                using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                {
                    UploadFile.File.CopyTo(stream);
                }

                Invoice invoice = FunctionGetValueXML.GetEasyinvoiceValueXML(fileNameWithPath, userInfo);

                if (invoice != null)
                {
                    invoice.Title = fileInfo.Name;
                    invoice.Source = FunctionEncryptFile.ConvertFileToByte(fileNameWithPath);

                    var accessToken = HttpContext.Session.GetString("JWToken");
                    var urlAPI = "https://localhost:7050/api/XMLUser/ImportXML";
                    HttpClient httpClient = new();
                    APIResultToken apiResult = JsonConvert.DeserializeObject<APIResultToken>(accessToken);
                    httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiResult.Data.AccessToken);
                    StringContent stringContent = new(JsonConvert.SerializeObject(invoice), Encoding.UTF8, "application/json");
                    var response = await httpClient.PostAsync(urlAPI, stringContent);

                    if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    {
                        hasError = true;
                        Console.WriteLine(await response.Content.ReadAsStringAsync());
                    }

                    DirectoryInfo di = new(path);
                    FileInfo[] files = di.GetFiles();
                    foreach (FileInfo file in files)
                    {
                        file.Delete();
                    }
                }
            }

            return Page();
        }
    }
}
