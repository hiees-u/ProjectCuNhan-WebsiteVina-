using BLL.Interface;
using DTO.WarehouseReceipt;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WarehouseReceiptController : ControllerBase
    {
        private readonly IWarehouseReceipt _warehouseReceiptBLL;

        public WarehouseReceiptController(IWarehouseReceipt warehouseReceiptBLL)
        {
            _warehouseReceiptBLL = warehouseReceiptBLL;
        }

        [HttpPost("InsertWarehouseReceipt")]
        public IActionResult InsertWarehouseReceipt([FromBody] WarehouseReceiptRequestModel request)
        {
            // Map the request to the BLL method
            var response = _warehouseReceiptBLL.InsertWarehouseReceipt(request.WarehouseID, request.ReceiptDetails);

            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }
    }
}
