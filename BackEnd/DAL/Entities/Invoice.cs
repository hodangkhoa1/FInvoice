namespace DAL.Entities
{
    public class Invoice
    {
        public string IdInvoice { get; set; }
        public string Series { get; set; }
        public string InvoiceNo { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public string PaymentMethod { get; set; }
        public float TaxtRate { get; set; }
        public decimal VatAmount { get; set; }
        public decimal TotalPayment { get; set; }
        public DateTime ImportedDate { get; set; }
        public byte[] Source { get; set; }
        public decimal SubTotal { get; set; }
        public int Status { get; set; }

        public string IdAccount { get; set; }
        public virtual Account Account { get; set; }
        public string IdInvoiceForm { get; set; }
        public virtual InvoiceForm InvoiceForm { get; set; }

        public virtual IEnumerable<Buyer> Buyers { get; set; }
        public virtual IEnumerable<Seller> Sellers { get; set; }
        public virtual IEnumerable<Item> Items { get; set; }
    }
}
