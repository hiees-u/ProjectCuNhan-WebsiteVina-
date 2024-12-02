using BLL.Interface;
using Microsoft.AspNetCore.Mvc;
using DTO.Order;
using API.Services;
using API.Models;
using Newtonsoft.Json;
using DTO.Payment;

namespace API.Controllers
{
    [Route("api/[controller]")]
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
            Console.WriteLine("Dữ liệu nhận từ Angular:");
            Console.WriteLine(JsonConvert.SerializeObject(model)); // Log dữ liệu nhận

            try
            {
                if (model.PaymentMethod == "vnpay")
                {
                    var payModel = new PaymentInformationModel
                    {
                        Amount = double.Parse(model.Amount),
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
                    OrderId = int.Parse(vnpayResponse.OrderId),
                };

                var trans = _transactionBLL.CreateTransaction(dto);

                var orderId = vnpayResponse?.OrderId;
                // Get Order by id
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
