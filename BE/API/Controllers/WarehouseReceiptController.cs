using BLL.Interface;
using DTO.WarehouseReceipt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WarehouseReceiptController : ControllerBase
    {
        private readonly IWarehouseReceipt iwarehouseReceipt;

        public WarehouseReceiptController(IWarehouseReceipt warehouseReceipt)
        {
            iwarehouseReceipt = warehouseReceipt;
        }

        [HttpPost("InsertWarehouseReceipt")]
        public IActionResult InsertWarehouseReceipt([FromBody] WarehouseReceiptRequestModel request)
        {
            // Map the request to the BLL method
            var response = iwarehouseReceipt.InsertWarehouseReceipt(request.WarehouseID, request.ReceiptDetails);

            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        [HttpDelete("DeleteWarehouseReceipt/{warehouseReceipID}")]
         [Authorize(Roles = "WarehouseEmployee")]
        public IActionResult Delete(int warehouseReceipID)
        {
            var result = this.iwarehouseReceipt.Delete(warehouseReceipID);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpGet("GetWarehouseReceiptInfo/{warehouseReceiptID}")]
        [Authorize(Roles = "WarehouseEmployee")]
        public IActionResult GetWarehouseReceiptInfo(int warehouseReceiptID)
        {
            var result = iwarehouseReceipt.GetWarehouseReceiptInfo(warehouseReceiptID);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpGet("GetWarehouseReceiptsByWarehouse/{warehouseID}")]
        [Authorize(Roles = "WarehouseEmployee")]
        public IActionResult GetWarehouseReceiptsByWarehouse(int warehouseID)
        {
            var result = iwarehouseReceipt.GetWarehouseReceiptsByWarehouse(warehouseID);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
    }
}
