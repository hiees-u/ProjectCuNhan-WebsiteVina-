using BLL.Interface;
using DTO.Responses;
using DTO.Supplier;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupplierController : ControllerBase
    {
        private ISupplier supplier;

        public SupplierController(ISupplier supplier)
        {
            this.supplier = supplier;
        }

        [HttpGet("Get Supplier By Id")]
        public IActionResult Get(int id)
        {
            BaseResponseModel model = supplier.GetById(id);

            if (model.IsSuccess)
            {
                return Ok(model);
            }
            return BadRequest(model);
        }

        [HttpGet("Get All")]
        [Authorize(Roles = "Moderator")]
        public IActionResult GetAll(string? cateName = null, int pageNumber = 1, int pageSize = 8)
        {
            BaseResponseModel result = supplier.GetPagition(cateName);

            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpPut()]
        [Authorize(Roles = "Moderator")]
        public IActionResult Put([FromBody] SupplierResponseModule req)
        {
            BaseResponseModel result = supplier.Put(req);

            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpPost()]
        [Authorize(Roles = "Moderator")]
        public IActionResult Post([FromBody] SupplierRequestModule req)
        {
            BaseResponseModel result = supplier.Post(req);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpDelete()]
        [Authorize(Roles = "Moderator")]
        public IActionResult Delete(int supplierId)
        {
            BaseResponseModel result = supplier.Delete(supplierId);
            return result.IsSuccess ? Ok(result) : BadRequest(result); 
        }
    }
}
