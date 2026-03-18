using AutoMapper;
using SecureCommerce_api.Dal.Entities;
using SecureCommerce_api.DTOs.Cart;

namespace SecureCommerce_api.Mappings
{
    public class CartProfile : Profile
    {
        public CartProfile()
        {
            CreateMap<Cart, CartDto>()
                .ForMember(destination => destination.Items,
                    options => options.MapFrom(source => source.CartItems));

            CreateMap<CartItem, CartItemDto>()
                .ForMember(destination => destination.ProductName,
                    options => options.MapFrom(source => source.Product != null ? source.Product.Name ?? string.Empty : string.Empty))
                .ForMember(destination => destination.ProductDescription,
                    options => options.MapFrom(source => source.Product != null ? source.Product.Description : null))
                .ForMember(destination => destination.UnitPrice,
                    options => options.MapFrom(source => source.Product != null ? source.Product.Price ?? 0 : 0))
                .ForMember(destination => destination.LineTotal,
                    options => options.MapFrom(source => (source.Product != null ? source.Product.Price ?? 0 : 0) * source.Quantity));
        }
    }
}
