namespace DTO.Order
{
    public class OrderResponseModelv3
    {
        public int? orderId { get; set; }
        public string phone {  get; set; }
        public string nameRecip { get; set; }
        public decimal total { get; set; }
        public DateTime created { get; set; }
        public string createBy { get; set; }
    }
}
