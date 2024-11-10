using System.Text.RegularExpressions;

namespace DTO.UserInfo
{
    public class UserInfoRequestModel
    {
        public string? fullName { get; set; } //--
        public string? email { get; set; } //--
        public string? phone { get; set; } //--
        public int addressId { get; set; } = 0;
        public int gender { get; set; }  //--
        public string houseNumber { get; set; } = string.Empty; //-- Check Address ID
        public string? note { get; set; } = null; //-- Check Address ID
        public string communeName { get; set; } = null!;
        public int commune { get; set; } = 0; //-- Check Address ID Can Input 0 => Insert Component
        public int districtId { get; set; } = 0; //-- Bắt Bộc trường hợp Commune chưa tồn tại => Insert Commune

        public bool IsValidEmail()
        {
            string regex = @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            return Regex.IsMatch(this.email!, regex);
        }
        public bool GenderIsValid()
        {
            return (this.gender == 0 || this.gender == 1) ? true : false;
        }
        public bool IsValidPhoneNumber()
        {
            string regex = @"^(\+84|0)(3[2-9]|5[689]|7[0-9]|8[1-9]|9[0-9])\d{7}$";
            return Regex.IsMatch(this.phone!, regex);
        }
    }
}
