namespace DAL.Entities
{
    public class Buyer
    {
        public string IdBuyer { get; set; }
        public Invoice Invoice { get; set; }
        public string Name { get; set; }
        public string Companyname { get; set; }
        public string TaxCode { get; set; }
        public string Address { get; set; }
        public string AccountBanking { get; set; }
        public string BankingName { get; set; }
    }
}
