using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DLL.Models;

/// <summary>
/// Nhà Sản Xuất
/// </summary>
[Table("Supplier")]
[Index("SupplierName", Name = "uni_ManufacturerName", IsUnique = true)]
public partial class Supplier
{
    [Key]
    [Column("SupplierID")]
    public int SupplierId { get; set; }

    [StringLength(30)]
    public string SupplierName { get; set; } = null!;

    [Column("AddressID")]
    public int AddressId { get; set; }

    public string DeleteBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DeleteTime { get; set; }

    [Column("date", TypeName = "datetime")]
    public DateTime? Date { get; set; }

    [ForeignKey("AddressId")]
    [InverseProperty("Suppliers")]
    public virtual Address Address { get; set; } = null!;

    [InverseProperty("SupplierNavigation")]
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
