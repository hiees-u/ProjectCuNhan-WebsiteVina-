namespace DTO.Transaction;

#nullable disable
public class TransactionDto
{
    public int LocalTransactionId { get; set; }
    public int OrderId { get; set; }
    public string PaymentMethod { get; set; }
    public string Information { get; set; }
    public string TransactionId { get; set; }
    public string State { get; set; }
    public decimal Amount { get; set; }
    public DateTime CreateAt { get; set; }
}
