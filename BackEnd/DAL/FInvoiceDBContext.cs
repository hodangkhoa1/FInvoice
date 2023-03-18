using DAL.DataSeeding;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DAL
{
    public class FInvoiceDBContext : DbContext
    {
        public FInvoiceDBContext()
        {

        }

        public FInvoiceDBContext(DbContextOptions<FInvoiceDBContext> options) : base(options)
        {

        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<AccountingSoftware> AccountingSoftwares { get; set; }
        public DbSet<Buyer> Buyers { get; set; }
        public DbSet<ExcelForm> ExcelForms { get; set; }
        public DbSet<ExcelResult> ExcelResults { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceForm> InvoiceForms { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Seller> Sellers { get; set; }
        public DbSet<SuppliedInvoiceForm> SuppliedInvoiceForms { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfiguration config = new ConfigurationBuilder()
                                            .SetBasePath(Directory.GetCurrentDirectory())
                                            .AddJsonFile("appsettings.json").Build();

                string connectionString = config["ConnectionStrings:DefaultConnection"];
                optionsBuilder.UseSqlServer(connectionString);
            }
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Role
            modelBuilder.Entity<Role>()
                .HasKey(r => r.IdRole);

            modelBuilder.Entity<Role>()
                .Property(r => r.Name)
                .HasColumnType("nvarchar")
                .HasMaxLength(100)
                .IsRequired();
            #endregion

            #region Account
            modelBuilder.Entity<Account>()
                .HasKey(a => a.IdAccount);

            modelBuilder.Entity<Account>()
                .Property(a => a.FullName)
                .HasColumnType("nvarchar")
                .HasMaxLength(100)
                .IsRequired();

            modelBuilder.Entity<Account>()
                .Property(a => a.Email)
                .HasColumnType("varchar")
                .HasMaxLength(100)
                .IsRequired();

            modelBuilder.Entity<Account>()
                .Property(a => a.Password)
                .HasColumnType("nvarchar(max)")
                .IsRequired();

            modelBuilder.Entity<Account>()
                .Property(a => a.DateOfBirth)
                .HasColumnType("date")
                .IsRequired(false);

            modelBuilder.Entity<Account>()
                .Property(a => a.Address)
                .HasColumnType("nvarchar")
                .HasMaxLength(150)
                .IsRequired(false);

            modelBuilder.Entity<Account>()
                .Property(a => a.Phone)
                .HasColumnType("char")
                .HasMaxLength(10)
                .IsRequired(false);

            modelBuilder.Entity<Account>()
                .Property(a => a.Gender)
                .HasColumnType("char")
                .IsRequired(false);

            modelBuilder.Entity<Account>()
                .Property(a => a.Avatar)
                .IsRequired(false);

            modelBuilder.Entity<Account>()
                .Property(a => a.TaxCode)
                .HasColumnType("varchar")
                .HasMaxLength(30);

            modelBuilder.Entity<Account>()
                .Property(a => a.DefaultAvatar)
                .HasColumnType("char")
                .IsRequired();

            modelBuilder.Entity<Account>()
                .Property(a => a.ColorAvatar)
                .HasColumnType("varchar")
                .HasMaxLength(10)
                .IsRequired();

            modelBuilder.Entity<Account>()
                .Property(a => a.OtpCode)
                .HasColumnType("varchar")
                .HasMaxLength(10);

            modelBuilder.Entity<Account>()
                .Property(a => a.OtpCodeTimeOut)
                .HasColumnType("datetime2");

            modelBuilder.Entity<Account>()
                .Property(a => a.LoginAttemps)
                .HasColumnType("int");

            modelBuilder.Entity<Account>()
                .Property(a => a.LoginTimeOut)
                .HasColumnType("datetime2");

            modelBuilder.Entity<Account>()
                .Property(a => a.Status)
                .HasColumnType("int")
                .HasDefaultValue(0)
                .IsRequired();

            modelBuilder.Entity<Account>()
                .HasOne<Role>(a => a.Role)
                .WithMany(r => r.Accounts)
                .HasForeignKey(r => r.UserRole);

            modelBuilder.Entity<Account>()
                .Property(a => a.AccountCreated)
                .HasColumnType("datetime")
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .IsRequired();
            #endregion

            #region ExcelResult
            modelBuilder.Entity<ExcelResult>()
                .HasKey(er => er.IdExcelResult);

            modelBuilder.Entity<ExcelResult>()
                .Property(er => er.ExportedDate)
                .HasColumnType("datetime")
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .IsRequired();

            modelBuilder.Entity<ExcelResult>()
                .Property(er => er.Source)
                .IsRequired(false);

            modelBuilder.Entity<ExcelResult>()
                .HasOne<Account>(er => er.Account)
                .WithMany(a => a.ExcelResults)
                .HasForeignKey(er => er.IdAccount);

            modelBuilder.Entity<ExcelResult>()
                .HasOne<ExcelForm>(er => er.ExcelForm)
                .WithMany(ef => ef.ExcelResults)
                .HasForeignKey(er => er.IdExcelForm);
            #endregion

            #region ExcelForm
            modelBuilder.Entity<ExcelForm>()
                .HasKey(ef => ef.IdExcelForm);

            modelBuilder.Entity<ExcelForm>()
                .Property(ef => ef.Name)
                .HasColumnType("nvarchar")
                .HasMaxLength(100)
                .IsRequired();

            modelBuilder.Entity<ExcelForm>()
                .Property(ef => ef.Source)
                .IsRequired(false);

            modelBuilder.Entity<ExcelForm>()
                .Property(ef => ef.ModifiedDate)
                .HasColumnType("datetime")
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .IsRequired();

            modelBuilder.Entity<ExcelForm>()
                .HasOne<AccountingSoftware>(ef => ef.AccountingSoftware)
                .WithMany(a => a.ExcelForms)
                .HasForeignKey(ef => ef.IdAccountingSoftware);
            #endregion

            #region AccountingSoftware
            modelBuilder.Entity<AccountingSoftware>()
                .HasKey(a => a.IdAccountingSoftware);

            modelBuilder.Entity<AccountingSoftware>()
                .Property(a => a.Name)
                .HasColumnType("nvarchar")
                .HasMaxLength(100)
                .IsRequired();

            modelBuilder.Entity<AccountingSoftware>()
                .Property(a => a.Logo);

            modelBuilder.Entity<AccountingSoftware>()
                .Property(a => a.ColorLogo)
                .HasColumnType("varchar")
                .HasMaxLength(10)
                .IsRequired();

            modelBuilder.Entity<AccountingSoftware>()
                .Property(a => a.DefaultLogo)
                .HasColumnType("char")
                .IsRequired();

            modelBuilder.Entity<AccountingSoftware>()
                .Property(a => a.Status)
                .HasColumnType("int")
                .IsRequired();

            modelBuilder.Entity<AccountingSoftware>()
                .Property(a => a.AccountingSoftwareCreated)
                .HasColumnType("datetime")
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .IsRequired();
            #endregion

            #region Invoice
            modelBuilder.Entity<Invoice>()
                .HasKey(i => i.IdInvoice);

            modelBuilder.Entity<Invoice>()
                .Property(i => i.Series)
                .HasColumnType("varchar")
                .HasMaxLength(100);

            modelBuilder.Entity<Invoice>()
                .Property(i => i.InvoiceNo)
                .HasColumnType("varchar")
                .HasMaxLength(100);

            modelBuilder.Entity<Invoice>()
                .Property(i => i.Title)
                .HasColumnType("nvarchar(max)");

            modelBuilder.Entity<Invoice>()
                .Property(i => i.Date)
                .HasColumnType("datetime");

            modelBuilder.Entity<Invoice>()
                .Property(i => i.PaymentMethod)
                .HasColumnType("nvarchar(max)");

            modelBuilder.Entity<Invoice>()
                .Property(i => i.TaxtRate)
                .HasColumnType("float");

            modelBuilder.Entity<Invoice>()
                .Property(i => i.VatAmount)
                .HasColumnType("decimal");

            modelBuilder.Entity<Invoice>()
                .Property(i => i.TotalPayment)
                .HasColumnType("decimal");

            modelBuilder.Entity<Invoice>()
                .Property(i => i.ImportedDate)
                .HasColumnType("datetime")
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .IsRequired();

            modelBuilder.Entity<Invoice>()
                .Property(i => i.Source);

            modelBuilder.Entity<Invoice>()
                .Property(i => i.SubTotal)
                .HasColumnType("decimal");

            modelBuilder.Entity<Invoice>()
                .Property(i => i.Status)
                .HasColumnType("int")
                .HasDefaultValue(0)
                .IsRequired();

            modelBuilder.Entity<Invoice>()
                .HasOne<Account>(i => i.Account)
                .WithMany(a => a.Invoices)
                .HasForeignKey(i => i.IdAccount);

            modelBuilder.Entity<Invoice>()
                .HasOne<InvoiceForm>(i => i.InvoiceForm)
                .WithMany(i => i.Invoices)
                .HasForeignKey(i => i.IdInvoiceForm);
            #endregion

            #region Buyer
            modelBuilder.Entity<Buyer>()
                .HasKey(b => b.IdBuyer);

            modelBuilder.Entity<Buyer>()
                .HasOne<Invoice>(b => b.Invoice)
                .WithMany(i => i.Buyers)
                .HasForeignKey(b => b.IdBuyer);

            modelBuilder.Entity<Buyer>()
                .Property(b => b.Name)
                .HasColumnType("nvarchar")
                .HasMaxLength(100);

            modelBuilder.Entity<Buyer>()
                .Property(b => b.Companyname)
                .HasColumnType("nvarchar")
                .HasMaxLength(100);

            modelBuilder.Entity<Buyer>()
                .Property(b => b.TaxCode)
                .HasColumnType("varchar")
                .HasMaxLength(10);

            modelBuilder.Entity<Buyer>()
                .Property(b => b.Address)
                .HasColumnType("nvarchar")
                .HasMaxLength(150);

            modelBuilder.Entity<Buyer>()
                .Property(b => b.AccountBanking)
                .HasColumnType("varchar")
                .HasMaxLength(15);

            modelBuilder.Entity<Buyer>()
                .Property(b => b.BankingName)
                .HasColumnType("nvarchar")
                .HasMaxLength(150);
            #endregion

            #region Item
            modelBuilder.Entity<Item>()
                .HasKey(i => i.IdItem);

            modelBuilder.Entity<Item>()
                .Property(i => i.Name)
                .HasColumnType("nvarchar")
                .HasMaxLength(100);

            modelBuilder.Entity<Item>()
                .Property(i => i.Unit)
                .HasColumnType("nvarchar")
                .HasMaxLength(50);

            modelBuilder.Entity<Item>()
                .Property(i => i.Quantity)
                .HasColumnType("int");

            modelBuilder.Entity<Item>()
                .Property(i => i.UnitPrice)
                .HasColumnType("decimal");

            modelBuilder.Entity<Item>()
                .Property(i => i.Amount)
                .HasColumnType("decimal");

            modelBuilder.Entity<Item>()
                .HasOne<Invoice>(i => i.Invoice)
                .WithMany(i => i.Items)
                .HasForeignKey(i => i.IdInvoice);
            #endregion

            #region Seller
            modelBuilder.Entity<Seller>()
                .HasKey(s => s.IdSeller);

            modelBuilder.Entity<Seller>()
                .HasOne<Invoice>(s => s.Invoice)
                .WithMany(i => i.Sellers)
                .HasForeignKey(s => s.IdSeller);

            modelBuilder.Entity<Seller>()
                .Property(s => s.Name)
                .HasColumnType("nvarchar")
                .HasMaxLength(100);

            modelBuilder.Entity<Seller>()
                .Property(s => s.Phone)
                .HasColumnType("char")
                .HasMaxLength(10);

            modelBuilder.Entity<Seller>()
                .Property(s => s.TaxCode)
                .HasColumnType("varchar")
                .HasMaxLength(10);

            modelBuilder.Entity<Seller>()
                .Property(s => s.Address)
                .HasColumnType("nvarchar")
                .HasMaxLength(150);

            modelBuilder.Entity<Seller>()
                .Property(s => s.Email)
                .HasColumnType("varchar")
                .HasMaxLength(100);

            modelBuilder.Entity<Seller>()
                .Property(s => s.AccountBanking)
                .HasColumnType("varchar")
                .HasMaxLength(15);

            modelBuilder.Entity<Seller>()
                .Property(s => s.BankingName)
                .HasColumnType("nvarchar")
                .HasMaxLength(150);
            #endregion

            #region InvoiceForm
            modelBuilder.Entity<InvoiceForm>()
                .HasKey(f => f.IdInvoiceForm);

            modelBuilder.Entity<InvoiceForm>()
                .Property(f => f.CodeForm)
                .HasColumnType("varchar")
                .HasMaxLength(100);

            modelBuilder.Entity<InvoiceForm>()
                .Property(f => f.NameInvoiceType)
                .HasColumnType("nvarchar")
                .HasMaxLength(100);

            modelBuilder.Entity<InvoiceForm>()
                .Property(f => f.Status)
                .HasColumnType("int")
                .HasDefaultValue(0)
                .IsRequired();
            #endregion

            #region SuppliedInvoiceForm
            modelBuilder.Entity<SuppliedInvoiceForm>()
                .HasKey(si => si.IdSuppliedInvoiceForm);

            modelBuilder.Entity<SuppliedInvoiceForm>()
                .Property(si => si.Source);

            modelBuilder.Entity<SuppliedInvoiceForm>()
                .HasOne<InvoiceForm>(f => f.InvoiceForm)
                .WithMany(si => si.SuppliedInvoiceForm)
                .HasForeignKey(si => si.IdInvoiceForm);

            modelBuilder.Entity<SuppliedInvoiceForm>()
                .HasOne<Supplier>(s => s.Supplier)
                .WithMany(si => si.SuppliedInvoiceForms)
                .HasForeignKey(si => si.IdSupplier);
            #endregion

            #region Supplier
            modelBuilder.Entity<Supplier>()
                .HasKey(s => s.IdSupplier);

            modelBuilder.Entity<Supplier>()
                .Property(s => s.Name)
                .HasColumnType("nvarchar")
                .HasMaxLength(100);

            modelBuilder.Entity<Supplier>()
                .Property(s => s.Logo);

            modelBuilder.Entity<Supplier>()
                .Property(s => s.ColorLogo)
                .HasColumnType("varchar")
                .HasMaxLength(10)
                .IsRequired();

            modelBuilder.Entity<Supplier>()
                .Property(s => s.DefaultLogo)
                .HasColumnType("char")
                .IsRequired();

            modelBuilder.Entity<Supplier>()
                .Property(s => s.Status)
                .HasColumnType("int")
                .IsRequired();

            modelBuilder.Entity<Supplier>()
                .Property(s => s.SupplierCreated)
                .HasColumnType("datetime")
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .IsRequired();
            #endregion

            #region Seed Data
            modelBuilder.SeedRole();
            #endregion
        }
    }
}
