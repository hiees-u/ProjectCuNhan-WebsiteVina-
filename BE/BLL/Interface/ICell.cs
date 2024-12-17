using DTO.Cells;
using DTO.Responses;

namespace BLL.Interface
{
    public interface ICell
    {
        public BaseResponseModel GetAllProducts();
        public BaseResponseModel GetProductsExpriryDate();
        public BaseResponseModel GetInfoProductsByProductID(int productId);
        public BaseResponseModel GetCellByShelve(int shelveID);
        public BaseResponseModel GetProductsByWarehouseID(int warehouseID);
        public BaseResponseModel Post(DTO.Cells.CellPostRequestModule request);
        public BaseResponseModel Put(CellRequestModule request);
        public BaseResponseModel Delete(int cellID);
    }
}
