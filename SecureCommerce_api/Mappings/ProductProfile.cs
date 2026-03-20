using AutoMapper;
using SecureCommerce_api.Dal.Entities;
using SecureCommerce_api.DTOs.Product;

namespace SecureCommerce_api.Mappings
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductDto>()
                .ForMember(destination => destination.Name,
                    options => options.MapFrom(source => source.Name ?? string.Empty))
                .ForMember(destination => destination.Price,
                    options => options.MapFrom(source => source.Price ?? 0))
                .ForMember(destination => destination.Stock,
                    options => options.MapFrom(source => source.Stock ?? 0))
                .ForMember(destination => destination.CategoryName,
                    options => options.MapFrom(source => source.Category != null ? source.Category.Name : null))
                .ForMember(destination => destination.AverageRating,
                    options => options.MapFrom(source => source.Reviews.Any() ? source.Reviews.Average(r => r.Rating) : 0))
                .ForMember(destination => destination.ReviewCount,
                    options => options.MapFrom(source => source.Reviews.Count));
        }
    }
}
