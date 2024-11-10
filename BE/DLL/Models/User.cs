using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DLL.Models;

/// <summary>
/// Người Dùng
/// </summary>
public partial class User
{
    [Key]
    [StringLength(25)]
    [Unicode(false)]
    public string AccountName { get; set; } = null!;

    [Column("role_id")]
    public int RoleId { get; set; }

    [ForeignKey("RoleId")]
    [InverseProperty("Users")]
    public virtual Role Role { get; set; } = null!;

    [InverseProperty("AccountNameNavigation")]
    public virtual UserInfo? UserInfo { get; set; }
}
