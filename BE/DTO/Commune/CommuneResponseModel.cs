namespace DTO.Commune
{
    public class CommuneResponseModel
    {
        public int CommuneId { get; set; }
        public string CommuneName { get; set; } = null!;
        public int DistrictId { get; set; }
    }
}
