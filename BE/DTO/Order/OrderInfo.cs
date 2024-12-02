namespace DTO.Order
{
    public class OrderInfo
    {
        public string FullName { get; set; }
        public string OrderId { get; set; }
        
        public string OrderInfomation { get; set; }

        public string Amount { get; set; }
        public string PaymentMethod { get; set; } = "vnpay";
    }
}
