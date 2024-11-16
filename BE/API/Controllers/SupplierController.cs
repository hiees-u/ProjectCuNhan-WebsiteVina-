using BLL.Interface;
using DTO.Responses;
using DTO.Supplier;
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
        public IActionResult GetAll(int? productId = null)
        {
            BaseResponseModel result = supplier.GetPagition(productId);

            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpPut()]
        public IActionResult Put([FromBody] SupplierResponseModule req)
        {
            BaseResponseModel result = supplier.Put(req);

            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpPost()]
        public IActionResult Post([FromBody] SupplierRequestModule req)
        {
            BaseResponseModel result = supplier.Post(req);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpDelete()]
        public IActionResult Delete(int supplierId)
        {
            BaseResponseModel result = supplier.Delete(supplierId);
            return result.IsSuccess ? Ok(result) : BadRequest(result); 
        }
    }
}
