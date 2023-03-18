namespace DAL.Entities
{
    public class InvoiceForm
    {
        public string IdInvoiceForm { get; set; }
        public string CodeForm { get; set; }
        public string NameInvoiceType { get; set; }
        public int Status { get; set; }

        public virtual IEnumerable<Invoice> Invoices { get; set; }
        public virtual IEnumerable<SuppliedInvoiceForm> SuppliedInvoiceForm { get; set; }
    }
}
