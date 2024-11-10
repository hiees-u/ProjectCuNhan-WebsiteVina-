using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DLL.Models;

/// <summary>
/// Tỉnh/Thành Phố
/// </summary>
[Table("Province")]
[Index("ProvinceName", Name = "uni_ProvinceName", IsUnique = true)]
public partial class Province
{
    [Key]
    [Column("ProvinceID")]
    public int ProvinceId { get; set; }

    [StringLength(30)]
    public string ProvinceName { get; set; } = null!;

    [InverseProperty("Province")]
    public virtual ICollection<District> Districts { get; set; } = new List<District>();
}
