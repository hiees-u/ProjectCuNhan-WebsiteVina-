using DTO.DeliveryNote;
using DTO.Responses;

namespace BLL.Interface
{
    public interface IDeliveryNote
    {
        BaseResponseModel InsertDeliveryNote(int warehouseID, string note ,List<DeliveryOrderDetai> deliveryOrderDetails);
        //BaseResponseModel Delete(int WarehouseReceiptID);
        //BaseResponseModel GetWarehouseReceiptInfo(int WarehouseReceiptID);
        //BaseResponseModel GetWarehouseReceiptsByWarehouse(int warehouseID);
    }
}
