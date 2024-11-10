using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DLL.Models;

[Table("Shelve")]
[Index("ShelvesName", Name = "UNI_ShelveName", IsUnique = true)]
public partial class Shelve
{
    [Key]
    [Column("ShelvesID")]
    public int ShelvesId { get; set; }

    [StringLength(30)]
    public string ShelvesName { get; set; } = null!;

    [Column("WarehouseID")]
    public int WarehouseId { get; set; }

    [StringLength(25)]
    [Unicode(false)]
    public string? ModifiedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreateTime { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ModifiedTime { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DeleteTime { get; set; }

    [InverseProperty("Shelves")]
    public virtual ICollection<Cell> Cells { get; set; } = new List<Cell>();

    [ForeignKey("WarehouseId")]
    [InverseProperty("Shelves")]
    public virtual Warehouse Warehouse { get; set; } = null!;
}
