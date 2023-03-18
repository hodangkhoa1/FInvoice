namespace DAL.Entities
{
    public class Item
    {
        public string IdItem { get; set; }
        public string Name { get; set; }    
        public string Unit { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Amount { get; set; }
        public string IdInvoice { get; set; }
        public virtual Invoice Invoice { get; set; }
    }
}
