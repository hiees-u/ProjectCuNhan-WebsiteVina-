using BLL.Interface;
using DLL.Models;
using DTO.Category;
using DTO.Responses;
using Microsoft.AspNetCore.Authorization;
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

        [HttpDelete]
        [Authorize(Roles = "Moderator")]
        public ActionResult Delete(int cateId)
        {
            BaseResponseModel res = category.Delete(cateId);

            return res.IsSuccess ? Ok(res) : BadRequest(res);
        }

        [HttpPost]
        [Authorize(Roles = "Moderator")]
        public ActionResult Post([FromBody]string cateName)
        {
            BaseResponseModel result = category.Post(cateName);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpPut]
        [Authorize(Roles = "Moderator")]
        public ActionResult Put([FromBody] CategoryRequestModule req)
        {
            BaseResponseModel result = category.Put(req);

            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpGet("Get By Id Product")]
        public IActionResult GetCateNameByProductID(int productID) 
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
        [Authorize(Roles = "Moderator")]
        public IActionResult GetPagition(int? cateId = null, string? cateName = null, int pageNumber = 1, int pageSize = 8)
        {
            BaseResponseModel res = category.GetPagition(cateId, cateName, pageNumber, pageSize);
            return res.IsSuccess ? Ok(res) : BadRequest(res);
        }
    }
}
