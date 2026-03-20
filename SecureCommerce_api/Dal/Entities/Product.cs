using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SecureCommerce_api.Dal.Entities;

[Table("products")]
public partial class Product
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("name")]
    [StringLength(200)]
    public string? Name { get; set; }

    [Column("description")]
    public string? Description { get; set; }

    [Column("price")]
    public decimal? Price { get; set; }

    [Column("stock")]
    public int? Stock { get; set; }

    [Column("vendor_id")]
    public Guid? VendorId { get; set; }

    [Column("category_id")]
    public Guid? CategoryId { get; set; }

    [Column("image_url")]
    public string? ImageUrl { get; set; }

    [InverseProperty("Product")]
    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    [InverseProperty("Product")]
    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    [InverseProperty("Product")]
    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    [ForeignKey("VendorId")]
    [InverseProperty("Products")]
    public virtual User? Vendor { get; set; }

    [ForeignKey("CategoryId")]
    [InverseProperty("Products")]
    public virtual Category? Category { get; set; }
}
