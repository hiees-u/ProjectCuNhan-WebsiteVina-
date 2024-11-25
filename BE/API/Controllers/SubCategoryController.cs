using BLL.Interface;
using DTO.Responses;
using DTO.Subcategory;
using Microsoft.AspNetCore.Authorization;
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

        [HttpDelete]
        [Authorize(Roles = "Moderator")]
        public IActionResult Delete(int subCateId)
        {
            BaseResponseModel result = subSategory.Delete(subCateId);

            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpPost]
        [Authorize(Roles = "Moderator")]
        public IActionResult Post([FromBody] string subCateName) 
        {
            BaseResponseModel result = subSategory.Post(subCateName);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpPut]
        [Authorize(Roles = "Moderator")]
        public IActionResult Put([FromBody] SubcategoryRequestModel req)
        {
            BaseResponseModel result = subSategory.Put(req);

            return result.IsSuccess ? Ok(result) : BadRequest(result);
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
        [Authorize(Roles = "Moderator")]
        public IActionResult GetPagition(int? subCateId = null, string? subCateName = null, int pageNumber = 1, int pageSize = 8)
        {
            BaseResponseModel result = subSategory.GetPagition(subCateId, subCateName, pageNumber, pageSize);

            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
    }
}
