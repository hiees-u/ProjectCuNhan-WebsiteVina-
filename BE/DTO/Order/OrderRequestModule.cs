namespace DTO.Order
{
    public class OrderRequestModule
    {
        public string phone { get; set; } = null!;
        public int adressId { get; set; }
        public string namerecipient { get; set; } = null!;

        public bool isLength(int maxLength, string request)
        {
            if(request.Length > maxLength) 
                return false;
            return true;
        }

        public bool IsValid()
        {
            if(string.IsNullOrWhiteSpace(namerecipient)) return false;
            if(string.IsNullOrWhiteSpace(phone)) return false;
            if(adressId <= 0) return false;

            if (!isLength(11, phone) || !isLength(50, namerecipient))
                return false;

            return true;
        }
    }
}
