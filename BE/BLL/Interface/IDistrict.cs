using DTO.Responses;

namespace BLL.Interface
{
    public interface IDistrict
    {
        public BaseResponseModel Gets();

        public BaseResponseModel GetDistrictByProvinceID(int provinceID);
    }
}
