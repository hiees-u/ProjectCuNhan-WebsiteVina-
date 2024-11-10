using DTO.Responses;

namespace BLL.Interface
{
    public interface ICategory
    {
        public BaseResponseModel GetCateNameByProductID(int productID);
        public BaseResponseModel GetTop10();
    }
}
