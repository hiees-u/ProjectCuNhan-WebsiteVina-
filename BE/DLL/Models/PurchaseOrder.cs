using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DLL.Models;

/// <summary>
/// Đơn đặt hàng của Công ty
/// </summary>
[Table("PurchaseOrder")]
public partial class PurchaseOrder
{
    [Key]
    [Column("PurchaseOrderID")]
    public int PurchaseOrderId { get; set; }

    [Column("EmployeeID")]
    public int EmployeeId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreateAt { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DeliveryDate { get; set; }

    [ForeignKey("EmployeeId")]
    [InverseProperty("PurchaseOrders")]
    public virtual Employee Employee { get; set; } = null!;

    [InverseProperty("PurchaseOrder")]
    public virtual ICollection<PurchaseOrderDetail> PurchaseOrderDetails { get; set; } = new List<PurchaseOrderDetail>();
}
