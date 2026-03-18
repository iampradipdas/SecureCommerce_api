namespace SecureCommerce_api.DTOs.Cart
{
    public class CartDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public int TotalItems { get; set; }
        public decimal TotalAmount { get; set; }
        public IReadOnlyCollection<CartItemDto> Items { get; set; } = Array.Empty<CartItemDto>();
    }
}
