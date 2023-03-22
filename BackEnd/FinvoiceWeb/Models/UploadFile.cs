using System.ComponentModel.DataAnnotations;

namespace FinvoiceWeb.Models
{
    public class UploadFile
    {
        [Required(ErrorMessage = "Please select file!")]
        public IFormFile File { get; set; }
    }
}
