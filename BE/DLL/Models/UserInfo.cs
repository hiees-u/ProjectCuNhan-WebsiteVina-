using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DLL.Models;

/// <summary>
/// Thông Tin Người Dùng
/// </summary>
[Table("UserInfo")]
[Index("EmployId", Name = "UQ_UserInfo_Employ_ID", IsUnique = true)]
[Index("CustomerId", Name = "UQ_UserInfo_customer_Id", IsUnique = true)]
public partial class UserInfo
{
    [Key]
    [StringLength(25)]
    [Unicode(false)]
    public string AccountName { get; set; } = null!;

    [Column("full_name")]
    [StringLength(50)]
    public string? FullName { get; set; }

    [Column("email")]
    [StringLength(50)]
    public string? Email { get; set; }

    [Column("address_id")]
    public int AddressId { get; set; }

    [Column("phone")]
    [StringLength(11)]
    [Unicode(false)]
    public string? Phone { get; set; }

    [Column("gender")]
    public int Gender { get; set; }

    [Column("Employ_ID")]
    public int? EmployId { get; set; }

    [Column("customer_Id")]
    public int? CustomerId { get; set; }

    [StringLength(25)]
    [Unicode(false)]
    public string? ModifiedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreateTime { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ModifiedTime { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DeleteTime { get; set; }

    [ForeignKey("AccountName")]
    [InverseProperty("UserInfo")]
    public virtual User AccountNameNavigation { get; set; } = null!;

    [ForeignKey("AddressId")]
    [InverseProperty("UserInfos")]
    public virtual Address Address { get; set; } = null!;

    [ForeignKey("CustomerId")]
    [InverseProperty("UserInfo")]
    public virtual Customer? Customer { get; set; }

    [ForeignKey("EmployId")]
    [InverseProperty("UserInfo")]
    public virtual Employee? Employ { get; set; }
}
