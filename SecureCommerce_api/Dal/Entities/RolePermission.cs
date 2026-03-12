using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SecureCommerce_api.Dal.Entities;

[Keyless]
[Table("role_permissions")]
public partial class RolePermission
{
    [Column("role_id")]
    public int? RoleId { get; set; }

    [Column("permission_id")]
    public int? PermissionId { get; set; }

    [ForeignKey("PermissionId")]
    public virtual Permission? Permission { get; set; }

    [ForeignKey("RoleId")]
    public virtual Role? Role { get; set; }
}
