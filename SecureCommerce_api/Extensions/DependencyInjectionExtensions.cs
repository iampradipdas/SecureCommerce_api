using Microsoft.EntityFrameworkCore;
using SecureCommerce_api.Bal;
using SecureCommerce_api.Bal.Interfaces;
using SecureCommerce_api.Dal;
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
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IReviewRepository, ReviewRepository>();
        services.AddScoped<IReviewService, ReviewService>();

        services.AddHttpContextAccessor();
        services.AddScoped<IImageService, LocalImageService>();

        return services;
    }
}
