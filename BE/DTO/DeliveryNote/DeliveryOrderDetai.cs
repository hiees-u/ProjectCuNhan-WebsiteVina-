using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.DeliveryNote
{
    public class DeliveryOrderDetai
    {
        public int OrderID   { get; set; }
        public int PriceHistoryId { get; set; }
        public int Quantity { get; set; }
        public int CellID { get; set; }        
    }
}
