//using BLL.Interface;
//using DTO.Product;
//using DTO.Responses;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;

//namespace API.Controllers
//{
//    [Route("api/[controller]")]
//    //[ApiController]
//    public class ProductController : ControllerBase
//    {
//        private readonly IProduct _product;

//        public ProductController(IProduct product)
//        {
//            _product = product;
//        }

//        [HttpGet()]
//        public IActionResult Get(int? productId, int? cateId, int? subCateId, int? supplierId, string? productName, int pageNumber = 1, int pageSize = 10, int sortByName = 0, int sortByPrice = 0)
//        {
//            return Ok(_product.GetProducts(productId, cateId, subCateId, supplierId, productName, pageNumber, pageSize, sortByName, sortByPrice));
//        }

//        [HttpGet("Get Products By Moderator")]
//        [Authorize(Roles = "Moderator")]
//        public IActionResult GetModerator(int? productId, int? cateId, int? subCateId, int? supplierId, string? productName, int pageNumber = 1, int pageSize = 10, int sortByName = 0, int sortByPrice = 0)
//        {
//            return Ok(_product.GetProductsModerator(productId, cateId, subCateId, supplierId, productName, pageNumber, pageSize, sortByName, sortByPrice));
//        }

//        [HttpPut]
//        [Authorize(Roles = "Moderator")]
//        public IActionResult Put([FromBody] ProductRequesModule req)
//        {
//            BaseResponseModel result = _product.Put(req);

//            return result.IsSuccess ? Ok(result) : BadRequest(result);
//        }

//        //Get không hiện do chưa có số lượng tồn kho... => DÙNG GetModerator
//        [HttpPost]
//        [Authorize(Roles = "Moderator")]
//        public IActionResult Post([FromBody] ProductRequestPostModule req)
//        {
//            BaseResponseModel result = _product.Post(req);

//            return result.IsSuccess ? Ok(result) : BadRequest(result) ;
//        }

//        [HttpDelete]
//        [Authorize(Roles = "Moderator")]
//        public IActionResult Delete(int productId)
//        {
//            BaseResponseModel result = _product.Delete(productId);

//            return result.IsSuccess ? Ok(result) : BadRequest(result);
//        }
//    }
//}
