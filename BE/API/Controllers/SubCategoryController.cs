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

            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpGet("Get 10 Sub Category")]
        public IActionResult GetTop10CateName()
        {
            BaseResponseModel result = subSategory.GetTop10();

            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpGet("Get Pagition")]
        public IActionResult GetPagition(int? subCateId = null, string? subCateName = null, int pageNumber = 1, int pageSize = 8)
        {
            BaseResponseModel result = subSategory.GetPagition(subCateId, subCateName, pageNumber, pageSize);

            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
    }
}
