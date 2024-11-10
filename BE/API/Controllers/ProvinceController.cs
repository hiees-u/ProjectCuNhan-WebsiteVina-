using BLL.Interface;
using DTO.Responses;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProvinceController : ControllerBase
    {
        private readonly IProvince province;

        public ProvinceController(IProvince province)
        {
            this.province = province;
        }

        [HttpGet]
        public IActionResult Get()
        {
            BaseResponseModel model = this.province.Gets();

            if(model.IsSuccess)
            {
                return Ok(model);
            }
            return BadRequest(model);
        }
    }
}
