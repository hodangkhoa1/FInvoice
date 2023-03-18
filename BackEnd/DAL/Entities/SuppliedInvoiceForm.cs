namespace DAL.Entities
{
    public class SuppliedInvoiceForm
    {
        public string IdSuppliedInvoiceForm { get; set; }
        public byte[] Source { get; set; }

        public string IdInvoiceForm { get; set; }
        public virtual InvoiceForm InvoiceForm { get; set; }

        public string IdSupplier { get; set; }
        public virtual Supplier Supplier { get; set; }
    }
}
