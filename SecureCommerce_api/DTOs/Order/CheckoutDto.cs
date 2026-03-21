namespace SecureCommerce_api.DTOs.Order
{
    public class CheckoutDto
    {
        public string ShippingFullName { get; set; } = string.Empty;
        public string ShippingAddress { get; set; } = string.Empty;
        public string ShippingCity { get; set; } = string.Empty;
        public string ShippingZipCode { get; set; } = string.Empty;
        public string ShippingCountry { get; set; } = string.Empty;
        public string PaymentMethod { get; set; } = "Credit Card";
    }
}
