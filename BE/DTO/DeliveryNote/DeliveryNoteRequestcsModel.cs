using DTO.WarehouseReceipt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.DeliveryNote
{
    public class DeliveryNoteRequestcsModel
    {
        public int WarehouseID { get; set; }
        public string? Note { get; set; }
        public List<DeliveryOrderDetai> DeliveryNoteDetail { get; set; }
    }
}
