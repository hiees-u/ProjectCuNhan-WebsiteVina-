namespace DTO.Address
{
    public class AddressResponseModel
    {
        public int AddressId { get; set; }
        public int CommuneId { get; set; }
        public string HouseNumber { get; set; } = null!;
        public string? Note { get; set; }
    }
}
