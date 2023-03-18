namespace DAL.Infrastructure
{
    public interface IDbFactory
    {
        FInvoiceDBContext Init();
    }
}
