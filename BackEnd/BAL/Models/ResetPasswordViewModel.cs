namespace BAL.Models
{
    public class ResetPasswordViewModel
    {
        public string UserEmail { get; set; }
        public string NewPassword { get; set; }
        public string ComrfirmPassword { get; set; }
    }
}
