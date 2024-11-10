namespace DTO.Commune
{
    public class CommuneRequestModule
    {
        public string CommuneName { get; set; } = null!;
        public int DistrictId { get; set; }

        public bool isValid()
        {
            if(string.IsNullOrEmpty(CommuneName)) return false;
            if(DistrictId <= 0) return false;
            return true;
        }
    }
}
