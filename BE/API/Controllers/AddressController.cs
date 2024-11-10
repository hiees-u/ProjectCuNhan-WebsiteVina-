using BLL.Interface;
using DTO.Address;
using DTO.Responses;
using Microsoft.AspNetCore.Authorization;
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
            return res.IsSuccess ? Ok(res) : BadRequest(res);
        }

        [HttpPost]
        public IActionResult Post(AddressRequestModule req)
        {
            BaseResponseModel model = this.address.Post(req);
            return model.IsSuccess ? Ok(model) : BadRequest(model);
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
            return model.IsSuccess ? Ok(model) : BadRequest(model);
        }

        [Authorize]
        [HttpGet("Get string address")]
        public IActionResult GetFullAddress()
        {
            BaseResponseModel req = this.address.GetAddressString();
            return req.IsSuccess ? Ok(req) : BadRequest(req);
        }
    }
}
