using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DLL.Models;

/// <summary>
/// Khách Hàng
/// </summary>
[Table("Customer")]
public partial class Customer
{
    [Key]
    [Column("customerId")]
    public int CustomerId { get; set; }

    [Column("type_customer_id")]
    public int TypeCustomerId { get; set; }

    [StringLength(25)]
    [Unicode(false)]
    public string? ModifiedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreateTime { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ModifiedTime { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DeleteTime { get; set; }

    [InverseProperty("Customer")]
    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

    [InverseProperty("CreateByNavigation")]
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    [ForeignKey("TypeCustomerId")]
    [InverseProperty("Customers")]
    public virtual CustomerType TypeCustomer { get; set; } = null!;

    [InverseProperty("Customer")]
    public virtual UserInfo? UserInfo { get; set; }
}
