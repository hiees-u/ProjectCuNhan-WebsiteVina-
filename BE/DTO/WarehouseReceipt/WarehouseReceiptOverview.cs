using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.WarehouseReceipt
{
    public class WarehouseReceiptOverview
    {
        public int WarehouseReceiptID { get; set; }
        public int EmployeeID { get; set; }
        public DateTime CreateAt { get; set; }
        public string WarehouseName { get; set; }
    }
}
