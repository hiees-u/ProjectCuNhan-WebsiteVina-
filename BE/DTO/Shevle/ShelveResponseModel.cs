namespace DTO.Shevle
{
    public class ShelveResponseModel
    {
        public int ShelvesId { get; set; }
        public string ShelvesName { get; set; } = null!;
        public int WarehouseId { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
        public DateTime? DeleteTime { get; set; }
    }
}
