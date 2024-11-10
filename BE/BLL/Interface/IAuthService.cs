using DTO;

namespace BLL.Interface
{
    public interface IAuthService
    {
        public string GenerateJwtToken(AuthModel model);
        public (string UserName, string Role) DecodeToken(string token);
    }
}
