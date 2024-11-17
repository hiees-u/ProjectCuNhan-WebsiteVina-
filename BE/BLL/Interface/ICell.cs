using DTO.Cells;
using DTO.Responses;
using DTO.Shevle;

namespace BLL.Interface
{
    public interface ICell
    {
        public BaseResponseModel GetCellByShelve(int shelveID);
        public BaseResponseModel GetProductsByWarehouseID(int warehouseID);
        public BaseResponseModel Post(CellPostRequestModule request);
        public BaseResponseModel Put(CellRequestModule request);
        public BaseResponseModel Delete(int cellID);
    }
}
