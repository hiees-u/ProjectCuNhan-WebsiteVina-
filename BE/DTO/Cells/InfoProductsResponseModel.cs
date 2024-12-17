using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Cells
{
    public class InfoProductsResponseModel
    {
        public string ProductName { get; set; } = null!;
        public string? Image { get; set; }
        public string WarehouseName { get; set; } = null!;
        public string CellName { get; set; } = null!;
        public string ShelvesName { get; set; } = null!;
        public int? TotalQuantity { get; set; }
        public DateTime? ExpriryDate { get; set; }
    }
}
