namespace SecureCommerce_api.DTOs.Order
{
    public class OrderDto
    {
        public Guid Id { get; set; }
        public Guid? UserId { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime? CreatedAt { get; set; }
        public IReadOnlyCollection<OrderItemDto> Items { get; set; } = Array.Empty<OrderItemDto>();
    }
}
