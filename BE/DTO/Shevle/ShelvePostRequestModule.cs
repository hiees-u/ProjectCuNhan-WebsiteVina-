namespace DTO.Shevle
{
    public class ShelvePostRequestModule
    {
        public string ShelvesName { get; set; } = null!;
        public int WarehouseId { get; set; }
        public string? ModifiedBy { get; set; }
    }
}
