namespace SecureCommerce_api.DTOs.Product
{
    public class ProductDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public Guid? VendorId { get; set; }
        public Guid? CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public double AverageRating { get; set; }
        public int ReviewCount { get; set; }
        public string? ImageUrl { get; set; }
    }
}
