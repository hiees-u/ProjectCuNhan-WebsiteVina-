namespace DTO.UserInfo
{
    public class UserInfoResponseModel
    {
        public string accountName { get; set; } = null!;
        public string? fullName { get; set; }
        public string? email { get; set; }
        public string? address { get; set; }
        public int addressId { get; set; }
        public string? customerType { get; set; }
        public string? phone { get; set; }
        public int gender { get; set; }
        public int commune { get; set; }
        public int district { get; set; }
        public int province { get; set; }
    }
}
