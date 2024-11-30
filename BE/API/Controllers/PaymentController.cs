using BLL.Interface;
using Microsoft.AspNetCore.Mvc;
using DTO.Order;
using Newtonsoft.Json;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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
            Console.WriteLine("Dữ liệu nhận từ Angular:");
            Console.WriteLine(JsonConvert.SerializeObject(model)); // Log dữ liệu nhận

            try
            {
                var response = await _momoService.CreatePaymentMomo(model);
                Console.WriteLine("Phản hồi từ Momo:");
                Console.WriteLine(JsonConvert.SerializeObject(response)); // Log phản hồi từ Momo

                if (response != null && !string.IsNullOrEmpty(response.PayUrl))
                {
                    return Ok(new { PayUrl = response.PayUrl });
                }

                return BadRequest(new
                {
                    message = "Không thể tạo URL thanh toán MOMO!",
                    errorCode = response?.ErrorCode ?? -1,
                    errorMessage = response?.Message ?? "Phản hồi không hợp lệ."
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi xử lý trong PaymentController:");
                Console.WriteLine(ex.Message); // Log lỗi
                Console.WriteLine(ex.StackTrace);

                return StatusCode(500, new { message = "Có lỗi xảy ra trong quá trình xử lý!", detailedMessage = ex.Message });
            }
        }

        [HttpGet]
        [Route("Get")]
        public IActionResult PaymentCallBacks() 
        {
            var response = _momoService.PaymentExecuteAsync();
            return View(response);
        }

    }
}
