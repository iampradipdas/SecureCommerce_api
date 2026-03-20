using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SecureCommerce_api.Dal.Entities;

[Table("orders")]
public partial class Order
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("user_id")]
    public Guid? UserId { get; set; }

    [Column("total_amount")]
    public decimal? TotalAmount { get; set; }

    [Column("status")]
    [StringLength(50)]
    public string? Status { get; set; }

    [Column("shipping_full_name")]
    [StringLength(100)]
    public string? ShippingFullName { get; set; }

    [Column("shipping_address")]
    [StringLength(255)]
    public string? ShippingAddress { get; set; }

    [Column("shipping_city")]
    [StringLength(100)]
    public string? ShippingCity { get; set; }

    [Column("shipping_zip_code")]
    [StringLength(20)]
    public string? ShippingZipCode { get; set; }

    [Column("shipping_country")]
    [StringLength(100)]
    public string? ShippingCountry { get; set; }

    [Column("payment_status")]
    [StringLength(50)]
    public string? PaymentStatus { get; set; }

    [Column("payment_method")]
    [StringLength(50)]
    public string? PaymentMethod { get; set; }

    [Column("created_at", TypeName = "timestamp without time zone")]
    public DateTime? CreatedAt { get; set; }

    [InverseProperty("Order")]
    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    [ForeignKey("UserId")]
    [InverseProperty("Orders")]
    public virtual User? User { get; set; }
}
