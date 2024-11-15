using DTO.Responses;
using DTO.Shevle;

namespace BLL.Interface
{
    public interface IShelve
    {
        public BaseResponseModel GetShelveOfWarehousehouse(int warehouseID);
        public BaseResponseModel Post(ShelvePostRequestModule request);
        public BaseResponseModel Put(ShelveRequestModule request);
        public BaseResponseModel Delete(int shelveID);
    }
}
