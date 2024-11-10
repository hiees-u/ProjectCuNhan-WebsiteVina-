using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DLL.Models;

/// <summary>
/// Chi Tiết Đơn Đặt Hàng Của Khách
/// </summary>
[PrimaryKey("OrderId", "PriceHistoryId")]
[Table("OrderDetail")]
public partial class OrderDetail
{
    [Key]
    [Column("Order_Id")]
    public int OrderId { get; set; }

    [Key]
    [Column("priceHistoryId")]
    public int PriceHistoryId { get; set; }

    public int Quantity { get; set; }

    [ForeignKey("OrderId")]
    [InverseProperty("OrderDetails")]
    public virtual Order Order { get; set; } = null!;

    [ForeignKey("PriceHistoryId")]
    [InverseProperty("OrderDetails")]
    public virtual PriceHistory PriceHistory { get; set; } = null!;
}
