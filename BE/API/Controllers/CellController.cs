using BLL.Interface;
using DTO.Cells;
using DTO.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CellController : ControllerBase
    {
        private readonly ICell i_cell;

        public CellController(ICell i_cell)
        {
            this.i_cell = i_cell;
        }
        [HttpGet("GetProductsByWarehouseID/{warehouseID}")]
        [Authorize(Roles = "WarehouseEmployee")]
        public IActionResult GetProductsByWarehouseID(int warehouseID)
        {
            BaseResponseModel res = this.i_cell.GetProductsByWarehouseID(warehouseID);
            return res.IsSuccess ? Ok(res) : BadRequest(res);
        }

        [HttpGet("GetCellByShelve/{shelveID}")]
        [Authorize(Roles = "WarehouseEmployee")]
        public IActionResult GetCellByShelve(int shelveID)
        {
            BaseResponseModel res = this.i_cell.GetCellByShelve(shelveID);
            return res.IsSuccess ? Ok(res) : BadRequest(res);
        }

        [HttpPost("PostCell")]
        [Authorize(Roles = "WarehouseEmployee")]
        public IActionResult Post(CellPostRequestModule request)
        {
            BaseResponseModel model = this.i_cell.Post(request);
            return model.IsSuccess ? Ok(model) : BadRequest(model);
        }

        [HttpPut("PutCell")]
        [Authorize(Roles = "WarehouseEmployee")]
        public IActionResult Put(CellRequestModule request)
        {
            BaseResponseModel model = this.i_cell.Put(request);
            return model.IsSuccess ? Ok(model) : BadRequest(model);
        }

        [HttpDelete("DeleteCell/{cellID}")]
         [Authorize(Roles = "WarehouseEmployee")]
        public IActionResult Delete(int cellID)
        {
            var result = this.i_cell.Delete(cellID);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
        [HttpGet("GetProductsExpriryDate")]
        [Authorize(Roles = "WarehouseEmployee")]
        public IActionResult GetProductsExpriryDate()
        {
            BaseResponseModel res = this.i_cell.GetProductsExpriryDate();
            return res.IsSuccess ? Ok(res) : BadRequest(res);
        }
        [HttpGet("GetInfoProductsByProductID")]
        [Authorize(Roles = "WarehouseEmployee")]
        public IActionResult GetInfoProductsByProductID(int productId)
        {
            BaseResponseModel res = this.i_cell.GetInfoProductsByProductID(productId);
            return res.IsSuccess ? Ok(res) : BadRequest(res);
        }
        [HttpGet("GetAllProducts")]
        [Authorize(Roles = "WarehouseEmployee")]
        public IActionResult GetAllProducts()
        {
            BaseResponseModel res = this.i_cell.GetAllProducts();
            return res.IsSuccess ? Ok(res) : BadRequest(res);
        }
    }
}
