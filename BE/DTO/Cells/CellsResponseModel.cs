namespace DTO.Cells
{
    public class CellsResponseModel
    {
        public int CellId { get; set; }
        public string CellName { get; set; } = null!;
        public int ShelvesId { get; set; }
        public int? Quantity { get; set; }
        public int? ProductId { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
        public DateTime? DeleteTime { get; set; }
    }
}
