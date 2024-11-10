using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DLL.Models;

/// <summary>
/// Giỏ Hàng
/// </summary>
[PrimaryKey("CustomerId", "ProductId")]
[Table("Cart")]
public partial class Cart
{
    [Key]
    [Column("customerId")]
    public int CustomerId { get; set; }

    [Key]
    [Column("product_id")]
    public int ProductId { get; set; }

    [Column("quantity")]
    public int? Quantity { get; set; }

    [ForeignKey("CustomerId")]
    [InverseProperty("Carts")]
    public virtual Customer Customer { get; set; } = null!;

    [ForeignKey("ProductId")]
    [InverseProperty("Carts")]
    public virtual Product Product { get; set; } = null!;
}
