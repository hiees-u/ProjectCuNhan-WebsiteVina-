namespace DTO.Category
{
    public class CategoryRequestModule
    {
        public int? CategoryId { get; set; }
        public string CategoryName { get; set; } = null!;

        public bool validateCateRes()
        {
            if(!string.IsNullOrEmpty(CategoryName)) return true;
            if(CategoryId.HasValue) return true;
            return false;
        }
    }
}
