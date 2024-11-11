using DTO.Order;
using DTO.Responses;

namespace BLL.Interface
{
    public interface IOrder
    {
        public BaseResponseModel Post(OrderRequestModule request);
        public BaseResponseModel Get(int orderState);
    }
}
