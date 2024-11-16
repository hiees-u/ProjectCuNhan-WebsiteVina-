using DTO.CustomerType;
using DTO.Responses;

namespace BLL.Interface
{
    public interface ICustomerType
    {
        public BaseResponseModel Get(int? customerID);
        public BaseResponseModel Post(CustomerTypeRequestModule req);
        public BaseResponseModel Put(CustomerTypeResponseModule req);
        public BaseResponseModel Delete(int? customerID);
    }
}
