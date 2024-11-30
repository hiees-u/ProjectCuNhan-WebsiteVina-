using BLL.Interface;
using Microsoft.AspNetCore.Mvc;
using DTO.Order;

namespace API.Controllers
{
    public class PaymentController : Controller
    {
        private IMomoService _momoService;
        public PaymentController(IMomoService momoService)
        {
            _momoService = momoService;

        }

        [HttpPost]
        [Route("CreatePaymentUrl")]
        public async Task<IActionResult> CreatePaymentMomo([FromBody] OrderInfo model)
        {
            try
            {
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

        [HttpGet]
        public IActionResult PaymentCallBacks() 
        {
            var response = _momoService.PaymentExecuteAsync();
            return View(response);
        }

    }
}
