using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DLL.Models;

/// <summary>
/// Chức Vụ
/// </summary>
[Table("EmployeeType")]
[Index("EmployeeTypeName", Name = "uni_EmployeeTypeName", IsUnique = true)]
public partial class EmployeeType
{
    [Key]
    [Column("EmployeeTypeID")]
    public int EmployeeTypeId { get; set; }

    [StringLength(30)]
    public string EmployeeTypeName { get; set; } = null!;

    [StringLength(25)]
    [Unicode(false)]
    public string? ModifiedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreateTime { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ModifiedTime { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DeleteTime { get; set; }

    [InverseProperty("EmployeeType")]
    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
