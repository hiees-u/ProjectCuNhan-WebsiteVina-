using BLL.Interface;
using DTO.Responses;
using DTO.Shevle;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShelveController : ControllerBase
    {
        private readonly IShelve i_shelve;

        public ShelveController(IShelve i_shelve)
        {
            this.i_shelve = i_shelve;
        }

        [HttpGet("GetShelveOfWarehouse/{warehouseID}")]
        [Authorize(Roles = "WarehouseEmployee")]
        public IActionResult GetShelveOfWarehousehouse(int warehouseID)
        {
            BaseResponseModel res = this.i_shelve.GetShelveOfWarehousehouse(warehouseID);
            return res.IsSuccess ? Ok(res) : BadRequest(res);
        }


        [HttpPost("PostShelve")]
        [Authorize(Roles = "WarehouseEmployee")]
        public IActionResult Post(ShelvePostRequestModule request)
        {
            BaseResponseModel model = this.i_shelve.Post(request);
            return model.IsSuccess ? Ok(model) : BadRequest(model);
        }


        [HttpPut("PutWarehouse")]
        [Authorize(Roles = "WarehouseEmployee")]
        public IActionResult Put(ShelveRequestModule request)
        {
            BaseResponseModel model = this.i_shelve.Put(request);
            return model.IsSuccess ? Ok(model) : BadRequest(model);
        }


        [HttpDelete("DeleteShelve/{shelveId}")]
         [Authorize(Roles = "WarehouseEmployee")]
        public IActionResult Delete(int shelveId)
        {
            var result = this.i_shelve.Delete(shelveId);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
    }
}
