using Azure.Core;
using BLL.Interface;
using DTO.Order;
using DTO.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrder order;

        public OrderController(IOrder order)
        {
            this.order = order;
        }

        [HttpGet]
        [Authorize(Roles = "Customer")]
        public IActionResult Get(int orderState)
        {
            BaseResponseModel model = order.Get(orderState);

            return model.IsSuccess ? Ok(model) : BadRequest(model);
        }

        [HttpPost]
        [Authorize(Roles = "Customer")]
        public IActionResult Order([FromBody] OrderRequestModule request)
        {
            BaseResponseModel model = order.Post(request);

            return model.IsSuccess ? Ok(model) : NotFound(model);
        }

        [HttpDelete]
        [Authorize(Roles = "Customer,OrderApprover")]
        public IActionResult Delete(int OrderId, int PriceHistory)
        {
            BaseResponseModel response = order.Delete(OrderId, PriceHistory);

            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        [HttpGet("Get by OrderApprover")]
        public IActionResult GetbyOrderApprover()
        {
            BaseResponseModel res = order.GetByOrderApprover();

            return res.IsSuccess ? Ok(res) : BadRequest(res);
        }

        [HttpGet("Get Order Detail by OrderApprover")]
        public IActionResult GetOrderDetailbyOrderApprover(int Oid)
        {
            BaseResponseModel res = order.GetOrderDetailByOA(Oid);

            return res.IsSuccess ? Ok(res) : BadRequest(res);
        }

        [HttpPost("GenerateInvoice")]
        [Authorize(Roles = "OrderApprover")]
        public IActionResult GenerateInvoice([FromBody] Invoice invoice)
        {
            try
            {
                BaseResponseModel res = order.UpdateStateOrderByOA(invoice.OrderId);

                if (res.IsSuccess)
                {
                    string filePath = order.GenerateInvoice(invoice);

                    string customerName = invoice.customerName.Replace(" ", "_"); // Thay khoảng trắng thành _
                    customerName = string.Join("", customerName.Split(Path.GetInvalidFileNameChars())); // Loại bỏ ký tự không hợp lệ
                    string uniqueFileName = $"{customerName}_{Guid.NewGuid()}.pdf";

                    byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);

                    return File(fileBytes, "application/pdf", uniqueFileName);
                }
                return BadRequest(res);
            }
            catch (Exception ex)
            {
                // Log lỗi
                Console.Error.WriteLine("Error generating invoice: " + ex.Message);
                return StatusCode(500, new { Message = "An error occurred while generating the invoice", Error = ex.Message });
            }
        }

    }
}
