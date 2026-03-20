namespace SecureCommerce_api.DTOs.Order
{
    public class OrderDto
    {
        public Guid Id { get; set; }
        public Guid? UserId { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? ShippingFullName { get; set; }
        public string? ShippingAddress { get; set; }
        public string? ShippingCity { get; set; }
        public string? ShippingZipCode { get; set; }
        public string? ShippingCountry { get; set; }
        public string? PaymentStatus { get; set; }
        public string? PaymentMethod { get; set; }
        public DateTime? CreatedAt { get; set; }
        public IReadOnlyCollection<OrderItemDto> Items { get; set; } = Array.Empty<OrderItemDto>();
    }
}
