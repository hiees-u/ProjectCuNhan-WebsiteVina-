using BLL.Interface;
using Microsoft.AspNetCore.Mvc;
using DTO.Order;
using Newtonsoft.Json;
using System.Security.Claims;
using DTO.Payment;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : Controller
    {
        private IMomoService _momoService;
        private IOrder _orderService;
        public PaymentController(IMomoService momoService, IOrder orderService)
        {
            _momoService = momoService;
            _orderService = orderService;

        }

        [HttpPost]
        [Route("CreatePaymentUrl")]
        public async Task<IActionResult> CreatePaymentMomo([FromBody] OrderInfo model)
        {
            Console.WriteLine("Dữ liệu nhận từ Angular:");
            Console.WriteLine(JsonConvert.SerializeObject(model)); // Log dữ liệu nhận

            try
            {
                // Gọi dịch vụ tạo URL thanh toán MOMO
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
        [Route("Save")]
        public async Task<IActionResult> PaymentCallBacks([FromQuery] string orderId, [FromQuery] decimal amount, [FromQuery] int resultCode)
        {
            try
            {
                if (resultCode == 0) // Thành công
                {
                    // Lưu dữ liệu vào DB
                    var paymentInfo = new MomoInfoModel
                    {
                        OrderId = orderId,
                        Amount = amount,
                        DatePaid = DateTime.Now,
                        PaymentStatus = true
                    };

                    await _orderService.UpdateOrderPaymentStatus(orderId, paymentInfo);

                    return Ok(new { message = "Giao dịch thành công!" });
                }
                else
                {
                    return BadRequest(new { message = "Giao dịch thất bại!" });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi xử lý callback:");
                Console.WriteLine(ex.Message);
                return StatusCode(500, new { message = "Lỗi xử lý!", detailedMessage = ex.Message });
            }
        }


    }
}
