namespace DTO.Cart
{
    public class CartResponseModel
    {
        public int ProductId { get; set; }
        public int? Quantity { get; set; } = 0;
        public string ProductName { get; set; } = string.Empty;
        public string? Image { get; set; }
        public decimal? Price { get; set; } = 0;
        public int priceHistoryId { get; set; }
    }
}
