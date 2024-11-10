using DTO.Responses;

namespace BLL.Interface
{
    public interface ISubCategory
    {
        public BaseResponseModel GetSubCateNameByProductID(int productID);
        public BaseResponseModel GetTop10();
    }
}
