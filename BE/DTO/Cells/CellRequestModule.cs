using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Cells
{
    public class CellRequestModule
    {
        public int CellId { get; set; }
        public string CellName { get; set; } = null!;
        public int ShelvesId { get; set; }
        public int? Quantity { get; set; }
        public int? ProductId { get; set; } 
    }
}
