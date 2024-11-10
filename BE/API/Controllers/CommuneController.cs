using BLL.Interface;
using DTO.Commune;
using DTO.Responses;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommuneController : ControllerBase
    {
        private readonly ICommune commune;

        public CommuneController (ICommune commune)
        {
            this.commune = commune;
        }

        [HttpGet]
        public IActionResult Get()
        {
            BaseResponseModel model = this.commune.Gets();

            if (model.IsSuccess)
            {
                return Ok(model);
            }
            return BadRequest(model);
        }

        [HttpGet("Get By District Id")]
        public IActionResult GetByDistrictId(int districtId)
        {
            BaseResponseModel model = this.commune.GetCommunesByDistrictIDAsync(districtId);

            if (model.IsSuccess)
            {
                return Ok(model);
            }
            return BadRequest(model);
        }

        [HttpPost]
        public IActionResult Post(CommuneRequestModule req)
        {
            BaseResponseModel model = this.commune.PostCommune(req);

            if (model.IsSuccess)
            {
                return Ok(model);
            }
            return BadRequest(model);
        }
    }
}
