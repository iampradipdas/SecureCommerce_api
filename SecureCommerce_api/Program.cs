using Microsoft.EntityFrameworkCore;
using SecureCommerce_api.Bal;
using SecureCommerce_api.Bal.Interfaces;
using SecureCommerce_api.Dal;
using SecureCommerce_api.Dal.Repositories;
using SecureCommerce_api.Dal.Repositories.Interfaces;
using SecureCommerce_api.Extensions;
using SecureCommerce_api.Mappings;

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddAutoMapper(typeof(ProductProfile));

// Configure DbContext
builder.Services.AddDbContext<SecureCommerceContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection") ?? "Host=localhost;Port=5432;User Id=postgres;Password=Pradip@123;Database=securecommerce_db"));

// Configure Dependency Injection
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();

builder.Services.AddJwtAuthorization(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseJwtAuthorization();

app.MapControllers();

app.Run();
