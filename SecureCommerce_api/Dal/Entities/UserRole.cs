using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SecureCommerce_api.Dal.Entities;

[Keyless]
[Table("user_roles")]
public partial class UserRole
{
    [Column("user_id")]
    public Guid? UserId { get; set; }

    [Column("role_id")]
    public int? RoleId { get; set; }

    [ForeignKey("RoleId")]
    public virtual Role? Role { get; set; }

    [ForeignKey("UserId")]
    public virtual User? User { get; set; }
}
