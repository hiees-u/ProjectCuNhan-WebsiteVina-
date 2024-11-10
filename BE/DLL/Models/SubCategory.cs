using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DLL.Models;

/// <summary>
/// Loại Sản Phẩm Phụ
/// </summary>
[Table("SubCategory")]
[Index("SubCategoryName", Name = "uni_SubCategoryName", IsUnique = true)]
public partial class SubCategory
{
    [Key]
    [Column("SubCategoryID")]
    public int SubCategoryId { get; set; }

    [StringLength(30)]
    public string SubCategoryName { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string? Image { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DeleteTime { get; set; }

    [StringLength(25)]
    [Unicode(false)]
    public string? ModifiedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreateTime { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ModifiedTime { get; set; }

    [InverseProperty("SubCategory")]
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
