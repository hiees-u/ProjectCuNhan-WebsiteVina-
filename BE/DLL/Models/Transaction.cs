using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DLL.Models;

[Table("Transactions")]
[PrimaryKey("LocalTransactionId")]
public class Transaction
{
    [Key]
    [Column("LocalTransactionId")]
    public int LocalTransactionId { get; set; }

    [Column("OrderId")]
    public int OrderId { get; set; }

    [Column("PaymentMethod")]
    public string PaymentMethod { get; set; }

    [Column("Information")]
    public string Information { get; set; }

    [Column("TransactionId")]
    public string TransactionId { get; set; }

    [Column("State")]
    public string State { get; set; }

    [Column("CreateAt")]
    public DateTime CreateAt { get; set; }
}
