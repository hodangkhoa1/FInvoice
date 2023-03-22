namespace FinvoiceWeb.Models
{
    public class Invoice
    {
        public string IdInvoice { get; set; }
        public string IdAccount { get; set; }
        public string IdInvoiceForm { get; set; }
        public string Series { get; set; }
        public string InvoiceNo { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public string PaymentMethod { get; set; }
        public float TaxtRate { get; set; }
        public decimal VatAmount { get; set; }
        public decimal TotalPayment { get; set; }
        public DateTime ImportedDate { get; set; }
        public decimal SubTotal { get; set; }
        public byte[] Source { get; set; }
        public InvoiceForm invoiceForm { get; set; }
        public SellerInvoice sellerInvoice { get; set; }
        public BuyerInvoice buyerInvoice { get; set; }
        public List<ItemInvoice> itemInvoiceList { get; set; }
    }
}
