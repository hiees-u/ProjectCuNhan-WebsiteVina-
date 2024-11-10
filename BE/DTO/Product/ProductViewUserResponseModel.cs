namespace DTO.Product
{
    public class ProductViewUserResponseModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public string? Image { get; set; }
        public int? TotalQuantity { get; set; }
        public int CategoryId { get; set; }
        public int Supplier { get; set; }
        public int SubCategoryId { get; set; }
        public string? Description { get; set; }
        public string? ModifiedBy { get; set; }
        public decimal Price { get; set; }
        public int priceHistoryId { get; set; }
        public DateTime? ExpriryDate { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
        public DateTime? DeleteTime { get; set; }
    }
}
