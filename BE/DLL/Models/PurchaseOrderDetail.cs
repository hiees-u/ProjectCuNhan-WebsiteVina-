using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DLL.Models;

/// <summary>
/// Chi tiết đơn đặt hàng của Công Ty
/// </summary>
[PrimaryKey("PurchaseOrderId", "PriceHistoryId")]
[Table("PurchaseOrderDetail")]
public partial class PurchaseOrderDetail
{
    [Key]
    [Column("PurchaseOrderID")]
    public int PurchaseOrderId { get; set; }

    [Key]
    [Column("priceHistoryId")]
    public int PriceHistoryId { get; set; }

    [Column("quantity")]
    public int? Quantity { get; set; }

    public int? Address { get; set; }

    public int State { get; set; }

    public int? QuantityDelivered { get; set; }

    [ForeignKey("Address")]
    [InverseProperty("PurchaseOrderDetails")]
    public virtual Address? AddressNavigation { get; set; }

    [ForeignKey("PriceHistoryId")]
    [InverseProperty("PurchaseOrderDetails")]
    public virtual PriceHistory PriceHistory { get; set; } = null!;

    [ForeignKey("PurchaseOrderId")]
    [InverseProperty("PurchaseOrderDetails")]
    public virtual PurchaseOrder PurchaseOrder { get; set; } = null!;

    [InverseProperty("PurchaseOrderDetail")]
    public virtual ICollection<WarehouseReceiptDetail> WarehouseReceiptDetails { get; set; } = new List<WarehouseReceiptDetail>();
}
