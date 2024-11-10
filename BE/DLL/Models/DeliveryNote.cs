using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DLL.Models;

/// <summary>
/// Phiếu Xuất Kho
/// </summary>
[Table("DeliveryNote")]
public partial class DeliveryNote
{
    [Key]
    [Column("DeliveryNoteID")]
    public int DeliveryNoteId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreateAt { get; set; }

    public int CreateBy { get; set; }

    public int OrderId { get; set; }

    public int WarehouseId { get; set; }

    [StringLength(50)]
    public string? Note { get; set; }

    [ForeignKey("CreateBy")]
    [InverseProperty("DeliveryNotes")]
    public virtual Employee CreateByNavigation { get; set; } = null!;

    [InverseProperty("DeliveryNoteNavigation")]
    public virtual ICollection<DeliveryNoteDetail> DeliveryNoteDetails { get; set; } = new List<DeliveryNoteDetail>();

    [ForeignKey("OrderId")]
    [InverseProperty("DeliveryNotes")]
    public virtual Order Order { get; set; } = null!;

    [ForeignKey("WarehouseId")]
    [InverseProperty("DeliveryNotes")]
    public virtual Warehouse Warehouse { get; set; } = null!;
}
