using BLL.Interface;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProduct _product;

        public ProductController(IProduct product)
        {
            _product = product;
        }

        [HttpGet]
        public IActionResult Get(int? productId, int? cateId, int? subCateId, int? supplierId, string? productName, int pageNumber = 1, int pageSize = 10, int sortByName = 0, int sortByPrice = 0)
        {
            return Ok(_product.GetProducts(productId, cateId, subCateId, supplierId, productName, pageNumber, pageSize, sortByName, sortByPrice));
        }
    }
}
