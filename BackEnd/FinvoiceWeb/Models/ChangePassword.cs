using System.ComponentModel.DataAnnotations;

namespace FinvoiceWeb.Models
{
    public class ChangePassword
    {
        [Required(ErrorMessage = "Please enter your old password!")]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "Please enter your new password!"), MinLength(8, ErrorMessage = "Password must be at least 8 characters!")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Please enter your confirm password!"), Compare(nameof(NewPassword), ErrorMessage = "Confirm password does not match!")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
