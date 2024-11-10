using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DLL.Models;

/// <summary>
/// Quận/Huyện
/// </summary>
[Table("District")]
[Index("DistrictName", Name = "uni_DistrictName", IsUnique = true)]
public partial class District
{
    [Key]
    [Column("DistrictID")]
    public int DistrictId { get; set; }

    [StringLength(30)]
    public string DistrictName { get; set; } = null!;

    [Column("ProvinceID")]
    public int ProvinceId { get; set; }

    [InverseProperty("District")]
    public virtual ICollection<Commune> Communes { get; set; } = new List<Commune>();

    [ForeignKey("ProvinceId")]
    [InverseProperty("Districts")]
    public virtual Province Province { get; set; } = null!;
}
