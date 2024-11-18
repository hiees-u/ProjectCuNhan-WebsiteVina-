using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Cells
{
    public class ProductOfWarehouseResponseModel
    {
        public string WarehouseName { get; set; } = null!;
        public string ShelvesName { get; set; } = null!;
        public string CellName { get; set; } = null!;
        public string ProductName { get; set; } = null!;
        public string? Image { get; set; }
        public int? Quantity { get; set; }
        public int? TotalQuantity { get; set; }
        public DateTime? ExpriryDate { get; set; }
        public string? CellModifiedBy { get; set; }
        public DateTime CellCreateTime { get; set; }
        public DateTime? CellModifiedTime { get; set; }
        public DateTime? CellDeleteTime { get; set; }
    }
}
