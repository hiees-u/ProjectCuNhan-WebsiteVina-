using DTO.Address;
using DTO.Responses;

namespace BLL.Interface
{
    public interface IAddress
    {
        public BaseResponseModel GetById(int iD);
        public BaseResponseModel Post(AddressRequestModule req);
        public BaseResponseModel GetAddressID(AddressRequestModule request);
    }
}
