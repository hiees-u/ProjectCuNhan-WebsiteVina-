
namespace DTO.Payment
{
    public class MomoInfoModel
    {
        public string OrderId { get; set; }
        public decimal Amount { get; set; }
        public DateTime DatePaid { get; set; }
        public bool PaymentStatus { get; set; }
    }
}
