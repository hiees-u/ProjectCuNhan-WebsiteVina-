using BLL.Interface;
using DTO.Responses;
using DTO.WareHouse;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WarehouseController : ControllerBase
    {
        private readonly IWareHouse iwarehouse;

        public WarehouseController(IWareHouse iwarehouse)
        {
            this.iwarehouse = iwarehouse;
        }
        [HttpGet("GetWarehouse")]
        //[Authorize(Roles = "WarehouseEmployee")]
        public IActionResult Get()
        {
            BaseResponseModel response = iwarehouse.Get();
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
        [HttpGet("GetInForWarehouseID/{warehouseID}")]
        //[Authorize(Roles = "WarehouseEmployee")]
        public IActionResult GetWareHouseID(int warehouseID)
        {
            BaseResponseModel res = this.iwarehouse.GetWareHouseID(warehouseID);
            return res.IsSuccess ? Ok(res) : BadRequest(res);
        }
        [HttpPost("PostWarehouse")]
        //[Authorize(Roles = "WarehouseEmployee")]
        public IActionResult Post(WareHousePostRequestModule request)
        {
            BaseResponseModel model = this.iwarehouse.Post(request);
            return model.IsSuccess ? Ok(model) : BadRequest(model);
        }
        [HttpPut("PutWarehouse")]
        //[Authorize(Roles = "WarehouseEmployee")]
        public IActionResult Put(WareHouseRequestModule request)
        {
            BaseResponseModel model = this.iwarehouse.Put(request);
            return model.IsSuccess ? Ok(model) : BadRequest(model);
        }

        [HttpDelete("DeleteWareHouse/{warehouseId}")]
        // [Authorize(Roles = "WarehouseEmployee")]
        public IActionResult Delete(int warehouseId)
        {
            var result = this.iwarehouse.Delete(warehouseId);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
    }
}
