using System.ComponentModel.DataAnnotations;

namespace FinvoiceWeb.Models
{
    public class Account
    {
        [Required(ErrorMessage = "Please enter your full name!")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "The length of full name is from 3 to 20 characters!")]
        [Display(Name = "Full name")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Please enter your email!")]
        [EmailAddress(ErrorMessage = "Please enter valid email!")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter your password!"), MinLength(8, ErrorMessage = "Password must be at least 8 characters!")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please enter your confirm password!"), Compare(nameof(Password), ErrorMessage = "Confirm password does not match!")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }
    }
}
