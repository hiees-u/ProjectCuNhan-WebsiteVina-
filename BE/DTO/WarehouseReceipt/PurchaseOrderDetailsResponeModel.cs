using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.WarehouseReceipt
{
    public class PurchaseOrderDetailsResponeModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public int? QuantityOrdered { get; set; }
        public int? QuantityDelivered { get; set; }
        public int CellId { get; set; }
        public string CellName { get; set; } = null!;
        public int PriceHistoryId { get; set; }
        public decimal Price { get; set; }
        public int? QuantityToImport { get; set; }
    }
}
