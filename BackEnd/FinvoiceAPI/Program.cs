using BAL.AutoMapperProfile;
using BAL.Services.Implements;
using BAL.Services.Interfaces;
using DAL;
using DAL.Entities;
using DAL.Infrastructure;
using DAL.Repositories.Implements;
using DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Filters;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(s =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    s.IncludeXmlComments(xmlPath);

    #region Authentication & Authorization
    s.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Standard Authorization header using the Beare Scheme",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    s.OperationFilter<SecurityRequirementsOperationFilter>();
    #endregion
});

builder.Services.AddMvc().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
});

builder.Services.AddDbContext<FInvoiceDBContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
});

#region Register Dependency Injection
builder.Services.AddScoped<IDbFactory, DbFactory>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<IRepository<AccountingSoftware>, AccountingSoftwareRepository>();
builder.Services.AddScoped<IRepository<Account>, AccountRepository>();
builder.Services.AddScoped<IRepository<Buyer>, BuyerRepository>();
builder.Services.AddScoped<IRepository<ExcelForm>, ExcelFormRepository>();
builder.Services.AddScoped<IRepository<ExcelResult>, ExcelResultRepository>();
builder.Services.AddScoped<IRepository<InvoiceForm>, InvoiceFormRepository>();
builder.Services.AddScoped<IRepository<Invoice>, InvoiceRepository>();
builder.Services.AddScoped<IRepository<Item>, ItemRepository>();
builder.Services.AddScoped<IRepository<Role>, RoleRepository>();
builder.Services.AddScoped<IRepository<Seller>, SellerRepository>();
builder.Services.AddScoped<IRepository<SuppliedInvoiceForm>, SuppliedInvoiceFormRepository>();
builder.Services.AddScoped<IRepository<Supplier>, SupplierRepository>();
builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

builder.Services.AddScoped<IAccountingSoftwareService, AccountingSoftwareService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IBuyerService, BuyerService>();
builder.Services.AddScoped<IExcelFormService, ExcelFormService>();
builder.Services.AddScoped<IExcelResultService, ExcelResultService>();
builder.Services.AddScoped<IInvoiceFormService, InvoiceFormService>();
builder.Services.AddScoped<IInvoiceService, InvoiceService>();
builder.Services.AddScoped<IItemService, ItemService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<ISellerService, SellerService>();
builder.Services.AddScoped<ISuppliedInvoiceFormService, SuppliedInvoiceFormService>();
builder.Services.AddScoped<ISupplierService, SupplierService>();
builder.Services.AddScoped<IRefreshTokenService, RefreshTokenService>();
#endregion

builder.Services.AddControllers().AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        // Tự cấp Token
        ValidateIssuer = false,
        ValidateAudience = false,

        //Ký vào Token
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey
        (
            Encoding.UTF8.GetBytes(builder.Configuration.GetSection("AppSettings:SerectKey").Value)
        ),
    };
});

#region Add Auto Mapper
builder.Services.AddAutoMapper(typeof(AccountingSoftwareProfile));
builder.Services.AddAutoMapper(typeof(AccountProfile));
builder.Services.AddAutoMapper(typeof(BuyerProfile));
builder.Services.AddAutoMapper(typeof(ExcelFormProfile));
builder.Services.AddAutoMapper(typeof(ExcelResultProfile));
builder.Services.AddAutoMapper(typeof(InvoiceFormProfile));
builder.Services.AddAutoMapper(typeof(InvoiceProfile));
builder.Services.AddAutoMapper(typeof(ItemProfile));
builder.Services.AddAutoMapper(typeof(RoleProfile));
builder.Services.AddAutoMapper(typeof(SellerProfile));
builder.Services.AddAutoMapper(typeof(SuppliedInvoiceFormProfile));
builder.Services.AddAutoMapper(typeof(SupplierProfile));
builder.Services.AddAutoMapper(typeof(UserProfile));
#endregion

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        policy =>
        {
            policy.WithOrigins("http://127.0.0.1:5500");
            policy.WithMethods("GET", "POST", "PUT");
            policy.WithHeaders("Content-Type", "Access-Control-Allow-Headers");
        });
});

var app = builder.Build();

//Allow Cross Origin to Create Separated Front-end
app.UseCors(MyAllowSpecificOrigins);
app.UseSession();

// Configure the HTTP request pipeline.
app.UseSwagger();

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Ho Le API v1");
});

app.UseHttpsRedirection();

#region Authentication & Authorization
app.UseAuthentication();
#endregion

app.UseAuthorization();

app.MapControllers();

#region Auto migration
bool autoMigrate = app.Configuration.GetValue<bool>("MigrationSettings:autoMigrate");
if (autoMigrate)
{
    DbContext context = new FInvoiceDBContext();
    context.Database.Migrate();
}
#endregion

app.Run();
