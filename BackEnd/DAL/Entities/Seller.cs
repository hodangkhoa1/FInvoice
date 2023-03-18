namespace DAL.Entities
{
    public class Seller
    {
        public string IdSeller { get; set; }
        public virtual Invoice Invoice { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string TaxCode { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string AccountBanking { get; set; }
        public string BankingName { get; set; }
    }
}
