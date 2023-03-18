namespace BAL.Models
{
    public class SupplierInfoViewModel
    {
        public string IdSupplier { get; set; }
        public string Name { get; set; }
        public byte[]? Logo { get; set; }
        public string ColorLogo { get; set; }
        public char DefaultLogo { get; set; }
        public int Status { get; set; }
        public DateTime SupplierCreated { get; set; }
    }
}
