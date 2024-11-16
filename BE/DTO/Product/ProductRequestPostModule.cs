using DLL.Models;

namespace DTO.Product
{
    public class ProductRequestPostModule
    {
        public string ProductName { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public int? CategoryId { get; set; }
        public int? Supplier { get; set; }
        public int? SubCategoryId { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal? Price { get; set; }

        public bool IsValid()
        {
            if (string.IsNullOrWhiteSpace(ProductName) || ProductName.Length > 50) return false;
            if (string.IsNullOrWhiteSpace(Image) || ProductName.Length > 50) return false;
            if (!CategoryId.HasValue || CategoryId <= 0) return false;
            if (!Supplier.HasValue || Supplier <= 0) return false;
            if (!SubCategoryId.HasValue || SubCategoryId <= 0) return false;
            if (!ExpiryDate.HasValue || ExpiryDate.Value == default(DateTime)) return false;
            if (string.IsNullOrWhiteSpace(Description)) return false;
            if (!Price.HasValue || Price <= 0) return false;

            return true;
        }
    }
}
