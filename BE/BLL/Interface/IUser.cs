using DTO.Register;
using DTO.Responses;
using DTO.User;

namespace BLL.Interface
{
    public interface IUser
    {
        public BaseResponseModel Login(LoginRequestModule module);

        public BaseResponseModel Register(RegisterRequestModule customer);

        public BaseResponseModel ChangePassword(LoginChangePassRequestModule module);

        public BaseResponseModel LogOut();

        public BaseResponseModel GetRole(string token);
        public BaseResponseModel GetFullName();
    }
}
