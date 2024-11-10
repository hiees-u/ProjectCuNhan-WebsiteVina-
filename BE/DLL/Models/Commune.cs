using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DLL.Models;

/// <summary>
/// Xã/Phường
/// </summary>
[Table("Commune")]
[Index("CommuneName", Name = "uni_CommuneName", IsUnique = true)]
public partial class Commune
{
    [Key]
    [Column("CommuneID")]
    public int CommuneId { get; set; }

    [StringLength(30)]
    public string CommuneName { get; set; } = null!;

    [Column("DistrictID")]
    public int DistrictId { get; set; }

    [InverseProperty("Commune")]
    public virtual ICollection<Address> Addresses { get; set; } = new List<Address>();

    [ForeignKey("DistrictId")]
    [InverseProperty("Communes")]
    public virtual District District { get; set; } = null!;
}
