using BLL.Interface;
using Microsoft.AspNetCore.Mvc;
using DTO.Order;
using API.Services;
using API.Models;

namespace API.Controllers
{
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
                if (model.PaymentMethod == "vnpay")
                {
                    var payModel = new PaymentInformationModel
                    {
                        Amount = double.Parse(model.PaymentMethod),
                        Name = model.OrderId,
                        OrderDescription = model.OrderInfomation,
                        OrderType = "other"
                    };

                    var res = _vnPayService.CreatePaymentUrl(payModel, HttpContext);

                    // Create Draf Order Here

                    if (!string.IsNullOrEmpty(res))
                    {
                        return Ok(new { PayUrl = res });
                    }
                }

                // Gọi dịch vụ tạo URL thanh toán MOMO
                var response = await _momoService.CreatePaymentMomo(model);

                // Kiểm tra phản hồi từ MOMO
                if (response != null && !string.IsNullOrEmpty(response.PayUrl))
                {
                    return Ok(new { PayUrl = response.PayUrl });
                }

                // Trường hợp phản hồi không hợp lệ
                return BadRequest(new
                {
                    message = "Không thể tạo URL thanh toán MOMO!",
                    errorCode = response?.ErrorCode ?? -1, // Nếu có mã lỗi từ MOMO
                    errorMessage = response?.Message ?? "Phản hồi không hợp lệ."
                });
            }
            catch (Exception ex)
            {
                // Ghi log lỗi chi tiết
                Console.WriteLine("Lỗi trong quá trình xử lý thanh toán:");
                Console.WriteLine($"- Message: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"- InnerException: {ex.InnerException.Message}");
                }
                Console.WriteLine($"- StackTrace: {ex.StackTrace}");

                // Trả về mã lỗi HTTP 500 và thông tin lỗi chi tiết
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

        [HttpGet]
        public IActionResult PaymentCallBacks() 
        {
            var response = _momoService.PaymentExecuteAsync();
            return View(response);
        }

    }
}
