namespace DTO.Order
{
    public class OrderResponseModelv2
    {
        public string productname { get; set; }
        public string image { get; set; }
        public decimal price { get; set; }
        public int quantity { get; set; }
        public decimal totalprice { get; set; }
        public int state { get; set; }
        public int orderid { get; set; }
    }
}
