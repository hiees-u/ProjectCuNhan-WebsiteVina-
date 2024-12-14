using BLL.Interface;
using DLL.Models;
using DTO.DeliveryNote;
using DTO.Responses;
using DTO.WarehouseReceipt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeliveryNoteController : ControllerBase
    {
        private readonly IDeliveryNote iDeliveryNote;

        public DeliveryNoteController(IDeliveryNote DeliveryNote)
        {
            iDeliveryNote = DeliveryNote;
        }


        [HttpPost("InsertDeliveryNote")]
        [Authorize(Roles = "WarehouseEmployee")]
        public IActionResult InsertDeliveryNote([FromBody] DeliveryNoteRequestcsModel request)
        {
            // Map the request to the BLL method
            var response = iDeliveryNote.InsertDeliveryNote(request.WarehouseID, request.Note, request.DeliveryNoteDetail);

            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        [HttpGet("GetOrderIDs")]
        [Authorize(Roles = "WarehouseEmployee")]
        public IActionResult GetOrderIDs()
        {
            BaseResponseModel response = iDeliveryNote.GetOrderIDs();
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet("GetOrderDetail")]
        [Authorize(Roles = "WarehouseEmployee")]
        public IActionResult GetOrderDetail(int orderID)
        {
            BaseResponseModel response = iDeliveryNote.GetOrderDetail(orderID);
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
    }
}
