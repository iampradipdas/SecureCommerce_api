using Microsoft.EntityFrameworkCore;
using SecureCommerce_api.Dal;
using SecureCommerce_api.Extensions;
using FluentValidation;
using FluentValidation.AspNetCore;
using Asp.Versioning;

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Configure DbContext
builder.Services.AddDbContext<SecureCommerceContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection") ?? "Host=localhost;Port=5432;User Id=postgres;Password=Pradip@123;Database=securecommerce_db"));
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddJwtAuthorization(builder.Configuration);

builder.WebHost.UseUrls("http://0.0.0.0:8080");

// Add FluentValidation services
builder.Services.AddValidatorsFromAssemblyContaining<Program>();
builder.Services.AddFluentValidationAutoValidation();

// Configure API versioning
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
    options.ApiVersionReader = new UrlSegmentApiVersionReader();
}).AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularDev",
        policy => policy.WithOrigins("http://localhost:4200", "https://localhost:4200")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAngularDev");

app.UseHttpsRedirection();

app.UseStaticFiles();  // Enable serving static files from wwwroot

app.UseJwtAuthorization();

app.UsePerformanceLogging();

app.MapControllers();

app.Run();
