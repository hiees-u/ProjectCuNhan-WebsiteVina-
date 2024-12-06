using DTO.Responses;
using DTO.WareHouse;

namespace BLL.Interface
{
    public interface IWareHouse
    {
        public BaseResponseModel Get();
        public BaseResponseModel GetWareHouseID(int warehouseID);
        public BaseResponseModel GetWareHouseByName(string warehouseName);
        public BaseResponseModel Post(WareHousePostRequestModule request);
        public BaseResponseModel Put(WareHouseRequestModule request);
        public BaseResponseModel Delete(int warehouseId);
    }
}
