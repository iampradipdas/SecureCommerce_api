using AutoMapper;
using SecureCommerce_api.Dal.Entities;
using SecureCommerce_api.DTOs.Order;

namespace SecureCommerce_api.Mappings
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<Order, OrderDto>()
                .ForMember(destination => destination.TotalAmount,
                    options => options.MapFrom(source => source.TotalAmount ?? 0))
                .ForMember(destination => destination.Status,
                    options => options.MapFrom(source => source.Status ?? string.Empty))
                .ForMember(destination => destination.Items,
                    options => options.MapFrom(source => source.OrderItems));

            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(destination => destination.ProductId,
                    options => options.MapFrom(source => source.ProductId ?? Guid.Empty))
                .ForMember(destination => destination.ProductName,
                    options => options.MapFrom(source => source.Product != null ? source.Product.Name ?? string.Empty : string.Empty))
                .ForMember(destination => destination.Quantity,
                    options => options.MapFrom(source => source.Quantity ?? 0))
                .ForMember(destination => destination.Price,
                    options => options.MapFrom(source => source.Price ?? 0))
                .ForMember(destination => destination.LineTotal,
                    options => options.MapFrom(source => (source.Price ?? 0) * (source.Quantity ?? 0)));
        }
    }
}
