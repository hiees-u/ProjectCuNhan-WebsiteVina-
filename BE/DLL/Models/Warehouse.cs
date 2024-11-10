using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DLL.Models;

/// <summary>
/// Kho
/// </summary>
[Table("Warehouse")]
[Index("WarehouseName", Name = "uni_WarehouseName", IsUnique = true)]
public partial class Warehouse
{
    [Key]
    [Column("WarehouseID")]
    public int WarehouseId { get; set; }

    [StringLength(30)]
    public string WarehouseName { get; set; } = null!;

    [Column("AddressID")]
    public int AddressId { get; set; }

    [StringLength(25)]
    [Unicode(false)]
    public string? ModifiedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreateTime { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ModifiedTime { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DeleteTime { get; set; }

    [ForeignKey("AddressId")]
    [InverseProperty("Warehouses")]
    public virtual Address Address { get; set; } = null!;

    [InverseProperty("Warehouse")]
    public virtual ICollection<DeliveryNote> DeliveryNotes { get; set; } = new List<DeliveryNote>();

    [InverseProperty("Warehouse")]
    public virtual ICollection<Shelve> Shelves { get; set; } = new List<Shelve>();

    [InverseProperty("Warehouse")]
    public virtual ICollection<WarehouseReceipt> WarehouseReceipts { get; set; } = new List<WarehouseReceipt>();
}
