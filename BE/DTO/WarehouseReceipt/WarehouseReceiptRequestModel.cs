using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.WarehouseReceipt
{
    public class WarehouseReceiptRequestModel
    {
        public int WarehouseID { get; set; }
        public List<ReceiptDetail> ReceiptDetails { get; set; }
    }
}
