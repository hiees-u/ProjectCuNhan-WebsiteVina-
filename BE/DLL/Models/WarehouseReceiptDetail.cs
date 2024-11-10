using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DLL.Models;

/// <summary>
/// Chi Tiết Phiếu Nhập Kho
/// </summary>
[PrimaryKey("WarehouseReceiptId", "CellId")]
[Table("WarehouseReceiptDetail")]
public partial class WarehouseReceiptDetail
{
    [Key]
    [Column("WarehouseReceiptID")]
    public int WarehouseReceiptId { get; set; }

    [Key]
    [Column("CellID")]
    public int CellId { get; set; }

    [Column("product_id")]
    public int ProductId { get; set; }

    [Column("quantity")]
    public int? Quantity { get; set; }

    [Column("PurchaseOrderID")]
    public int PurchaseOrderId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? UpdateTime { get; set; }

    public int? UpdateBy { get; set; }

    [ForeignKey("CellId")]
    [InverseProperty("WarehouseReceiptDetails")]
    public virtual Cell Cell { get; set; } = null!;

    [ForeignKey("PurchaseOrderId, ProductId")]
    [InverseProperty("WarehouseReceiptDetails")]
    public virtual PurchaseOrderDetail PurchaseOrderDetail { get; set; } = null!;

    [ForeignKey("WarehouseReceiptId")]
    [InverseProperty("WarehouseReceiptDetails")]
    public virtual WarehouseReceipt WarehouseReceipt { get; set; } = null!;
}
