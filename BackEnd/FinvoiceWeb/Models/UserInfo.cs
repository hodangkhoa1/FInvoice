namespace FinvoiceWeb.Models
{
    public class UserInfo
    {
        public string IdAccount { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public char? Gender { get; set; }
        public byte[]? Avatar { get; set; }
        public string TaxCode { get; set; }
        public string ColorAvatar { get; set; }
        public char DefaultAvatar { get; set; }
        public int Status { get; set; }
        public DateTime AccountCreated { get; set; }
    }
}
