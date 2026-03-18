using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SecureCommerce_api.Dal.Entities;

[Table("cart_items")]
[Index("CartId", Name = "idx_cart_items_cart_id")]
[Index("ProductId", Name = "idx_cart_items_product_id")]
[Index("CartId", "ProductId", Name = "uq_cart_items_cart_product", IsUnique = true)]
public partial class CartItem
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("cart_id")]
    public Guid CartId { get; set; }

    [Column("product_id")]
    public Guid ProductId { get; set; }

    [Column("quantity")]
    public int Quantity { get; set; }

    [Column("created_at", TypeName = "timestamp without time zone")]
    public DateTime? CreatedAt { get; set; }

    [Column("updated_at", TypeName = "timestamp without time zone")]
    public DateTime? UpdatedAt { get; set; }

    [ForeignKey("CartId")]
    [InverseProperty("CartItems")]
    public virtual Cart Cart { get; set; } = null!;

    [ForeignKey("ProductId")]
    [InverseProperty("CartItems")]
    public virtual Product Product { get; set; } = null!;
}
