using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DLL.Models;

/// <summary>
/// Phiếu Nhập Kho
/// </summary>
[Table("WarehouseReceipt")]
public partial class WarehouseReceipt
{
    [Key]
    [Column("WarehouseReceiptID")]
    public int WarehouseReceiptId { get; set; }

    [Column("EmployeeID")]
    public int EmployeeId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreateAt { get; set; }

    [Column("WarehouseID")]
    public int WarehouseId { get; set; }

    [ForeignKey("EmployeeId")]
    [InverseProperty("WarehouseReceipts")]
    public virtual Employee Employee { get; set; } = null!;

    [ForeignKey("WarehouseId")]
    [InverseProperty("WarehouseReceipts")]
    public virtual Warehouse Warehouse { get; set; } = null!;

    [InverseProperty("WarehouseReceipt")]
    public virtual ICollection<WarehouseReceiptDetail> WarehouseReceiptDetails { get; set; } = new List<WarehouseReceiptDetail>();
}
