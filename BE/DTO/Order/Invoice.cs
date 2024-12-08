namespace DTO.Order
{
    public class Invoice
    {
        public int OrderId { get; set; }
        public string customerName { get; set; } = string.Empty; // tên người nhận hàng
        public string customerAddress { get; set; } = string.Empty; // địa chỉ
    }
}
