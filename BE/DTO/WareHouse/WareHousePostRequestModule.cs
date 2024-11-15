namespace DTO.WareHouse
{
    public class WareHousePostRequestModule
    {
        public string WarehouseName { get; set; } = null!;
        public int AddressId { get; set; }
        public string? ModifiedBy { get; set; }
    }
}
