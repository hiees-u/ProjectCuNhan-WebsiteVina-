using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DLL.Models;

/// <summary>
/// Loại Khách Hàng
/// </summary>
[Table("CustomerType")]
[Index("TypeCustomerName", Name = "uni_type_customer_name", IsUnique = true)]
public partial class CustomerType
{
    [Key]
    [Column("type_customer_id")]
    public int TypeCustomerId { get; set; }

    [Column("type_customer_name")]
    [StringLength(30)]
    public string TypeCustomerName { get; set; } = null!;

    [InverseProperty("TypeCustomer")]
    public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();
}
