using DLL.Models;
using DTO.Order;
using DTO.Responses;

namespace BLL.Interface
{
    public interface IOrder
    {
        public BaseResponseModel Post(OrderRequestModule request);
        public BaseResponseModel Get(int orderState);
        public BaseResponseModel Delete(int OrderId, int PriceHistory);

        BaseResponseModel UpdatePaymentStatus(string orderId, bool isPaid);
        BaseResponseModel UpdateOrder(Order order);

        BaseResponseModel CreateOrder(Order order);
        public BaseResponseModel GetByOrderApprover();

        //get order detail
        public BaseResponseModel GetOrderDetailByOA(int oID);

        public string GenerateInvoice(Invoice invoice);
        public BaseResponseModel UpdateStateOrderByOA(int orderID);
        public BaseResponseModel getOrdersByTS(int pageNumber = 1, int pageSize = 8);
        public BaseResponseModel Delivery(int orderID);
    }
}
