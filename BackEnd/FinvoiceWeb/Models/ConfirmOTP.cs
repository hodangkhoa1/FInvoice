using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace FinvoiceWeb.Models
{
    public class ConfirmOTP
    {
        [Required(ErrorMessage = "Please enter your otp code!")]
        [StringLength(6, MinimumLength = 0, ErrorMessage = "The length of otp code is from 0 to 6 characters!")]
        [Display(Name = "Otp Code")]
        public string OtpCode { get; set; }
    }
}
