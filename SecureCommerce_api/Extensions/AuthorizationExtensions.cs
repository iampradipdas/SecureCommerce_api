using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace SecureCommerce_api.Extensions;

public static class AuthorizationExtensions
{
    public static IServiceCollection AddJwtAuthorization(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSettings = configuration.GetSection("Jwt");
        var key = Encoding.UTF8.GetBytes(
            jwtSettings["Key"] ?? "SecureCommerce_Secret_Key_For_JWT_Authentication_12345");

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings["Issuer"],
                ValidAudience = jwtSettings["Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(key)
            };
        });

        services.AddAuthorization(options =>
        {
            options.AddPolicy("ProductRead", policy => policy.RequireClaim("Permission", "product:read"));
            options.AddPolicy("ProductWrite", policy => policy.RequireClaim("Permission", "product:write"));
            options.AddPolicy("ProductDelete", policy => policy.RequireClaim("Permission", "product:delete"));
            options.AddPolicy("CategoryWrite", policy => policy.RequireClaim("Permission", "category:write"));
            options.AddPolicy("OrderShip", policy => policy.RequireClaim("Permission", "order:ship"));
            options.AddPolicy("AdminOnly", policy => policy.RequireClaim("Permission", "user:manage"));
        });

        return services;
    }

    public static IApplicationBuilder UseJwtAuthorization(this IApplicationBuilder app)
    {
        app.UseAuthentication();
        app.UseAuthorization();

        return app;
    }
}
