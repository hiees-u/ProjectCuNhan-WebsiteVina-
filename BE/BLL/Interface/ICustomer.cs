using DTO.Customer;
using DTO.Responses;

namespace BLL.Interface
{
    public interface ICustomer
    {
        public BaseResponseModel Get(
            int? TypeCustomerId,
            int pageNumber = 1,
            int pageSize = 8
        );
        public BaseResponseModel Put(CustomerRequestModule req);

        public BaseResponseModel Delete(string userName);
    }
}
