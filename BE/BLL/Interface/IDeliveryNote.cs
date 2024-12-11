using DTO.DeliveryNote;
using DTO.Responses;

namespace BLL.Interface
{
    public interface IDeliveryNote
    {
        BaseResponseModel InsertDeliveryNote(int warehouseID, string note ,List<DeliveryOrderDetai> deliveryOrderDetails);
        BaseResponseModel GetOrderIDs();
        BaseResponseModel GetOrderDetail(int orderID);

        //BaseResponseModel GetWarehouseReceiptsByWarehouse(int warehouseID);
    }
}
