namespace DTO.WareHouse
{
    public class WareHouseResponseModel
    {
        public int WarehouseId { get; set; }
        public string WarehouseName { get; set; } = null!;
        public int Address { get; set; }
        public string FullAddress { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
    }
}
