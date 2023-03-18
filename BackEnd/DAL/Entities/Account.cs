namespace DAL.Entities
{
    public class Account
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
        public string? OtpCode { get; set; }
        public DateTime? OtpCodeTimeOut { get; set; }
        public int LoginAttemps { get; set; }
        public DateTime? LoginTimeOut { get; set; }
        public int Status { get; set; }
        public DateTime AccountCreated { get; set; }

        public string UserRole { get; set; }
        public virtual Role Role { get; set; }

        public virtual IEnumerable<Invoice> Invoices { get; set; }
        public virtual IEnumerable<ExcelResult> ExcelResults { get; set; }
    }
}
