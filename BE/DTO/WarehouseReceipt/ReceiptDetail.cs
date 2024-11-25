using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.WarehouseReceipt
{
    public class ReceiptDetail
    {
        public int ProductID { get; set; }
        public int Quantity { get; set; }
        public int CellID { get; set; }
        public int PurchaseOrderId { get; set; }
    }
}
