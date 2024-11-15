namespace DTO.Subcategory
{
    public class SubcategoryRequestModel
    {
        public int? SubCategoryId { get; set; }
        public string SubCategoryName { get; set; } = null!;

        public bool isValidate()
        {
            if(SubCategoryId.HasValue)
                return true;
            if(!string.IsNullOrWhiteSpace(SubCategoryName)) return true; return false;
        }
    }
}
