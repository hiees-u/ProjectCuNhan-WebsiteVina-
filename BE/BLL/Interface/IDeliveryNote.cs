using DTO.DeliveryNote;
using DTO.Responses;
using DTO.WarehouseReceipt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
