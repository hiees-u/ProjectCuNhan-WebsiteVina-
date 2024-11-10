using DTO.Responses;
namespace DTO.User
{
    public class LoginChangePassRequestModule
    {
        public string AccountName { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string NewPassword { get; set; } = null!;
        public BaseResponseModel Validate()
        {
            if (String.IsNullOrWhiteSpace(AccountName))
                return new BaseResponseModel
                {
                    IsSuccess = false,
                    Message = "Vui lòng nhập Tên đăng nhập!"
                };
            if (String.IsNullOrWhiteSpace(Password))
                return new BaseResponseModel
                {
                    IsSuccess = false,
                    Message = "Vui lòng nhập Mật khẩu!"
                };
            if (String.IsNullOrWhiteSpace(NewPassword))
                return new BaseResponseModel
                {
                    IsSuccess = false,
                    Message = "Vui lòng nhập Mật khẩu mới!"
                };
            if(Password.Equals(NewPassword))
                return new BaseResponseModel 
                {
                    IsSuccess = false,
                    Message = "Vui lòng nhập mật khẩu cũ khác với mật khẩu mới!!." 
                };
            return new BaseResponseModel
            {
                IsSuccess = true,
            };
        }
    }
}
