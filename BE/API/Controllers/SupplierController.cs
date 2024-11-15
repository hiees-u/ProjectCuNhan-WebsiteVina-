using BLL.Interface;
using DTO.Responses;
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
    }
}
