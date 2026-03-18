using SecureCommerce_api.Bal;
using SecureCommerce_api.Bal.Interfaces;
using SecureCommerce_api.Dal.Repositories;
using SecureCommerce_api.Dal.Repositories.Interfaces;
using SecureCommerce_api.Mappings;

namespace SecureCommerce_api.Extensions;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAutoMapper(typeof(ProductProfile));

        services.AddScoped<IAuthRepository, AuthRepository>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ICartRepository, CartRepository>();
        services.AddScoped<ICartService, CartService>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IProductService, ProductService>();

        return services;
    }
}
