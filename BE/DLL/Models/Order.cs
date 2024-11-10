using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DLL.Models;

/// <summary>
/// Đơn Đặt Hàng Của Khách
/// </summary>
[Table("Order")]
public partial class Order
{
    [Key]
    [Column("Order_ID")]
    public int OrderId { get; set; }

    [StringLength(11)]
    public string Phone { get; set; } = null!;

    [Column("Adress_ID")]
    public int AdressId { get; set; }

    [Column("Name_Recipient")]
    [StringLength(50)]
    public string NameRecipient { get; set; } = null!;

    public int CreateBy { get; set; }

    [Column("Total_Payment", TypeName = "decimal(10, 0)")]
    public decimal TotalPayment { get; set; }

    [Column("Create_At", TypeName = "datetime")]
    public DateTime? CreateAt { get; set; }

    public int State { get; set; }

    [ForeignKey("AdressId")]
    [InverseProperty("Orders")]
    public virtual Address Adress { get; set; } = null!;

    [ForeignKey("CreateBy")]
    [InverseProperty("Orders")]
    public virtual Customer CreateByNavigation { get; set; } = null!;

    [InverseProperty("Order")]
    public virtual ICollection<DeliveryNote> DeliveryNotes { get; set; } = new List<DeliveryNote>();

    [InverseProperty("Order")]
    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}
