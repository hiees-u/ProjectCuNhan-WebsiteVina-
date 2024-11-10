using BLL.Interface;
using DTO.Address;
using DTO.Responses;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly IAddress address;

        public AddressController(IAddress address)
        {
            this.address = address;
        }

        [HttpGet]
        public IActionResult GetById(int Id)
        {
            BaseResponseModel res = this.address.GetById(Id);

            if (res.IsSuccess)
            {
                return Ok(res);
            }
            return BadRequest(res);
        }

        [HttpPost]
        public IActionResult Post(AddressRequestModule req)
        {
            BaseResponseModel model = this.address.Post(req);

            if (model.IsSuccess)
            {
                return Ok(model);
            }
            return BadRequest(model);
        }

        [HttpGet("Get Id")]
        public IActionResult GetId(int communeId, string houseNumber, string note) 
        { 
            AddressRequestModule req = new AddressRequestModule 
            {
                CommuneId = communeId, 
                HouseNumber = houseNumber, 
                Note = note 
            }; 
            BaseResponseModel model = this.address.GetAddressID(req); 
            if (model.IsSuccess) 
            { 
                return Ok(model); 
            } 
            return BadRequest(model); 
        }
    }
}
