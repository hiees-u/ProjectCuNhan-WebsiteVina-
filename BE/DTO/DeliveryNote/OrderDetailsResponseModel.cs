using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.DeliveryNote
{
    public class OrderDetailsResponseModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public int Quantity { get; set; }
        public int CellId { get; set; }
        public string CellName { get; set; } = null!;
        public int WarehouseId { get; set; }
        public string WarehouseName { get; set; } = null!;
        public int PriceHistoryId { get; set; }

    }
}
