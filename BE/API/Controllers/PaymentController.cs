using BLL.Interface;
using Microsoft.AspNetCore.Mvc;
using DTO.Order;
using Newtonsoft.Json;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
        public IActionResult PaymentCallBacks() 
        {
            var response = _momoService.PaymentExecuteAsync();
            return View(response);

            //var response = _momoService.PaymentExecuteAsync(HttpContext.Request.Query);
            //var requestQuery = HttpContext.Request.Query;
            //if (requestQuery["resultCode"] == 0)
            //{
            //    var newMomoInsert = new MomoInfoModel
            //    {
            //        OrderId = requestQuery["orderId"],
            //        FullName = User.FindFirstValue(ClaimTypes.Email),
            //        Amount = decimal.Parse(requestQuery["Amount"]),
            //        OrderInfo = requestQuery["orderInfo"],
            //        DatePaid = DateTime.Now
            //    };
            //    _dataContext.Add(newMomoInsert);
            //    await _dataContext.SaveChangesAsync();
            //}
            //else
            //{
            //    TempData["success"] = "Đã hủy giao dịch Momo.";
            //    return RedirectToAction("Index", "Cart");
            //}

            //// Call checkout method after saving MomoInfo
            //var checkoutResult = await Checkout(requestQuery["orderId"]);
            //return View(response);
        }

    }
}
