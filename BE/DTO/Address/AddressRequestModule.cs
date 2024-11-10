namespace DTO.Address
{
    public class AddressRequestModule
    {
        public int CommuneId { get; set; } = 0;
        public string HouseNumber { get; set; } = null!;
        public string? Note { get; set; }
        public string CommuneName { get; set; } = null!;
        public int DistrictId { get; set; } = 0;
    }
}
