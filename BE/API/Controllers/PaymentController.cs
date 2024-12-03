using BLL.Interface;
using Microsoft.AspNetCore.Mvc;
using DTO.Order;
using API.Services;
using API.Models;
using DLL.Models;
using DTO.Transaction;
using Mapster;
using Mapster;

namespace API.Controllers
{
    [Route("api/Payment")]
    [ApiController]
    public class PaymentController : Controller
    {
        private IMomoService _momoService;
        private readonly IVnPayService _vnPayService;
        private readonly ITransactionBLL _transactionBLL;
        private readonly IOrder _order;

        public PaymentController(IMomoService momoService, IVnPayService vnPayService, ITransactionBLL transactionBLL, IOrder order)
        {
            _momoService = momoService;
            _vnPayService = vnPayService;
            _transactionBLL = transactionBLL;
            _order = order;
        }

        [HttpPost]
        [Route("CreatePaymentUrl")]
        public async Task<IActionResult> CreatePaymentMomo([FromBody] OrderInfo model)
        {
            try
            {
                // 1. Tạo bản nháp (draft) của đơn hàng
                var draftOrder = new Order
                {
                    TotalPayment = decimal.Parse(model.Amount),
                    State = 0,
                    CreateAt = DateTime.Now                   // Thời điểm tạo đơn hàng
                };

                // Lưu bản nháp vào cơ sở dữ liệu
                _order.CreateOrder(draftOrder); // Hàm này sẽ lưu vào DB và sinh OrderId tự động

                // Lấy OrderId sau khi lưu
                var orderId = draftOrder.OrderId;

                // 2. Xử lý thanh toán dựa trên phương thức thanh toán
                if (model.PaymentMethod == "vnpay")
                {
                    var payModel = new PaymentInformationModel
                    {
                        Amount = double.Parse(model.Amount),   // Số tiền cần thanh toán
                        Name = model.FullName,
                        OrderDescription = model.OrderInfomation,
                        OrderType = "other",
                        OrderId = orderId.ToString() // Truyền OrderId dưới dạng chuỗi
                    };

                    var res = _vnPayService.CreatePaymentUrl(payModel, HttpContext);

                    if (!string.IsNullOrEmpty(res))
                    {
                        return Ok(new { PayUrl = res, OrderId = orderId }); // Trả về cả PayUrl và OrderId
                    }
                }

                // Gọi dịch vụ tạo URL thanh toán MOMO
                var response = await _momoService.CreatePaymentMomo(model);

                // Kiểm tra phản hồi từ MOMO
                if (response != null && !string.IsNullOrEmpty(response.PayUrl))
                {
                    return Ok(new { PayUrl = response.PayUrl, OrderId = orderId });
                }

                // Trường hợp phản hồi không hợp lệ
                return BadRequest(new
                {
                    message = "Không thể tạo URL thanh toán!",
                    errorCode = response?.ErrorCode ?? -1,
                    errorMessage = response?.Message ?? "Phản hồi không hợp lệ."
                });
            }
            catch (Exception ex)
            {
                // Log lỗi chi tiết
                Console.WriteLine("Lỗi trong quá trình xử lý thanh toán:");
                Console.WriteLine($"- Message: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"- InnerException: {ex.InnerException.Message}");
                }
                Console.WriteLine($"- StackTrace: {ex.StackTrace}");

                // Trả về mã lỗi HTTP 500
                return StatusCode(500, new
                {
                    message = "Có lỗi xảy ra trong quá trình thanh toán!",
                    detailedMessage = ex.Message,
                    innerException = ex.InnerException?.Message
                });
            }
        }



        [HttpPost]
        [Route("ProcessCheckout")]
        public IActionResult ProcessCheckout()
        {
            var query = Request.Query;
            var paymentMethod = Request.Query.FirstOrDefault(s => s.Key.Equals("payment_method")).Value.ToString();
            if (paymentMethod == "vnpay")
            {
                var vnpayResponse = _vnPayService.PaymentExecute(Request.Query);
                var dto = new DTO.Transaction.TransactionDto()
                {
                    CreateAt = DateTime.Now,
                    Information = vnpayResponse.OrderDescription,
                    PaymentMethod = vnpayResponse.PaymentMethod,
                    State = vnpayResponse.Success ? "Thanh toán thành công" : "Thanh toán thất bại",
                    TransactionId = vnpayResponse.TransactionId,
                };

                var trans = _transactionBLL.CreateTransaction(dto);

                // Update Draf Order Here

                return Created();
            }

            var response = _momoService.PaymentExecuteAsync();
            return View(response);
        }

        [HttpPost]
        [Route("Callback")]
        public IActionResult VnPayCallback([FromQuery] IQueryCollection queryParams)
        {
            try
            {
                // Kiểm tra các tham số từ VNPay
                string orderId = queryParams["order_id"];
                string amount = queryParams["vnp_Amount"];
                string responseCode = queryParams["vnp_ResponseCode"];

                if (string.IsNullOrEmpty(orderId) || string.IsNullOrEmpty(amount) || responseCode != "00")
                {
                    return BadRequest(new { success = false, message = "Thông tin không hợp lệ!" });
                }

                // Lưu thông tin đơn hàng và giao dịch vào cơ sở dữ liệu
                var order = new Order
                {
                    OrderId = int.Parse(orderId),
                    TotalPayment = decimal.Parse(amount),
                    State = responseCode == "00" ? 1 : 0,
                    CreateAt = DateTime.Now,
                };
                _order.UpdateOrder(order);

                var transaction = new Transaction
                {
                    OrderId = int.Parse(orderId),
                    TransactionId = queryParams["vnp_TransactionNo"],
                    PaymentMethod = "vnpay",
                    State = "Thành công",
                    CreateAt = DateTime.Now
                };
                var transactionDto = transaction.Adapt<TransactionDto>();
                _transactionBLL.CreateTransaction(transactionDto);

                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500, new { success = false, message = "Lỗi xử lý callback." });
            }
        }

    }


}

