namespace DTO.WareHouse
{
    public class WareHouseRequestModule
    {
        public int WarehouseId { get; set; }
        public string WarehouseName { get; set; } = null!;
        public int AddressId { get; set; }
        public string? ModifiedBy { get; set; }
    }
}
