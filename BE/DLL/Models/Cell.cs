using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DLL.Models;

/// <summary>
/// Ô chứa sản phẩm
/// </summary>
[Index("CellName", Name = "uni_CellName", IsUnique = true)]
public partial class Cell
{
    [Key]
    [Column("CellID")]
    public int CellId { get; set; }

    [StringLength(30)]
    public string CellName { get; set; } = null!;

    [Column("ShelvesID")]
    public int ShelvesId { get; set; }

    public int? Quantity { get; set; }

    [Column("product_id")]
    public int? ProductId { get; set; }

    [StringLength(25)]
    [Unicode(false)]
    public string? ModifiedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreateTime { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ModifiedTime { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DeleteTime { get; set; }

    [InverseProperty("Cell")]
    public virtual ICollection<DeliveryNoteDetail> DeliveryNoteDetails { get; set; } = new List<DeliveryNoteDetail>();

    [ForeignKey("ProductId")]
    [InverseProperty("Cells")]
    public virtual Product? Product { get; set; }

    [ForeignKey("ShelvesId")]
    [InverseProperty("Cells")]
    public virtual Shelve Shelves { get; set; } = null!;

    [InverseProperty("Cell")]
    public virtual ICollection<WarehouseReceiptDetail> WarehouseReceiptDetails { get; set; } = new List<WarehouseReceiptDetail>();
}
