using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DLL.Models;

/// <summary>
/// Địa Chỉ
/// </summary>
[Table("Address")]
public partial class Address
{
    [Key]
    [Column("AddressID")]
    public int AddressId { get; set; }

    [Column("CommuneID")]
    public int CommuneId { get; set; }

    /// <summary>
    /// Tên đường + số nhà
    /// </summary>
    [StringLength(10)]
    public string HouseNumber { get; set; } = null!;

    [StringLength(50)]
    public string? Note { get; set; }

    [ForeignKey("CommuneId")]
    [InverseProperty("Addresses")]
    public virtual Commune Commune { get; set; } = null!;

    [InverseProperty("Adress")]
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    [InverseProperty("AddressNavigation")]
    public virtual ICollection<PurchaseOrderDetail> PurchaseOrderDetails { get; set; } = new List<PurchaseOrderDetail>();

    [InverseProperty("Address")]
    public virtual ICollection<Supplier> Suppliers { get; set; } = new List<Supplier>();

    [InverseProperty("Address")]
    public virtual ICollection<UserInfo> UserInfos { get; set; } = new List<UserInfo>();

    [InverseProperty("Address")]
    public virtual ICollection<Warehouse> Warehouses { get; set; } = new List<Warehouse>();
}
