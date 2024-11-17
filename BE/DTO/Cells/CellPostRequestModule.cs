
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Cells
{
    public class CellPostRequestModule
    {
        public string CellName { get; set; } = null!;
        public int ShelvesId { get; set; }
        public int? Quantity { get; set; }
        public int? ProductId { get; set; }
        public string? ModifiedBy { get; set; }
    }
}
