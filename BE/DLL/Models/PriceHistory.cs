using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DLL.Models;

/// <summary>
/// Lịch sử giá
/// </summary>
[Table("PriceHistory")]
public partial class PriceHistory
{
    [Key]
    [Column("priceHistoryId")]
    public int PriceHistoryId { get; set; }

    [Column("product_id")]
    public int ProductId { get; set; }

    [Column("price", TypeName = "decimal(10, 0)")]
    public decimal Price { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreateDate { get; set; }

    public int CreateBy { get; set; }

    [Column("isActive")]
    public bool IsActive { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? UpdateTime { get; set; }

    public int? UpdateBy { get; set; }

    [InverseProperty("PriceHistory")]
    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    [ForeignKey("ProductId")]
    [InverseProperty("PriceHistories")]
    public virtual Product Product { get; set; } = null!;

    [InverseProperty("PriceHistory")]
    public virtual ICollection<PurchaseOrderDetail> PurchaseOrderDetails { get; set; } = new List<PurchaseOrderDetail>();
}
