using BLL.Interface;
using DTO.Responses;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DistrictController : ControllerBase
    {
        private readonly IDistrict district;

        public DistrictController(IDistrict district)
        {
            this.district = district;
        }

        [HttpGet]
        public IActionResult Get()
        {
            BaseResponseModel model = this.district.Gets();

            if (model.IsSuccess)
            {
                return Ok(model);
            }
            return BadRequest(model);
        }

        [HttpGet("Get By Province ID")]
        public IActionResult GetByDistrictId(int provinceID)
        {
            BaseResponseModel model = this.district.GetDistrictByProvinceID(provinceID);

            if (model.IsSuccess)
            {
                return Ok(model);
            }
            return BadRequest(model);
        }
    }
}
