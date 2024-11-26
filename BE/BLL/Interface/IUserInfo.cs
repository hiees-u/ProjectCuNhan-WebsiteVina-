using DTO.Responses;
using DTO.UserInfo;

namespace BLL.Interface
{
    public interface IUserInfo
    {
        public BaseResponseModel Get();
        public BaseResponseModel Put(UserInfoRequestModel req);
        public BaseResponseModel GetAccontName();
    }
}
