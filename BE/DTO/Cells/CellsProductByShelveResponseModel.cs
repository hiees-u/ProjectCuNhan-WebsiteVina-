using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Cells
{
    public class CellsProductByShelveResponseModel
    {
        public int CellId { get; set; }
        public string CellName { get; set; } = null!;        
        public int? Quantity { get; set; }
        public int? ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public string? Image { get; set; }
        public int? TotalQuantity { get; set; }
        public DateTime? ExpriryDate { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
        public DateTime? DeleteTime { get; set; }

    }
}
