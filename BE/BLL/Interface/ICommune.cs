using DTO.Commune;
using DTO.Responses;

namespace BLL.Interface
{
    public interface ICommune
    {
        public BaseResponseModel Gets();
        public BaseResponseModel GetCommunesByDistrictIDAsync(int districID);

        public BaseResponseModel PostCommune(CommuneRequestModule req);
    }
}
