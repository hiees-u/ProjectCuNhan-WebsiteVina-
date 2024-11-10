using BLL.Interface;
using DLL.Models;
using DTO.Responses;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubCategoryController : ControllerBase
    {
        ISubCategory subSategory;

        public SubCategoryController(ISubCategory subSategory)
        {
            this.subSategory = subSategory;
        }

        [HttpGet("Get By Product Id")]
        public IActionResult GetCateNameByProductID([FromQuery] int productID)
        {
            BaseResponseModel result = subSategory.GetSubCateNameByProductID(productID);

            if (result.IsSuccess)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpGet("Get 10 Sub Category")]
        public IActionResult GetTop10CateName()
        {
            BaseResponseModel result = subSategory.GetTop10();

            if (result.IsSuccess)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }
    }
}
