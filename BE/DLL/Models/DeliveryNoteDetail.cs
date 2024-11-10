using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DLL.Models;

/// <summary>
/// Chi Tiết Phiếu Xuất Kho
/// </summary>
[PrimaryKey("DeliveryNote", "CellId")]
[Table("DeliveryNoteDetail")]
public partial class DeliveryNoteDetail
{
    [Key]
    public int DeliveryNote { get; set; }

    [Column("product_id")]
    public int ProductId { get; set; }

    [Key]
    [Column("CellID")]
    public int CellId { get; set; }

    [Column("quantity")]
    public int Quantity { get; set; }

    [ForeignKey("CellId")]
    [InverseProperty("DeliveryNoteDetails")]
    public virtual Cell Cell { get; set; } = null!;

    [ForeignKey("DeliveryNote")]
    [InverseProperty("DeliveryNoteDetails")]
    public virtual DeliveryNote DeliveryNoteNavigation { get; set; } = null!;

    [ForeignKey("ProductId")]
    [InverseProperty("DeliveryNoteDetails")]
    public virtual Product Product { get; set; } = null!;
}
