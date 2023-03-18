namespace FinvoiceWeb.Models
{
    public class ChangePasswordAPI
    {
        public string UserEmail { get; set; }
        public string NewPassword { get; set; }
        public string ComrfirmPassword { get; set; }
    }
}
