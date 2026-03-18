using AutoMapper;
using SecureCommerce_api.Bal.Interfaces;
using SecureCommerce_api.Dal.Repositories.Interfaces;
using SecureCommerce_api.DTOs.Order;

namespace SecureCommerce_api.Bal
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public OrderService(IOrderRepository orderRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        public async Task<(bool Success, string Message, OrderDto? Order)> CheckoutAsync(Guid userId)
        {
            var result = await _orderRepository.CheckoutAsync(userId);
            return result.Order == null
                ? (result.Success, result.Message, null)
                : (result.Success, result.Message, _mapper.Map<OrderDto>(result.Order));
        }

        public async Task<IReadOnlyCollection<OrderDto>> GetOrdersAsync(Guid userId)
        {
            var orders = await _orderRepository.GetOrdersByUserIdAsync(userId);
            return _mapper.Map<List<OrderDto>>(orders);
        }

        public async Task<OrderDto?> GetOrderByIdAsync(Guid userId, Guid orderId)
        {
            var order = await _orderRepository.GetOrderByIdAsync(userId, orderId);
            return order == null ? null : _mapper.Map<OrderDto>(order);
        }
    }
}
