using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SecureCommerce_api.Dal.Entities;

[Table("refresh_tokens")]
public partial class RefreshToken
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("user_id")]
    public Guid? UserId { get; set; }

    [Column("token")]
    public string? Token { get; set; }

    [Column("expiry_date", TypeName = "timestamp without time zone")]
    public DateTime? ExpiryDate { get; set; }

    [Column("is_revoked")]
    public bool? IsRevoked { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("RefreshTokens")]
    public virtual User? User { get; set; }
}
