namespace DTO.Order
{
    public class OrderResponseModelv4
    {
        public int OrderId { get; set; }
        public string Phone { get; set; } = string.Empty;
        public string Addres { get; set; } = string.Empty;
        public decimal TotalPayment { get; set; }
        public DateTime CreateAt { get; set; }
        public bool paymentStatus { get; set; }
    }
}
