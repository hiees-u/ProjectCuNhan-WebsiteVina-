using BLL.Interface;
using DTO.Cart;
using DTO.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICart icart;

        public CartController(ICart icart)
        {
            this.icart = icart;
        }

        [HttpGet]
        [Authorize(Roles = "Customer")]
        public IActionResult Get()
        {
            BaseResponseModel response = icart.Get();
            if(response.IsSuccess)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPost]
        [Authorize(Roles = "Customer")]
        public IActionResult Post([FromBody] CartRequestModule request)
        {
            BaseResponseModel response = icart.Post(request);
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpDelete]
        [Authorize(Roles = "Customer")]
        public IActionResult Delete([FromBody] int request)
        {
            BaseResponseModel response = icart.Delete(request);
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPut]
        [Authorize(Roles = "Customer")]
        public IActionResult Put([FromBody] CartRequestModule request)
        {
            BaseResponseModel response = icart.Put(request);
            if(response.IsSuccess)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
    }
}
