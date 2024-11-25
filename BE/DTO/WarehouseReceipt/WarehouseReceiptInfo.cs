using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.WarehouseReceipt
{
    public class WarehouseReceiptInfo
    {
        public int WarehouseReceiptID { get; set; }
        public int EmployeeID { get; set; }
        public DateTime CreateAt { get; set; }
        public int WarehouseID { get; set; }
    }
}
