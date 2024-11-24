using DTO.Responses;
using DTO.WarehouseReceipt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interface
{
    public interface IWarehouseReceipt
    {
        BaseResponseModel InsertWarehouseReceipt(int warehouseID, List<ReceiptDetail> receiptDetails);
    }
}
