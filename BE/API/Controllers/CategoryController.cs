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

        [HttpGet("Get By Id Product")]
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

        [HttpGet("Get Pagition")]
        public IActionResult GetPagition(int? cateId = null, string? cateName = null, int pageNumber = 1, int pageSize = 8)
        {
            BaseResponseModel res = category.GetPagition(cateId, cateName, pageNumber, pageSize);
            return res.IsSuccess ? Ok(res) : BadRequest(res);
        }
    }
}
