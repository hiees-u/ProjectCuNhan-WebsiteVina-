using BLL.Interface;
using DLL.Models;
using DTO.Responses;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        ICategory category;

        public CategoryController(ICategory category) {
            this.category = category;
        }

        [HttpGet]
        public IActionResult GetCateNameByProductID([FromQuery] int productID) 
        {
            BaseResponseModel result = category.GetCateNameByProductID(productID);

            if (result.IsSuccess)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpGet("Get 10 Category")]
        public IActionResult GetTop10CateName()
        {
            BaseResponseModel result = category.GetTop10();

            if (result.IsSuccess)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }
    }
}
