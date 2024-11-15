using DTO.Responses;
using DTO.Subcategory;

namespace BLL.Interface
{
    public interface ISubCategory
    {
        public BaseResponseModel GetSubCateNameByProductID(int productID);
        public BaseResponseModel GetTop10();
        public BaseResponseModel GetPagition(int? cateId = null, string? cateName = null, int pageNumber = 1, int pageSize = 8);
        public BaseResponseModel Put(SubcategoryRequestModel model);
        public BaseResponseModel Post(string SubcategoryName);
        public BaseResponseModel Delete(int subCateId);
    }
}
