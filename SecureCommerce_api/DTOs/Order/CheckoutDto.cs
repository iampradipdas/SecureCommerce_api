using System.ComponentModel.DataAnnotations;

namespace SecureCommerce_api.DTOs.Order
{
    public class CheckoutDto
    {
        [Required]
        [StringLength(100)]
        public string ShippingFullName { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        public string ShippingAddress { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string ShippingCity { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string ShippingZipCode { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string ShippingCountry { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string PaymentMethod { get; set; } = "Credit Card";
    }
}
