using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AccountingSoftwares",
                columns: table => new
                {
                    IdAccountingSoftware = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Logo = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    ColorLogo = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false),
                    DefaultLogo = table.Column<string>(type: "char(1)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    AccountingSoftwareCreated = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountingSoftwares", x => x.IdAccountingSoftware);
                });

            migrationBuilder.CreateTable(
                name: "InvoiceForms",
                columns: table => new
                {
                    IdInvoiceForm = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CodeForm = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    NameInvoiceType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceForms", x => x.IdInvoiceForm);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    IdRole = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.IdRole);
                });

            migrationBuilder.CreateTable(
                name: "Suppliers",
                columns: table => new
                {
                    IdSupplier = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Logo = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    ColorLogo = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false),
                    DefaultLogo = table.Column<string>(type: "char(1)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    SupplierCreated = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suppliers", x => x.IdSupplier);
                });

            migrationBuilder.CreateTable(
                name: "ExcelForms",
                columns: table => new
                {
                    IdExcelForm = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Source = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    IdAccountingSoftware = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExcelForms", x => x.IdExcelForm);
                    table.ForeignKey(
                        name: "FK_ExcelForms_AccountingSoftwares_IdAccountingSoftware",
                        column: x => x.IdAccountingSoftware,
                        principalTable: "AccountingSoftwares",
                        principalColumn: "IdAccountingSoftware",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    IdAccount = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "date", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Phone = table.Column<string>(type: "char(10)", maxLength: 10, nullable: true),
                    Gender = table.Column<string>(type: "char(1)", nullable: true),
                    Avatar = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    TaxCode = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    ColorAvatar = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false),
                    DefaultAvatar = table.Column<string>(type: "char(1)", nullable: false),
                    OtpCode = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true),
                    OtpCodeTimeOut = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LoginAttemps = table.Column<int>(type: "int", nullable: false),
                    LoginTimeOut = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    AccountCreated = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UserRole = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.IdAccount);
                    table.ForeignKey(
                        name: "FK_Accounts_Roles_UserRole",
                        column: x => x.UserRole,
                        principalTable: "Roles",
                        principalColumn: "IdRole",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SuppliedInvoiceForms",
                columns: table => new
                {
                    IdSuppliedInvoiceForm = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Source = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    IdInvoiceForm = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IdSupplier = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SuppliedInvoiceForms", x => x.IdSuppliedInvoiceForm);
                    table.ForeignKey(
                        name: "FK_SuppliedInvoiceForms_InvoiceForms_IdInvoiceForm",
                        column: x => x.IdInvoiceForm,
                        principalTable: "InvoiceForms",
                        principalColumn: "IdInvoiceForm",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SuppliedInvoiceForms_Suppliers_IdSupplier",
                        column: x => x.IdSupplier,
                        principalTable: "Suppliers",
                        principalColumn: "IdSupplier",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExcelResults",
                columns: table => new
                {
                    IdExcelResult = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ExportedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    Source = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    IdAccount = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IdExcelForm = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExcelResults", x => x.IdExcelResult);
                    table.ForeignKey(
                        name: "FK_ExcelResults_Accounts_IdAccount",
                        column: x => x.IdAccount,
                        principalTable: "Accounts",
                        principalColumn: "IdAccount",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExcelResults_ExcelForms_IdExcelForm",
                        column: x => x.IdExcelForm,
                        principalTable: "ExcelForms",
                        principalColumn: "IdExcelForm",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Invoices",
                columns: table => new
                {
                    IdInvoice = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Series = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    InvoiceNo = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime", nullable: false),
                    PaymentMethod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TaxtRate = table.Column<double>(type: "float", nullable: false),
                    VatAmount = table.Column<decimal>(type: "decimal", nullable: false),
                    TotalPayment = table.Column<decimal>(type: "decimal", nullable: false),
                    ImportedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    Source = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    SubTotal = table.Column<decimal>(type: "decimal", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    IdAccount = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IdInvoiceForm = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoices", x => x.IdInvoice);
                    table.ForeignKey(
                        name: "FK_Invoices_Accounts_IdAccount",
                        column: x => x.IdAccount,
                        principalTable: "Accounts",
                        principalColumn: "IdAccount",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Invoices_InvoiceForms_IdInvoiceForm",
                        column: x => x.IdInvoiceForm,
                        principalTable: "InvoiceForms",
                        principalColumn: "IdInvoiceForm",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdAccount = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    JwtId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsUsed = table.Column<bool>(type: "bit", nullable: false),
                    IsRevoked = table.Column<bool>(type: "bit", nullable: false),
                    IssuedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiredAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshTokens_Accounts_IdAccount",
                        column: x => x.IdAccount,
                        principalTable: "Accounts",
                        principalColumn: "IdAccount",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Buyers",
                columns: table => new
                {
                    IdBuyer = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Companyname = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TaxCode = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    AccountBanking = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: false),
                    BankingName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Buyers", x => x.IdBuyer);
                    table.ForeignKey(
                        name: "FK_Buyers_Invoices_IdBuyer",
                        column: x => x.IdBuyer,
                        principalTable: "Invoices",
                        principalColumn: "IdInvoice",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    IdItem = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal", nullable: false),
                    IdInvoice = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.IdItem);
                    table.ForeignKey(
                        name: "FK_Items_Invoices_IdInvoice",
                        column: x => x.IdInvoice,
                        principalTable: "Invoices",
                        principalColumn: "IdInvoice",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sellers",
                columns: table => new
                {
                    IdSeller = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Phone = table.Column<string>(type: "char(10)", maxLength: 10, nullable: false),
                    TaxCode = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Email = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    AccountBanking = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: false),
                    BankingName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sellers", x => x.IdSeller);
                    table.ForeignKey(
                        name: "FK_Sellers_Invoices_IdSeller",
                        column: x => x.IdSeller,
                        principalTable: "Invoices",
                        principalColumn: "IdInvoice",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "IdRole", "Name" },
                values: new object[] { "L5uojNlToi", "User" });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "IdRole", "Name" },
                values: new object[] { "o4kINXogbG", "Admin" });

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_UserRole",
                table: "Accounts",
                column: "UserRole");

            migrationBuilder.CreateIndex(
                name: "IX_ExcelForms_IdAccountingSoftware",
                table: "ExcelForms",
                column: "IdAccountingSoftware");

            migrationBuilder.CreateIndex(
                name: "IX_ExcelResults_IdAccount",
                table: "ExcelResults",
                column: "IdAccount");

            migrationBuilder.CreateIndex(
                name: "IX_ExcelResults_IdExcelForm",
                table: "ExcelResults",
                column: "IdExcelForm");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_IdAccount",
                table: "Invoices",
                column: "IdAccount");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_IdInvoiceForm",
                table: "Invoices",
                column: "IdInvoiceForm");

            migrationBuilder.CreateIndex(
                name: "IX_Items_IdInvoice",
                table: "Items",
                column: "IdInvoice");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_IdAccount",
                table: "RefreshTokens",
                column: "IdAccount");

            migrationBuilder.CreateIndex(
                name: "IX_SuppliedInvoiceForms_IdInvoiceForm",
                table: "SuppliedInvoiceForms",
                column: "IdInvoiceForm");

            migrationBuilder.CreateIndex(
                name: "IX_SuppliedInvoiceForms_IdSupplier",
                table: "SuppliedInvoiceForms",
                column: "IdSupplier");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Buyers");

            migrationBuilder.DropTable(
                name: "ExcelResults");

            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.DropTable(
                name: "Sellers");

            migrationBuilder.DropTable(
                name: "SuppliedInvoiceForms");

            migrationBuilder.DropTable(
                name: "ExcelForms");

            migrationBuilder.DropTable(
                name: "Invoices");

            migrationBuilder.DropTable(
                name: "Suppliers");

            migrationBuilder.DropTable(
                name: "AccountingSoftwares");

            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "InvoiceForms");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
