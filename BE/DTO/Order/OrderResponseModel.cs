namespace DTO.Order
{
    public class OrderResponseModel
    {
        public int orderId { get; set; }
        public string phone { get; set; } = null!;
        public int adressId { get; set; }
        public string nameRecipient { get; set; } = null!;
        public int createBy { get; set; }
        public decimal totalPayment { get; set; }
        public DateTime? createAt { get; set; }
        public int state { get; set; }
    }
}
