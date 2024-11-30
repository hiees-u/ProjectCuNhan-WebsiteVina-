using DTO.Order;
using DTO.Payment;
using DTO.Responses;

namespace BLL.Interface
{
    public interface IOrder
    {
        public BaseResponseModel Post(OrderRequestModule request);
        public BaseResponseModel Get(int orderState);
        public BaseResponseModel Delete(int OrderId, int PriceHistory);
        Task<bool> UpdateOrderPaymentStatus(string orderId, MomoInfoModel paymentInfo);
    }
}
